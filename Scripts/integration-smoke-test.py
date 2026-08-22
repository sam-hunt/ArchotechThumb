#!/usr/bin/env python3
# Pre-release integration smoke test: boots the real game once on this repo's
# pinned minimal mod list, then classifies every Player.log error/warning by
# origin and fails on anything attributed to Archotech Thumb. With no
# optional integrations, this is a clean-startup-log gate. Thin shim over
# the shared engine in l10n/smoke/startup_smoke.py (see its header for
# mechanics and the BetterTradersGuild v1.1.0 CWTL incident this exists to
# catch).
#
# Run this before every release, with the game closed:
#   python3 Scripts/integration-smoke-test.py              # boot + scan
#   python3 Scripts/integration-smoke-test.py --no-launch  # rescan last log
#   python3 Scripts/integration-smoke-test.py --strict     # any error fails

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent.parent / "l10n" / "smoke"))
import startup_smoke as engine  # noqa: E402

engine.REPO_ROOT = Path(__file__).resolve().parent.parent

engine.PACKAGE_ID = "shunter.archotechthumb"

# RATIONALE: this is this repo's l10n CANONICAL_ACTIVE_MODS list - no DLC and
# no optional mod dependencies, so there is nothing to add beyond Core,
# Harmony and the mod itself. No optional integration mods means no
# conditional patches/reflection to exercise, so this is simply a
# clean-startup-log gate. Probe last (auto-quit).
engine.SMOKE_ACTIVE_MODS = [
    "brrainz.harmony",
    "ludeon.rimworld",
    "shunter.archotechthumb",
    "shunter.l10nprobe",
]

engine.OWN_PATTERNS = ["ArchotechThumb", "AT_"]

# No optional integrations for this mod - any error is either ours or
# unrelated third-party noise.
engine.INTEGRATION_PATTERNS = {}

raise SystemExit(engine.main())
