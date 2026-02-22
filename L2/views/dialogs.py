from __future__ import annotations

from PyQt6.QtCore import pyqtSignal
from PyQt6.QtWidgets import (
    QComboBox,
    QDataWidgetMapper,
    QDialog,
    QDialogButtonBox,
    QFormLayout,
    QGroupBox,
    QHBoxLayout,
    QLabel,
    QLineEdit,
    QMessageBox,
    QPushButton,
    QSpinBox,
    QTableView,
    QVBoxLayout,
    QWidget,
)

from models.book_model import (
    AUTHOR_COL,
    CIRCULATION_COL,
    PUBLISHER_COL,
    QueryCriteria,
    TITLE_COL,
    TOTAL_TOMES_COL,
    VOLUMES_COL,
)
from models.proxy_models import BooksFilterProxyModel, PagingProxyModel
from views.main_window import PaginationPanel


class CriteriaWidget(QWidget):
    def __init__(self, parent: QWidget | None = None) -> None:
        super().__init__(parent)

        layout = QVBoxLayout(self)

        form = QFormLayout()
        self.author_edit = QLineEdit()
        self.publisher_edit = QLineEdit()
        self.title_edit = QLineEdit()
        form.addRow("ФИО автора:", self.author_edit)
        form.addRow("Издательство:", self.publisher_edit)
        form.addRow("Название книги:", self.title_edit)
        layout.addLayout(form)

        volumes_group = QGroupBox("Число томов (диапазон)")
        volumes_layout = QHBoxLayout(volumes_group)
        self.min_volumes_spin = QSpinBox()
        self.max_volumes_spin = QSpinBox()
        self.min_volumes_spin.setRange(0, 1_000_000)
        self.max_volumes_spin.setRange(0, 1_000_000)
        self.min_volumes_spin.setSpecialValueText("не задан")
        self.max_volumes_spin.setSpecialValueText("не задан")
        volumes_layout.addWidget(QLabel("От"))
        volumes_layout.addWidget(self.min_volumes_spin)
        volumes_layout.addWidget(QLabel("До"))
        volumes_layout.addWidget(self.max_volumes_spin)
        layout.addWidget(volumes_group)

        self.circulation_cmp, self.circulation_value = self._add_comparison_group(layout, "Тираж")
        self.total_cmp, self.total_value = self._add_comparison_group(layout, "Итого томов")

    def to_criteria(self) -> QueryCriteria:
        min_volumes = self.min_volumes_spin.value()
        max_volumes = self.max_volumes_spin.value()
        circulation_cmp = _cmp_from_text(self.circulation_cmp.currentText())
        total_cmp = _cmp_from_text(self.total_cmp.currentText())

        return QueryCriteria(
            author=self.author_edit.text(),
            publisher=self.publisher_edit.text(),
            title=self.title_edit.text(),
            min_volumes=min_volumes if min_volumes > 0 else None,
            max_volumes=max_volumes if max_volumes > 0 else None,
            circulation_cmp=circulation_cmp,
            circulation_value=self.circulation_value.value() if circulation_cmp else None,
            total_cmp=total_cmp,
            total_value=self.total_value.value() if total_cmp else None,
        ).normalized()

    def _add_comparison_group(self, layout: QVBoxLayout, title: str) -> tuple[QComboBox, QSpinBox]:
        group = QGroupBox(title)
        group_layout = QHBoxLayout(group)
        cmp_box = QComboBox()
        cmp_box.addItems(["не задано", ">", "<"])
        value_spin = QSpinBox()
        value_spin.setRange(0, 1_000_000_000)
        group_layout.addWidget(cmp_box)
        group_layout.addWidget(value_spin)
        layout.addWidget(group)
        return cmp_box, value_spin


class AddBookDialog(QDialog):
    def __init__(self, table_model, parent: QWidget | None = None) -> None:
        super().__init__(parent)
        self.setWindowTitle("Добавить запись")
        self._table_model = table_model

        layout = QVBoxLayout(self)
        form = QFormLayout()

        self.title_edit = QLineEdit()
        self.author_edit = QLineEdit()
        self.publisher_edit = QLineEdit()
        self.volumes_spin = QSpinBox()
        self.circulation_spin = QSpinBox()

        self.volumes_spin.setRange(1, 1_000_000)
        self.circulation_spin.setRange(1, 1_000_000_000)
        self.volumes_spin.setValue(1)
        self.circulation_spin.setValue(1)

        form.addRow("Название книги:", self.title_edit)
        form.addRow("ФИО автора:", self.author_edit)
        form.addRow("Издательство:", self.publisher_edit)
        form.addRow("Число томов:", self.volumes_spin)
        form.addRow("Тираж:", self.circulation_spin)
        layout.addLayout(form)

        buttons = QDialogButtonBox(QDialogButtonBox.StandardButton.Ok | QDialogButtonBox.StandardButton.Cancel)
        buttons.accepted.connect(self._save)
        buttons.rejected.connect(self.reject)
        layout.addWidget(buttons)

        self._mapper = QDataWidgetMapper(self)
        self._mapper.setModel(self._table_model)
        self._mapper.setSubmitPolicy(QDataWidgetMapper.SubmitPolicy.ManualSubmit)
        self._mapper.addMapping(self.title_edit, TITLE_COL)
        self._mapper.addMapping(self.author_edit, AUTHOR_COL)
        self._mapper.addMapping(self.publisher_edit, PUBLISHER_COL)
        self._mapper.addMapping(self.volumes_spin, VOLUMES_COL)
        self._mapper.addMapping(self.circulation_spin, CIRCULATION_COL)

        self._new_row = self._table_model.rowCount()
        if not self._table_model.insertRow(self._new_row):
            raise RuntimeError("Не удалось добавить строку в модель")

        self._table_model.setData(self._table_model.index(self._new_row, TOTAL_TOMES_COL), 0)
        self._mapper.setCurrentIndex(self._new_row)

    def _save(self) -> None:
        if not self.title_edit.text().strip() or not self.author_edit.text().strip() or not self.publisher_edit.text().strip():
            QMessageBox.warning(self, "Ошибка ввода", "Заполните поля: название, автор и издательство.")
            return

        if not self._mapper.submit():
            QMessageBox.critical(self, "Ошибка", "Не удалось передать данные в модель.")
            return

        if not self._table_model.submitAll():
            QMessageBox.critical(self, "Ошибка", self._table_model.lastError().text())
            self._table_model.revertAll()
            return

        self.accept()

    def reject(self) -> None:
        self._table_model.revertAll()
        super().reject()


