# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Archotech Thumb** is a RimWorld 1.6 mod adding an archotech thumb body part — a tiny prosthetic concealing an archotech subpersona with an uplink to a derelict orbital weapon platform. Installing it grants the pawn the AT_OrbitalBeamStrike ability (a callable power beam with a configurable cooldown via mod settings), and the part is patched into vanilla/DLC archotech reward pools.

**Key Technologies:** C# (.NET Framework 4.7.2), Harmony library, RimWorld modding API, XML definitions

## Build Commands

```bash
# Build the mod (outputs to 1.6/Assemblies/ and deploys to RimWorld Mods folder)
dotnet build ArchotechThumb.sln -c Release

# Build only the main project
dotnet build Source/1.6/ArchotechThumb.csproj

# Stage the mod files only (atomic wipe+recopy; use when Defs/Patches are renamed or deleted)
dotnet build Source/1.6/ArchotechThumb.csproj -t:StageMod

# Run the test suite (native; vstest hosts the net472 suite via mono)
dotnet test Tests/1.6/ArchotechThumb.Tests.csproj

# Validate localization (also a CI release gate)
python3 Scripts/check-translations.py --strict
```

The build system auto-detects the RimWorld installation path on Windows/Linux/Mac (including WSL targeting a Windows install). For CI builds without RimWorld installed, it falls back to the `Krafs.Rimworld.Ref` NuGet package.

### Deployment

The repo lives in `~/dev/ArchotechThumb`, separate from the RimWorld Mods folder. The csproj's `StageMod` target is the **single source of truth** for what files ship: its ItemGroup feeds both the post-build local deploy (`DeployToModFolder` → `StageMod`, an atomic wipe+recopy of `$RIMWORLD_PATH/Mods/ArchotechThumb/`, so renamed/deleted files never linger) and the CI release, which invokes the same target with `-p:StageDir=...` so the release zip cannot drift from local deploys. Add/remove shipped files only in that ItemGroup.

A machine-local Claude Code Stop hook (`.claude/hooks/sync-mod.sh`, untracked) rebuilds and redeploys after any turn that touched mod files, so the deployed copy stays fresh without manual builds.

**WSL Setup:** Requires `RIMWORLD_PATH` env var in `~/.bashrc` pointing to the Windows RimWorld install (e.g., `/mnt/c/Program Files (x86)/Steam/steamapps/common/RimWorld`).

## Architecture

### Directory Structure

```
About/              # Mod metadata (About.xml, ModIcon.png, Preview.png)
1.6/
├── Assemblies/     # Compiled DLL (build output)
├── Defs/           # XML defs (AT_OrbitalBeamStrike ability; AT_ArchotechThumb hediff/thing/recipe)
├── Patches/        # XPath patches (Patch_RewardPools.xml adds the thumb to reward pools)
└── Languages/English/Keyed/ArchotechThumb.xml  # All player-facing strings (ArchotechThumb_ prefix)
Textures/           # Custom textures
Source/1.6/
├── Core/           # ArchotechThumbMod entry point + ArchotechThumbSettings
├── Patches/        # Harmony patches
└── Properties/     # AssemblyInfo
Tests/1.6/          # Headless xUnit (net472) suite for pure logic
Scripts/
├── check-translations.py           # Deterministic localization validator (CI release gate)
├── refresh-translation-expectations.py  # Regenerates the sidecar via ../L10nProbe game boot
└── expected-injections.json        # Checked-in DefInjected expectations sidecar
.github/workflows/  # CI (release.yml triggers on v*.*.* tags)
```

### .claude layout

`.gitignore` tracks only `.claude/skills/` (shared: `release`, `translate`, `rimworld-logs`); `.claude/hooks/` and `.claude/settings.local.json` (Stop-hook wiring, permissions) stay machine-local.

### Def Naming Convention

All defs use the `AT_` prefix (Archotech Thumb).

**Namespace Convention:** Use `*Patches` suffix for patch namespaces to avoid RimWorld type conflicts (e.g., `ArchotechThumb.Patches`).

## Testing

`Tests/1.6/` holds an xUnit (net472) suite for the pure logic: `ArchotechThumbSettings` defaults, `ResetToDefaults`, and the days-to-ticks cooldown conversion. Tests are headless — anything needing `DefDatabase`/`Current.Game` (e.g. `ApplyOrbitalBeamCooldown`, the settings UI) is out of scope. Run natively with `dotnet test Tests/1.6/ArchotechThumb.Tests.csproj` — vstest hosts the net472 suite via mono. If a run fails with `BadImageFormatException`/`TypeLoadException`, a DLL is missing from the test csproj copy target (see the Assembly-CSharp-firstpass comment there): mono resolves field types eagerly where the Windows CLR is lazy. CI builds the Tests project but does not run it.

## Localization

English (`1.6/Languages/English/Keyed/ArchotechThumb.xml`, `ArchotechThumb_` prefix) is the source of truth; the mod also ships Defs, so non-English languages need `DefInjected` files too. The pipeline is shared with the sibling mod repos (`../TradersStockXenogerms`, `../UniqueMeleeWeapons`, etc.):

- `python3 Scripts/check-translations.py [--strict]` — deterministic validator (key/placeholder parity, `<!-- EN: ... -->` staleness comments, DefInjected paths, file hygiene). Run by the `translate` and `release` skills and as a CI release gate.
- `Scripts/expected-injections.json` — checked-in sidecar of every DefInjected key the live game expects for this mod; regenerated by `python3 Scripts/refresh-translation-expectations.py` (boots RimWorld via the `../L10nProbe` dev mod; refuses while the game is open). Regenerate after any Defs change — the checker fails on a stale sidecar.
- The `translate` skill holds the family-shared per-language grammar/glossary knowledge; CONTRIBUTING.md carries the public roster (English only so far) and must move in the same commit as any language change.
- **Workshop title coupling:** each language's `ArchotechThumb_SettingsCategory` Keyed value is the localized Steam Workshop title and must equal the title line (line 1) of `.steamworkshop/Description/<Language>.txt` — always change the two together (English keeps `Archotech Thumb` in both).

## Linting

Roslynator.Analyzers runs on every build (warnings only, never fails the build; `PrivateAssets=all` so nothing ships). Severities are pinned in `.editorconfig`, which also enforces PascalCase public members (fields excluded — RimWorld's `Scribe_Values.Look` relies on camelCase field names) and the no-XML-doc-comments convention (plain `//` only). Formatting-only sweeps are registered in `.git-blame-ignore-revs` (each clone must run `git config blame.ignoreRevsFile .git-blame-ignore-revs` once).

## Debugging

Use the `rimworld-logs` skill — it covers Player.log locations (Windows/WSL/Linux), the `[Archotech Thumb]` log prefix, and API disassembly (`monodis`/`ilspycmd` against the live install's `Assembly-CSharp.dll`, preferred over the `Krafs.Rimworld.Ref` CI fallback).

## Releases

Tag a `v*.*.*` push and the GitHub Actions release workflow builds, packages, and publishes a zip. The `/release` slash command (in `.claude/skills/release/SKILL.md`) walks through version bump → changelog → build → tag → push.
