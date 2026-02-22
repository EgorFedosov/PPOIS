from __future__ import annotations

from pathlib import Path

from PyQt6.QtCore import Qt
from PyQt6.QtSql import QSqlTableModel
from PyQt6.QtWidgets import QFileDialog, QMessageBox

from models.book_model import (
    AUTHOR_COL,
    CIRCULATION_COL,
    PUBLISHER_COL,
    TABLE_HEADERS,
    TITLE_COL,
    TOTAL_TOMES_COL,
    VOLUMES_COL,
)
from models.database import DatabaseManager
from models.proxy_models import BooksFilterProxyModel, PagingProxyModel
from models.xml_handlers import read_books_from_xml, write_books_to_xml
from views.dialogs import AddBookDialog, DeleteDialog, SearchDialog
from views.main_window import MainWindow


class AppController:
    def __init__(self, window: MainWindow, db_manager: DatabaseManager) -> None:
        self.window = window
        self.db_manager = db_manager

        self.table_model: QSqlTableModel | None = None
        self.filter_proxy: BooksFilterProxyModel | None = None
        self.page_proxy: PagingProxyModel | None = None

        self._setup_model_stack()
        self._connect_signals()
        self._refresh_view()

    def _setup_model_stack(self) -> None:
        self.table_model = QSqlTableModel(self.window, self.db_manager.db)
        self.table_model.setTable("books")
        self.table_model.setEditStrategy(QSqlTableModel.EditStrategy.OnManualSubmit)

        for column, header in TABLE_HEADERS.items():
            self.table_model.setHeaderData(column, Qt.Orientation.Horizontal, header)

        if not self.table_model.select():
            raise RuntimeError(self.table_model.lastError().text())

        self.filter_proxy = BooksFilterProxyModel(self.window)
        self.filter_proxy.setSourceModel(self.table_model)

        self.page_proxy = PagingProxyModel(self.window)
        self.page_proxy.setSourceModel(self.filter_proxy)
        self.page_proxy.set_page_size(self.window.pagination.page_size_spin.value())

        self.window.set_table_model(self.page_proxy)

    def _connect_signals(self) -> None:
        self.window.add_action.triggered.connect(self._open_add_dialog)
        self.window.search_action.triggered.connect(self._open_search_dialog)
        self.window.delete_action.triggered.connect(self._open_delete_dialog)
        self.window.save_xml_action.triggered.connect(self._save_to_xml)
        self.window.load_xml_action.triggered.connect(self._load_from_xml)
        self.window.open_db_action.triggered.connect(self._choose_database)
        self.window.exit_action.triggered.connect(self.window.close)

        self.window.view_table_action.triggered.connect(self.window.show_table)
        self.window.view_tree_action.triggered.connect(self.window.show_tree)

        self.window.pagination.first_requested.connect(lambda: self._navigate_page("first_page"))
        self.window.pagination.previous_requested.connect(lambda: self._navigate_page("previous_page"))
        self.window.pagination.next_requested.connect(lambda: self._navigate_page("next_page"))
        self.window.pagination.last_requested.connect(lambda: self._navigate_page("last_page"))
        self.window.pagination.page_size_changed.connect(self._change_page_size)

    def _open_add_dialog(self) -> None:
        if self.table_model is None:
            return

        try:
            dialog = AddBookDialog(self.table_model, self.window)
            if not dialog.exec():
                return

            self._select_table_model()
            self._refresh_view()
        except Exception as error:
            self._show_error("Не удалось добавить запись", error)

    def _open_search_dialog(self) -> None:
        if self.table_model is None:
            return

        try:
            dialog = SearchDialog(self.table_model, self.window)
            dialog.exec()
        except Exception as error:
            self._show_error("Не удалось открыть поиск", error)

    def _open_delete_dialog(self) -> None:
        dialog = DeleteDialog(self.window)
        if not dialog.exec():
            return

        try:
            deleted_count = self.db_manager.delete_by_criteria(dialog.to_criteria())
            self._sync_after_data_change()
        except Exception as error:
            self._show_error("Не удалось удалить записи", error)
            return

        if deleted_count:
            QMessageBox.information(self.window, "Удаление", f"Удалено записей: {deleted_count}.")
        else:
            QMessageBox.information(self.window, "Удаление", "По заданным условиям записи не найдены.")

    def _save_to_xml(self) -> None:
        data_dir = self._data_dir()
        filename, _ = QFileDialog.getSaveFileName(
            self.window,
            "Сохранить XML",
            str(Path(data_dir) / "books_export.xml"),
            "XML Files (*.xml)",
        )
        if not filename:
            return

        try:
            books = self.db_manager.fetch_all_books()
            write_books_to_xml(books, Path(filename))
        except Exception as error:
            self._show_error("Не удалось сохранить XML", error)
            return

        QMessageBox.information(self.window, "Сохранение", "Данные успешно сохранены в XML.")

    def _load_from_xml(self) -> None:
        data_dir = self._data_dir()
        filename, _ = QFileDialog.getOpenFileName(
            self.window,
            "Загрузить XML",
            data_dir,
            "XML Files (*.xml)",
        )
        if not filename:
            return

        try:
            books = read_books_from_xml(Path(filename))
            self.db_manager.replace_all_books(books)
            self._sync_after_data_change()
        except Exception as error:
            self._show_error("Не удалось загрузить XML", error)
            return

        QMessageBox.information(self.window, "Загрузка", "Данные успешно загружены из XML.")

    def _choose_database(self) -> None:
        filename, _ = QFileDialog.getSaveFileName(
            self.window,
            "Выберите файл базы данных",
            str(self.db_manager.db_path),
            "SQLite Database (*.db)",
        )
        if not filename:
            return

        try:
            self.db_manager.switch_database(Path(filename))
            self._setup_model_stack()
            self._refresh_view()
        except Exception as error:
            self._show_error("Не удалось открыть базу данных", error)
            return

        QMessageBox.information(self.window, "База данных", f"Текущая БД: {self.db_manager.db_path}")

    def _change_page_size(self, page_size: int) -> None:
        if self.page_proxy is None:
            return
        self.page_proxy.set_page_size(page_size)
        self._refresh_view()

    def _navigate_page(self, action: str) -> None:
        if self.page_proxy is None:
            return
        getattr(self.page_proxy, action)()
        self._refresh_view()

    def _normalize_page_after_data_change(self) -> None:
        if self.page_proxy is None:
            return

        if self.page_proxy.total_pages == 0:
            self.page_proxy.set_page(1)
            return

        if self.page_proxy.current_page > self.page_proxy.total_pages:
            self.page_proxy.set_page(self.page_proxy.total_pages)
        else:
            self.page_proxy.set_page(self.page_proxy.current_page)

    def _refresh_view(self) -> None:
        if self.page_proxy is None:
            return

        total_pages = self.page_proxy.total_pages
        current_page = 0 if total_pages == 0 else self.page_proxy.current_page
        self.window.pagination.set_page_size(self.page_proxy.page_size)
        self.window.pagination.set_info(
            current_page,
            total_pages,
            self.page_proxy.rowCount(),
            self.page_proxy.total_rows,
        )
        self.window.table_view.resizeColumnsToContents()
        self.window.set_tree_rows(self._collect_tree_rows())

    def _collect_tree_rows(self) -> list[dict[str, str]]:
        if self.page_proxy is None:
            return []

        columns = (
            ("title", TITLE_COL),
            ("author", AUTHOR_COL),
            ("publisher", PUBLISHER_COL),
            ("volumes", VOLUMES_COL),
            ("circulation", CIRCULATION_COL),
            ("total_tomes", TOTAL_TOMES_COL),
        )
        rows: list[dict[str, str]] = []
        for row in range(self.page_proxy.rowCount()):
            rows.append(
                {
                    key: str(self.page_proxy.data(self.page_proxy.index(row, col)) or "")
                    for key, col in columns
                }
            )
        return rows

    def _data_dir(self) -> str:
        return str(self.db_manager.db_path.parent)

    def _show_error(self, message: str, error: Exception) -> None:
        QMessageBox.critical(self.window, "Ошибка", f"{message}:\n{error}")

    def _select_table_model(self) -> None:
        if self.table_model is None:
            return
        if not self.table_model.select():
            raise RuntimeError(self.table_model.lastError().text())

    def _sync_after_data_change(self) -> None:
        self._select_table_model()
        self._normalize_page_after_data_change()
        self._refresh_view()
