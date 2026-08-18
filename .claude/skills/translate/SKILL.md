---
name: translate
description: Generate, update, or audit mod localization (Keyed and DefInjected) for a target language, grounded in vanilla RimWorld terminology — particularly Core's archotech body-part vocabulary. Use when asked to add a language, update translations, or check translation freshness.
argument-hint: "[language, e.g. German | update | check]"
---

# Translate

Produce or refresh localization files for Archotech Thumb. English is
the source of truth; every other language derives from it.

**The family-wide process lives in the `l10n/` submodule — load these first,
and only these** (progressive disclosure; if `l10n/` is empty, run
`git submodule update --init`):

- `l10n/process.md` — non-negotiables, file/format conventions, terminology
  grounding method, and the generation / update / audit workflows. This is
  the workflow authority; follow it step by step.
- `l10n/languages/<Language>.md` — the target language's engine mechanics,
  style rules, and vanilla-grounded common vocabulary. Read ONLY the target
  language's file.
- `glossary/<Language>.md` (beside this file) — this mod's own coined-term
  file for the target language. Read it in the same pass. Every one is
  currently a placeholder — no language pass has run for this mod yet.
- `l10n/lessons.md` — cross-language lessons; read when generating a new
  language, skim otherwise.
- `l10n/workshop.md` — Steam Workshop description/title conventions;
  `.steamworkshop/README.md` names this mod's anchor term and title-coupling
  key (`ArchotechThumb_SettingsCategory`).

**Where learnings land:** mod-independent findings (engine mechanics, a
language's grammar rule, corpus style facts) go in the `l10n/` submodule —
edit the canonical checkout at `~/dev/rimworld-l10n`, commit there, then bump
the pin here. Mod-specific findings (coined terms, phrasing decisions) go in
`glossary/<Language>.md`.

## This mod's translation surface

- English Keyed source: `1.6/Languages/English/Keyed/ArchotechThumb.xml` — a
  single file covering the mod settings window (the orbital-beam cooldown
  slider, reset button) and any other player-facing prose this mod owns
  outside its Defs. Every key is `ArchotechThumb_`-prefixed. There is no
  second Keyed file.
- **This mod ships its own Defs, so the DefInjected surface is real and
  non-empty from day one.** `1.6/Defs/*.xml` defines four real defs with
  real translatable fields:
  - `AbilityDef AT_OrbitalBeamStrike` — `label`, `description`.
  - `HediffDef AT_ArchotechThumb` — `label`, `labelNoun`, `description`, plus
    a nested `HediffCompProperties_VerbGiver` melee tool (`label: "fist"`)
    that is exactly the kind of collection-nested field the sidecar exists
    to surface — do not assume it isn't translatable just because it sits
    three levels deep in `comps`.
  - `ThingDef AT_ArchotechThumb` — `label`, `description` (shares its
    defName with the HediffDef above; they resolve independently because
    DefInjected is folder-scoped by def *type*, so there is no collision).
  - `RecipeDef AT_InstallArchotechThumb` — `label`, `description`,
    `jobString`.
  All four are vanilla def types (`AbilityDef`, `HediffDef`, `ThingDef`,
  `RecipeDef`), so every DefInjected folder is a bare vanilla name
  (`DefInjected/AbilityDef/`, etc.) — none subclass a custom C# Def class.
  Audit the surface against `Scripts/expected-injections.json`, never
  against this bullet list: the sidecar also catches anything inherited
  from the `AddedBodyPartBase`/`BodyPartArchotechBase`/
  `SurgeryInstallBodyPartArtificialBase` vanilla parent defs these four
  ParentName-chain onto, and a collection-nested field's key is an index
  path that is only known by asking the sidecar (e.g. the verb-giver tool's
  `label`, something like `AT_ArchotechThumb.comps.1.tools.0.label` — never
  hand-derive the index from the XML's list positions).
- This mod has no gated compat load root today (no `MayRequire`-gated defs,
  no `1.6/Mods/<Name>/` folders), so every translation lives under the plain
  `1.6/Languages/<Language>/` tree.
- Placeholders: both mod-settings keys
  (`ArchotechThumb_OrbitalBeamCooldown` and
  `ArchotechThumb_OrbitalBeamCooldownDesc`) take a single `{0}` — the
  configured cooldown in days — so plan the phrasing around a bare integer,
  not a pre-pluralized noun.
- `ArchotechThumb_SettingsCategory` is not an ordinary settings-window
  label: it is that language's localized Steam Workshop title, and must
  stay in sync with the title line (line 1) of
  `.steamworkshop/Description/<Language>.txt` (see `l10n/workshop.md` and
  the CLAUDE.md localization note). English keeps `Archotech Thumb` in both.

## This mod's grounding domain

Domain: **Core only** (no DLC dependency). Ground against the Core tar —
this mod has no DLC dependency, and the vanilla defs its own defs are
directly modeled on (`ArchotechEye`/`ArchotechArm`/`ArchotechLeg` and their
install recipes, in
`Data/Core/Defs/HediffDefs/BodyParts/Hediffs_BodyParts_Archotech.xml`) are
Core content, not DLC-gated. Terms that MUST be grounded before use:
archotech (the tech-level/adjective, and the noun for the entities
themselves), the "an artificial `<part>` built by an archotech"
ThingDef-description formula, "install archotech `<part>`"
recipe-label/jobString phrasing, "subpersona AI", "betterThanNatural"/
"harder to damage than plasteel and repairs itself over time" body-part
flavor text, and ability-gizmo cooldown/recharge vocabulary (this mod's
orbital beam strike is mod-coined — there is no vanilla "orbital beam
strike" to match — but the *cooldown/recharging* UI language around it must
match vanilla ability gizmos). **No language pass has run for this mod
yet** — `glossary/<Language>.md` files are placeholders until the first
pass grounds these terms and records them.

## Workflows

Follow `l10n/process.md`'s Initial generation / Update pass / Audit-only
workflows verbatim. This mod's specifics on top:

- The checker: `python3 Scripts/check-translations.py` (`--strict` for new
  languages). Sidecar regen: `python3
  Scripts/refresh-translation-expectations.py` (game must be closed; drives
  the deployed L10nProbe).
- Enumerate the DefInjected surface from the sidecar's `required` entries,
  not from `1.6/Defs/`'s `<label>`/`<description>` fields — this mod has no
  English DefInjected tree to enumerate from either, so the sidecar is the
  only authority. Take the English source text for each entry from the
  sidecar's own `"english"` field rather than re-reading it off the def
  XML: it is also what the checker compares `<!-- EN: -->` comments
  against, so sourcing EN comments from it programmatically makes drift
  impossible, and it is the only place a nested-`comps` field's text is
  unambiguous.
- The public roster (and credits) is CONTRIBUTING.md's localization table —
  update it in the same commit as any language addition or native review.
  Today it lists English only.
