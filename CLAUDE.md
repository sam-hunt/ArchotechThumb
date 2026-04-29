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

# Clean build + deploy (removes stale files, rebuilds, redeploys)
./Scripts/clean-build.sh

# Clean deployed mod folder (use when Defs/Patches are renamed or deleted)
dotnet build Source/1.6/ArchotechThumb.csproj -t:CleanModFolder
```

The build system auto-detects the RimWorld installation path on Windows/Linux/Mac (including WSL targeting a Windows install). For CI builds without RimWorld installed, it falls back to the `Krafs.Rimworld.Ref` NuGet package.

### Deployment

The repo lives in `~/dev/ArchotechThumb`, separate from the RimWorld Mods folder. A post-build MSBuild target (`DeployToModFolder`) automatically copies runtime files to `$RIMWORLD_PATH/Mods/ArchotechThumb/`. The `Scripts/clean-build.sh` script performs a full clean build + deploy cycle and is also run automatically via a Claude Code Stop hook after each conversation turn.

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
