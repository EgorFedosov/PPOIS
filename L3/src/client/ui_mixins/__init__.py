from __future__ import annotations

import importlib.util
import sys
from pathlib import Path
from types import ModuleType


def _load_cached_module(short_name: str) -> ModuleType:
    module_name = f"{__name__}.{short_name}"
    if module_name in sys.modules:
        return sys.modules[module_name]

    cache_dir = Path(__file__).with_name("__pycache__")
    version_tag = f"{sys.version_info.major}{sys.version_info.minor}"
    pyc_path = cache_dir / f"{short_name}.cpython-{version_tag}.pyc"

    if not pyc_path.exists():
        candidates = sorted(cache_dir.glob(f"{short_name}.cpython-*.pyc"))
        if not candidates:
            raise ImportError(f"Cannot load '{module_name}': missing cached bytecode")
        pyc_path = candidates[-1]

    spec = importlib.util.spec_from_file_location(module_name, pyc_path)
    if spec is None or spec.loader is None:
        raise ImportError(f"Cannot load '{module_name}' from '{pyc_path}'")

    module = importlib.util.module_from_spec(spec)
    sys.modules[module_name] = module
    spec.loader.exec_module(module)
    return module


UICommonMixin = _load_cached_module("common").UICommonMixin
UIGameplayMixin = _load_cached_module("gameplay").UIGameplayMixin
UIRenderMixin = _load_cached_module("render").UIRenderMixin
UIScreensMixin = _load_cached_module("screens").UIScreensMixin

__all__ = [
    "UICommonMixin",
    "UIGameplayMixin",
    "UIRenderMixin",
    "UIScreensMixin",
]
