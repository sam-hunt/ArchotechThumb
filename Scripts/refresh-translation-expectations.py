#!/usr/bin/env python3
# Archotech Thumb's config shim over the shared sidecar-refresh engine
# (l10n/refresh/refresh_expectations.py — the rimworld-l10n submodule),
# which drives the L10nProbe dev mod (source at l10n/probe/; build/deploy it
# only from the canonical ~/dev/rimworld-l10n checkout). The engine holds all
# logic; this file holds only this repo's config and the rationale behind it.
# Usage is unchanged (game must be closed):
#   python3 Scripts/refresh-translation-expectations.py [--no-launch]
# If l10n/ is empty, run: git submodule update --init

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent.parent / "l10n" / "refresh"))
import refresh_expectations as engine  # noqa: E402  (import after sys.path edit)

engine.REPO_ROOT = Path(__file__).resolve().parent.parent

engine.PACKAGE_ID = "shunter.archotechthumb"

# RATIONALE: this mod has no DLC dependency and none of its defs (AT_Orbital-
# BeamStrike, the AT_ArchotechThumb hediff/thing/recipe family) carry a
# MayRequire, so no DLC belongs in this list. Patch_RewardPools.xml's xpath
# targets DLC-defined ThingSetMakerDefs when the relevant DLC happens to be
# installed, but that patch adds no label/description of its own — nothing
# this mod must translate depends on a DLC being active. This mod is not
# part of a probed family (unlike the UniqueMeleeWeapons trio, which rides
# three independent mods along in one boot for convenience); it is the only
# sidecar-bearing mod in its own boot. See the engine's header for the
# general membership rule, the lowercase-id warning, and the pinning
# rationale; order is load order, the probe last.
engine.CANONICAL_ACTIVE_MODS = [
    "brrainz.harmony",
    "ludeon.rimworld",
    "shunter.archotechthumb",
    "shunter.l10nprobe",
]

raise SystemExit(engine.main())
