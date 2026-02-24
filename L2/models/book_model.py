from __future__ import annotations

from dataclasses import dataclass
from typing import Literal

ComparisonType = Literal["gt", "lt"]

ID_COL = 0
TITLE_COL = 1
AUTHOR_COL = 2
PUBLISHER_COL = 3
VOLUMES_COL = 4
CIRCULATION_COL = 5
TOTAL_TOMES_COL = 6

TABLE_HEADERS = { 
    TITLE_COL: "Название книги",
    AUTHOR_COL: "ФИО автора",
    PUBLISHER_COL: "Издательство",
    VOLUMES_COL: "Число томов",
    CIRCULATION_COL: "Тираж",
    TOTAL_TOMES_COL: "Итого томов",
}


@dataclass(slots=True)
class Book:
    title: str
    author: str
    publisher: str
    volumes: int
    circulation: int


@dataclass(slots=True)
class QueryCriteria:
    author: str | None = None
    publisher: str | None = None
    title: str | None = None
    min_volumes: int | None = None
    max_volumes: int | None = None
    circulation_cmp: ComparisonType | None = None
    circulation_value: int | None = None
    total_cmp: ComparisonType | None = None
    total_value: int | None = None

    def normalized(self) -> "QueryCriteria":
        return QueryCriteria(
            author=_normalize_text(self.author),
            publisher=_normalize_text(self.publisher),
            title=_normalize_text(self.title),
            min_volumes=self.min_volumes,
            max_volumes=self.max_volumes,
            circulation_cmp=self.circulation_cmp,
            circulation_value=self.circulation_value,
            total_cmp=self.total_cmp,
            total_value=self.total_value,
        )

    def is_empty(self) -> bool:
        normalized = self.normalized()
        return all(
            value is None
            for value in (
                normalized.author,
                normalized.publisher,
                normalized.title,
                normalized.min_volumes,
                normalized.max_volumes,
                normalized.circulation_cmp,
                normalized.circulation_value,
                normalized.total_cmp,
                normalized.total_value,
            )
        )


def build_where_clause(criteria: QueryCriteria) -> tuple[str, dict[str, object]]:
    normalized = criteria.normalized()
    parts: list[str] = []
    params: dict[str, object] = {}

    if normalized.author is not None:
        parts.append("LOWER(author) = LOWER(:author)")
        params[":author"] = normalized.author

    if normalized.publisher is not None:
        parts.append("LOWER(publisher) = LOWER(:publisher)")
        params[":publisher"] = normalized.publisher

    if normalized.title is not None:
        parts.append("LOWER(title) = LOWER(:title)")
        params[":title"] = normalized.title

    if normalized.min_volumes is not None:
        parts.append("volumes >= :min_volumes")
        params[":min_volumes"] = normalized.min_volumes

    if normalized.max_volumes is not None:
        parts.append("volumes <= :max_volumes")
        params[":max_volumes"] = normalized.max_volumes

    if normalized.circulation_cmp is not None and normalized.circulation_value is not None:
        op = ">" if normalized.circulation_cmp == "gt" else "<"
        parts.append(f"circulation {op} :circulation_value")
        params[":circulation_value"] = normalized.circulation_value

    if normalized.total_cmp is not None and normalized.total_value is not None:
        op = ">" if normalized.total_cmp == "gt" else "<"
        parts.append(f"total_tomes {op} :total_value")
        params[":total_value"] = normalized.total_value

    return " AND ".join(parts), params


def _normalize_text(value: str | None) -> str | None:
    if value is None:
        return None
    stripped = value.strip()
    return stripped or None

