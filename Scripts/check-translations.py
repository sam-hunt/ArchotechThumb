#!/usr/bin/env python3
# Archotech Thumb's config shim over the shared translation checker
# (l10n/checker/check_translations.py — the rimworld-l10n submodule). The
# engine holds all logic; this file holds only this repo's config and the
# rationale behind it. Usage is unchanged:
#   python3 Scripts/check-translations.py [--strict] [--root PATH]
# If l10n/ is empty, run: git submodule update --init

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent.parent / "l10n" / "checker"))
import check_translations as engine  # noqa: E402  (import after sys.path edit)

engine.REPO_ROOT = Path(__file__).resolve().parent.parent

# No [TranslationCanChangeCount]-style matching-token fields in this repo.
engine.PARITY_EXEMPT_FIELDS = set()

# RATIONALE: this mod's hard dependency is Harmony only (no DLC); every one
# of its own defs (AT_OrbitalBeamStrike, the AT_ArchotechThumb hediff/thing/
# recipe family) loads with Core alone, and none of them carry a MayRequire
# gate. The reward-pool patch (Patch_RewardPools.xml) xpath-matches
# DLC-defined ThingSetMakerDefs when the relevant DLC is installed, but that
# patch adds no label/description of its own — it only appends our
# already-translated ThingDef's defName to a vanilla list — so no DLC is
# required for the sidecar to see every injection point this mod is
# responsible for.
engine.REQUIRED_DLCS = set()

# Empty here today; ArchotechAndroidHardware's shim carries the first real
# entry (VREA's AndroidGeneDef -> GeneDef).
engine.DEF_TYPE_ALIASES = {}

# This mod ships a real Keyed surface, so a missing Languages/ tree is a hard
# config error, not a legal state.
engine.ALLOW_NO_KEYED_SURFACE = False

raise SystemExit(engine.main())
