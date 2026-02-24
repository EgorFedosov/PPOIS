from __future__ import annotations

from pathlib import Path

from PyQt6.QtSql import QSqlDatabase, QSqlQuery

from models.book_model import Book, QueryCriteria, build_where_clause

_SCHEMA_SQL = (
    "CREATE TABLE IF NOT EXISTS books ("
    "id INTEGER PRIMARY KEY AUTOINCREMENT,"
    "title TEXT NOT NULL,"
    "author TEXT NOT NULL,"
    "publisher TEXT NOT NULL,"
    "volumes INTEGER NOT NULL,"
    "circulation INTEGER NOT NULL,"
    "total_tomes INTEGER NOT NULL DEFAULT 0)"
)
_SELECT_SQL = "SELECT title, author, publisher, volumes, circulation FROM books ORDER BY id"
_INSERT_SQL = (
    "INSERT INTO books (title, author, publisher, volumes, circulation) "
    "VALUES (:title, :author, :publisher, :volumes, :circulation)"
)
_TRIGGER_SQL = (
    "CREATE TRIGGER IF NOT EXISTS books_total_tomes_after_insert AFTER INSERT ON books "
    "BEGIN UPDATE books SET total_tomes = NEW.volumes * NEW.circulation WHERE id = NEW.id; END",
    "CREATE TRIGGER IF NOT EXISTS books_total_tomes_after_update AFTER UPDATE OF volumes, circulation ON books "
    "BEGIN UPDATE books SET total_tomes = NEW.volumes * NEW.circulation WHERE id = NEW.id; END",
)


class DatabaseManager:
    def __init__(self, db_path: Path, connection_name: str = "library_connection") -> None:
        self._connection_name = connection_name
        self._db_path = Path(db_path)
        self._db = self._open_connection(self._db_path)
        self.ensure_schema()

    @property
    def db_path(self) -> Path:
        return self._db_path

    @property
    def db(self) -> QSqlDatabase:
        return self._db

    def switch_database(self, db_path: Path) -> None:
        self._db_path = Path(db_path)
        if self._db.isOpen():
            self._db.close()
        self._db.setDatabaseName(str(self._db_path))
        if not self._db.open():
            raise RuntimeError(self._db.lastError().text())
        self.ensure_schema()

    def ensure_schema(self) -> None:
        self._db_path.parent.mkdir(parents=True, exist_ok=True)
        self._exec(_SCHEMA_SQL)
        if not self._has_column("books", "total_tomes"):
            self._exec("ALTER TABLE books ADD COLUMN total_tomes INTEGER NOT NULL DEFAULT 0")
        self._exec("UPDATE books SET total_tomes = volumes * circulation")
        for sql in _TRIGGER_SQL:
            self._exec(sql)

    def fetch_all_books(self) -> list[Book]:
        query = QSqlQuery(self._db)
        if not query.exec(_SELECT_SQL):
            raise RuntimeError(query.lastError().text())

        books: list[Book] = []
        while query.next():
            books.append(Book(str(query.value(0)), str(query.value(1)), str(query.value(2)), int(query.value(3)), int(query.value(4))))
        return books

    def replace_all_books(self, books: list[Book]) -> None:
        if not self._db.transaction():
            raise RuntimeError(self._db.lastError().text())

        try:
            self._exec("DELETE FROM books")
            insert = QSqlQuery(self._db)
            if not insert.prepare(_INSERT_SQL):
                raise RuntimeError(insert.lastError().text())

            for book in books:
                for key, value in ((":title", book.title), (":author", book.author), (":publisher", book.publisher), (":volumes", book.volumes), (":circulation", book.circulation)):
                    insert.bindValue(key, value)
                if not insert.exec():
                    raise RuntimeError(insert.lastError().text())

            if not self._db.commit():
                raise RuntimeError(self._db.lastError().text())
        except Exception:
            self._db.rollback()
            raise

    def delete_by_criteria(self, criteria: QueryCriteria) -> int:
        where, params = build_where_clause(criteria)
        if not where:
            return 0

        query = QSqlQuery(self._db)
        if not query.prepare(f"DELETE FROM books WHERE {where}"):
            raise RuntimeError(query.lastError().text())
        for key, value in params.items():
            query.bindValue(key, value)
        if not query.exec():
            raise RuntimeError(query.lastError().text())
        return max(0, int(query.numRowsAffected()))

    def _open_connection(self, db_path: Path) -> QSqlDatabase:
        db_path.parent.mkdir(parents=True, exist_ok=True)
        db = QSqlDatabase.database(self._connection_name) if QSqlDatabase.contains(self._connection_name) else QSqlDatabase.addDatabase("QSQLITE", self._connection_name)
        db.setDatabaseName(str(db_path))
        if not db.open():
            raise RuntimeError(db.lastError().text())
        return db

    def _has_column(self, table_name: str, column_name: str) -> bool:
        query = QSqlQuery(self._db)
        if not query.exec(f"PRAGMA table_info({table_name})"):
            raise RuntimeError(query.lastError().text())
        while query.next():
            if str(query.value(1)) == column_name:
                return True
        return False

    def _exec(self, sql: str) -> None:
        query = QSqlQuery(self._db)
        if not query.exec(sql):
            raise RuntimeError(query.lastError().text())