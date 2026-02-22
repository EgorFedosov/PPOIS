from __future__ import annotations

from PyQt6.QtCore import QModelIndex, QSortFilterProxyModel, Qt

from models.book_model import (
    AUTHOR_COL,
    CIRCULATION_COL,
    PUBLISHER_COL,
    QueryCriteria,
    TITLE_COL,
    TOTAL_TOMES_COL,
    VOLUMES_COL,
)


class BooksFilterProxyModel(QSortFilterProxyModel):
    def __init__(self, parent=None) -> None:
        super().__init__(parent)
        self._criteria: QueryCriteria | None = None
        self.setFilterCaseSensitivity(Qt.CaseSensitivity.CaseInsensitive)

    def set_criteria(self, criteria: QueryCriteria | None) -> None:
        self._criteria = criteria.normalized() if criteria else None
        self.invalidateFilter()

    def filterAcceptsRow(self, source_row: int, source_parent: QModelIndex) -> bool:
        if self._criteria is None or self._criteria.is_empty():
            return True

        model = self.sourceModel()
        if model is None:
            return False

        criteria = self._criteria
        author = str(model.data(model.index(source_row, AUTHOR_COL, source_parent), Qt.ItemDataRole.DisplayRole) or "")
        publisher = str(
            model.data(model.index(source_row, PUBLISHER_COL, source_parent), Qt.ItemDataRole.DisplayRole) or ""
        )
        title = str(model.data(model.index(source_row, TITLE_COL, source_parent), Qt.ItemDataRole.DisplayRole) or "")
        volumes = int(model.data(model.index(source_row, VOLUMES_COL, source_parent), Qt.ItemDataRole.DisplayRole) or 0)
        circulation = int(
            model.data(model.index(source_row, CIRCULATION_COL, source_parent), Qt.ItemDataRole.DisplayRole) or 0
        )
        total_tomes = int(
            model.data(model.index(source_row, TOTAL_TOMES_COL, source_parent), Qt.ItemDataRole.DisplayRole) or 0
        )

        if criteria.author is not None and author.casefold() != criteria.author.casefold():
            return False

        if criteria.publisher is not None and publisher.casefold() != criteria.publisher.casefold():
            return False

        if criteria.title is not None and title.casefold() != criteria.title.casefold():
            return False

        if criteria.min_volumes is not None and volumes < criteria.min_volumes:
            return False

        if criteria.max_volumes is not None and volumes > criteria.max_volumes:
            return False

        if criteria.circulation_cmp == "gt" and criteria.circulation_value is not None:
            if circulation <= criteria.circulation_value:
                return False

        if criteria.circulation_cmp == "lt" and criteria.circulation_value is not None:
            if circulation >= criteria.circulation_value:
                return False

        if criteria.total_cmp == "gt" and criteria.total_value is not None:
            if total_tomes <= criteria.total_value:
                return False

        if criteria.total_cmp == "lt" and criteria.total_value is not None:
            if total_tomes >= criteria.total_value:
                return False

        return True


class PagingProxyModel(QSortFilterProxyModel):
    def __init__(self, parent=None) -> None:
        super().__init__(parent)
        self._page_size = 10
        self._current_page = 1

    @property
    def page_size(self) -> int:
        return self._page_size

    @property
    def current_page(self) -> int:
        return self._current_page

    @property
    def total_rows(self) -> int:
        model = self.sourceModel()
        return 0 if model is None else model.rowCount()

    @property
    def total_pages(self) -> int:
        if self.total_rows == 0:
            return 0
        return (self.total_rows + self._page_size - 1) // self._page_size

    def set_page_size(self, value: int) -> None:
        self._page_size = max(1, value)
        self._current_page = 1
        self.invalidateFilter()

    def set_page(self, value: int) -> None:
        if self.total_pages == 0:
            self._current_page = 1
            self.invalidateFilter()
            return
        self._current_page = min(max(1, value), self.total_pages)
        self.invalidateFilter()

    def first_page(self) -> None:
        self.set_page(1)

    def last_page(self) -> None:
        self.set_page(self.total_pages)

    def next_page(self) -> None:
        self.set_page(self._current_page + 1)

    def previous_page(self) -> None:
        self.set_page(self._current_page - 1)

    def filterAcceptsRow(self, source_row: int, source_parent: QModelIndex) -> bool:
        if self.total_rows == 0:
            return False
        start = (self._current_page - 1) * self._page_size
        end = start + self._page_size
        return start <= source_row < end
