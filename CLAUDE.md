# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Archotech Thumb** is a RimWorld 1.6 mod. Scope and content TBD — update this section once the design is finalised.

**Key Technologies:** C# (.NET Framework 4.7.2), Harmony library, RimWorld modding API, XML definitions

## Build Commands

```bash
# Build the mod (outputs to 1.6/Assemblies/ and deploys to RimWorld Mods folder)
dotnet build ArchotechThumb.sln -c Release

# Build only the main project
dotnet build Source/1.6/ArchotechThumb.csproj

# Stage the mod files only (atomic wipe+recopy; use when Defs/Patches are renamed or deleted)
dotnet build Source/1.6/ArchotechThumb.csproj -t:StageMod
```

The build system auto-detects the RimWorld installation path on Windows/Linux/Mac (including WSL targeting a Windows install). For CI builds without RimWorld installed, it falls back to the `Krafs.Rimworld.Ref` NuGet package.

### Deployment

The repo lives in `~/dev/ArchotechThumb`, separate from the RimWorld Mods folder. The csproj's `StageMod` target is the **single source of truth** for what files ship: its ItemGroup feeds both the post-build local deploy (`DeployToModFolder` → `StageMod`, an atomic wipe+recopy of `$RIMWORLD_PATH/Mods/ArchotechThumb/`, so renamed/deleted files never linger) and the CI release, which invokes the same target with `-p:StageDir=...` so the release zip cannot drift from local deploys. Add/remove shipped files only in that ItemGroup.

**WSL Setup:** Requires `RIMWORLD_PATH` env var in `~/.bashrc` pointing to the Windows RimWorld install (e.g., `/mnt/c/Program Files (x86)/Steam/steamapps/common/RimWorld`).

## Architecture

### Directory Structure

```
About/              # Mod metadata (About.xml, ModIcon.png, Preview.png)
1.6/
├── Assemblies/     # Compiled DLL (build output)
├── Defs/           # XML def files
└── Patches/        # XPath patches (XML overrides for vanilla / other mods)
Textures/           # Custom textures
Source/1.6/
├── Core/           # Mod subclass (Harmony bootstrap, settings if any)
├── Patches/        # Harmony patches
└── Properties/     # AssemblyInfo
Scripts/            # Build / deploy helpers
.github/workflows/  # CI (release.yml triggers on v*.*.* tags)
```

### Def Naming Convention

All defs use the `AT_` prefix (Archotech Thumb).

**Namespace Convention:** Use `*Patches` suffix for patch namespaces to avoid RimWorld type conflicts (e.g., `ArchotechThumb.Patches`).

## Debugging

1. **Enable RimWorld Dev Mode:** Settings > Dev Mode > Logging
2. **Log locations:**
   - **Windows:** `%USERPROFILE%\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Player.log`
   - **WSL:** `/mnt/c/Users/*/AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Player.log`
3. **Logging:** Use `Log.Message("[Archotech Thumb] ...")` for mod-specific logs
4. **Inspect RimWorld API:** `ilspycmd "/mnt/c/.../RimWorldWin64_Data/Managed/Assembly-CSharp.dll" -t "Namespace.ClassName"`

## Releases

Tag a `v*.*.*` push and the GitHub Actions release workflow builds, packages, and publishes a zip. The `/release` slash command (in `.claude/skills/release/SKILL.md`) walks through version bump → changelog → build → tag → push.