class SearchDialog(QDialog):
    criteria_applied = pyqtSignal(QueryCriteria)

    def __init__(self, source_model, parent: QWidget | None = None) -> None:
        super().__init__(parent)
        self.setWindowTitle("Поиск записей")
        self.resize(980, 620)

        self._filter_proxy = BooksFilterProxyModel(self)
        self._filter_proxy.setSourceModel(source_model)
        self._page_proxy = PagingProxyModel(self)
        self._page_proxy.setSourceModel(self._filter_proxy)
        self._page_proxy.set_page_size(10)

        layout = QVBoxLayout(self)
        self.criteria = CriteriaWidget()
        layout.addWidget(self.criteria)

        controls = QHBoxLayout()
        self.search_button = QPushButton("Найти")
        self.clear_button = QPushButton("Сбросить фильтр")
        controls.addWidget(self.search_button)
        controls.addWidget(self.clear_button)
        controls.addStretch(1)
        layout.addLayout(controls)

        self.table = QTableView()
        self.table.setModel(self._page_proxy)
        self.table.hideColumn(0)
        self.table.horizontalHeader().setStretchLastSection(True)
        self.table.verticalHeader().setVisible(False)
        layout.addWidget(self.table)

        self.pagination = PaginationPanel()
        layout.addWidget(self.pagination)

        close_buttons = QDialogButtonBox(QDialogButtonBox.StandardButton.Close)
        close_buttons.rejected.connect(self.reject)
        layout.addWidget(close_buttons)

        self.search_button.clicked.connect(self._apply_search)
        self.clear_button.clicked.connect(self._clear_search)
        self.pagination.first_requested.connect(lambda: self._navigate_page("first_page"))
        self.pagination.previous_requested.connect(lambda: self._navigate_page("previous_page"))
        self.pagination.next_requested.connect(lambda: self._navigate_page("next_page"))
        self.pagination.last_requested.connect(lambda: self._navigate_page("last_page"))
        self.pagination.page_size_changed.connect(self._set_page_size)

        self._refresh_info()

    def _apply_search(self) -> None:
        criteria = self.criteria.to_criteria()
        if criteria.is_empty():
            QMessageBox.warning(self, "Ошибка ввода", "Введите хотя бы одно условие поиска.")
            return

        self._filter_proxy.set_criteria(criteria)
        self._page_proxy.first_page()
        self.criteria_applied.emit(criteria)
        self._refresh_info()
        self.table.resizeColumnsToContents()

    def _clear_search(self) -> None:
        self._filter_proxy.set_criteria(None)
        self._page_proxy.first_page()
        self._refresh_info()
        self.table.resizeColumnsToContents()

    def _set_page_size(self, value: int) -> None:
        self._page_proxy.set_page_size(value)
        self._refresh_info()

    def _navigate_page(self, action: str) -> None:
        getattr(self._page_proxy, action)()
        self._refresh_info()

    def _refresh_info(self) -> None:
        total_pages = self._page_proxy.total_pages
        current_page = 0 if total_pages == 0 else self._page_proxy.current_page
        self.pagination.set_page_size(self._page_proxy.page_size)
        self.pagination.set_info(
            current_page,
            total_pages,
            self._page_proxy.rowCount(),
            self._page_proxy.total_rows,
        )


class DeleteDialog(QDialog):
    def __init__(self, parent: QWidget | None = None) -> None:
        super().__init__(parent)
        self.setWindowTitle("Удаление записей")

        layout = QVBoxLayout(self)
        self.criteria = CriteriaWidget()
        layout.addWidget(self.criteria)

        buttons = QDialogButtonBox(QDialogButtonBox.StandardButton.Ok | QDialogButtonBox.StandardButton.Cancel)
        buttons.accepted.connect(self._validate_and_accept)
        buttons.rejected.connect(self.reject)
        layout.addWidget(buttons)

    def _validate_and_accept(self) -> None:
        if self.criteria.to_criteria().is_empty():
            QMessageBox.warning(self, "Ошибка ввода", "Введите хотя бы одно условие удаления.")
            return
        self.accept()

    def to_criteria(self) -> QueryCriteria:
        return self.criteria.to_criteria()


def _cmp_from_text(text: str):
    if text == ">":
        return "gt"
    if text == "<":
        return "lt"
    return None
