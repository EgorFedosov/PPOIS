from __future__ import annotations

import json
from pathlib import Path


def load_records(path: Path) -> list[dict[str, int | str]]:
    if not path.exists():
        return [{"name": "AAA", "score": 0}]
    try:
        data = json.loads(path.read_text(encoding="utf-8"))
        if not isinstance(data, list):
            return [{"name": "AAA", "score": 0}]
        records: list[dict[str, int | str]] = []
        for item in data:
            if not isinstance(item, dict):
                continue
            name = str(item.get("name", "AAA"))[:16]
            score = int(item.get("score", 0))
            records.append({"name": name or "AAA", "score": score})
        return records or [{"name": "AAA", "score": 0}]
    except (json.JSONDecodeError, OSError, ValueError, TypeError):
        return [{"name": "AAA", "score": 0}]


def save_records(path: Path, records: list[dict[str, int | str]]) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(records, ensure_ascii=False, indent=2), encoding="utf-8")


def add_record(path: Path, name: str, score: int, limit: int = 10) -> list[dict[str, int | str]]:
    records = load_records(path)
    clean_name = "".join(ch for ch in name[:16] if ch.isalnum() or ch in " _-").strip() or "NONAME"
    records.append({"name": clean_name, "score": int(score)})
    records.sort(key=lambda item: int(item["score"]), reverse=True)
    records = records[:limit]
    save_records(path, records)
    return records
