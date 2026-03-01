"""FastAPI application entry point."""

from pathlib import Path

from fastapi import FastAPI, HTTPException
from fastapi.responses import HTMLResponse

try:
    from L4.app.service import CookingService, UnknownActionError
    from L4.shared.exceptions import CookingError, InvalidStateTransitionError
except ModuleNotFoundError:
    from app.service import CookingService, UnknownActionError
    from shared.exceptions import CookingError, InvalidStateTransitionError

BASE_DIR = Path(__file__).resolve().parents[1]
WEB_INDEX_PATH = BASE_DIR / "web" / "index.html"
STATE_PATH = BASE_DIR / "data" / "state.json"

service = CookingService(STATE_PATH)
app = FastAPI(title="L4 Cooking API", version="1.0.0")


@app.get("/", response_class=HTMLResponse)
def index() -> str:
    return WEB_INDEX_PATH.read_text(encoding="utf-8")


@app.get("/api/state")
def get_state() -> dict[str, object]:
    return service.get_state()


@app.post("/api/actions/{action}")
def execute_action(action: str) -> dict[str, object]:
    try:
        message, snapshot = service.execute(action)
        return {"message": message, "state": snapshot}
    except UnknownActionError as error:
        raise HTTPException(status_code=404, detail=str(error)) from error
    except InvalidStateTransitionError as error:
        raise HTTPException(status_code=409, detail=str(error)) from error
    except CookingError as error:
        raise HTTPException(status_code=400, detail=str(error)) from error


@app.post("/api/reset")
def reset_state() -> dict[str, object]:
    snapshot = service.reset()
    return {"message": "Состояние сброшено. Можно начать заново.", "state": snapshot}

