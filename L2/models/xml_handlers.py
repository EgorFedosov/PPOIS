from __future__ import annotations

from pathlib import Path
from xml.dom import minidom
from xml.sax import ContentHandler, make_parser

from models.book_model import Book


def write_books_to_xml(books: list[Book], file_path: Path) -> None:
    path = Path(file_path)
    path.parent.mkdir(parents=True, exist_ok=True)

    document = minidom.Document()
    root = document.createElement("library")
    document.appendChild(root)

    for book in books:
        book_node = document.createElement("book")
        root.appendChild(book_node)
        _append_text_node(document, book_node, "title", book.title)
        _append_text_node(document, book_node, "author", book.author)
        _append_text_node(document, book_node, "publisher", book.publisher)
        _append_text_node(document, book_node, "volumes", str(book.volumes))
        _append_text_node(document, book_node, "circulation", str(book.circulation))

    with path.open("w", encoding="utf-8") as xml_file:
        document.writexml(xml_file, addindent="  ", newl="\n", encoding="utf-8")


def read_books_from_xml(file_path: Path) -> list[Book]:
    parser = make_parser()
    handler = _BooksSaxHandler()
    parser.setContentHandler(handler)
    parser.parse(str(file_path))
    return handler.books


def _append_text_node(
    document: minidom.Document,
    parent: minidom.Element,
    tag_name: str,
    text: str,
) -> None:
    node = document.createElement(tag_name)
    node.appendChild(document.createTextNode(text))
    parent.appendChild(node)


def _safe_int(value: str, default: int = 0) -> int:
    try:
        return int(value)
    except (TypeError, ValueError):
        return default


class _BooksSaxHandler(ContentHandler):
    def __init__(self) -> None:
        super().__init__()
        self.books: list[Book] = []
        self._current_tag = ""
        self._text_buffer: list[str] = []
        self._record: dict[str, str] = {}

    def startElement(self, name: str, attrs):  # noqa: N802
        self._current_tag = name
        self._text_buffer = []
        if name == "book":
            self._record = {}

    def characters(self, content: str):  # noqa: N802
        if self._current_tag:
            self._text_buffer.append(content)

    def endElement(self, name: str):  # noqa: N802
        text = "".join(self._text_buffer).strip()
        if name in {"title", "author", "publisher", "volumes", "circulation"}:
            self._record[name] = text
        if name == "book":
            self.books.append(
                Book(
                    title=self._record.get("title", ""),
                    author=self._record.get("author", ""),
                    publisher=self._record.get("publisher", ""),
                    volumes=_safe_int(self._record.get("volumes", "0")),
                    circulation=_safe_int(self._record.get("circulation", "0")),
                )
            )
        self._current_tag = ""
        self._text_buffer = []
