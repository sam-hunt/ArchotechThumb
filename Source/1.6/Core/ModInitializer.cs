using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchotechThumb;

// Mod entry point. Registers settings, applies Harmony patches, and exposes
// the singleton mod instance for cross-component access.
public class ArchotechThumbMod : Mod
{
    public static ArchotechThumbMod Instance { get; private set; }

    public ArchotechThumbSettings Settings { get; }

    public ArchotechThumbMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<ArchotechThumbSettings>();

        var harmony = new Harmony("shunter.archotechthumb");
        harmony.PatchAll();
        Log.Message($"[Archotech Thumb] Initialized with {harmony.GetPatchedMethods().EnumerableCount()} patches.");
    }

    public override string SettingsCategory() => "Archotech Thumb";

    public override void DoSettingsWindowContents(Rect inRect) => Settings.DoWindowContents(inRect);

    public override void WriteSettings()
    {
        base.WriteSettings();
        ApplyOrbitalBeamCooldown();
    }

    // Push the configured cooldown onto the orbital beam ability def. Called at
    // startup once defs are loaded and again whenever the user changes settings.
    public void ApplyOrbitalBeamCooldown()
    {
        var def = DefDatabase<AbilityDef>.GetNamedSilentFail("AT_OrbitalBeamStrike");
        if (def == null) return;

        int ticks = Settings.OrbitalBeamCooldownTicks;
        def.cooldownTicksRange = new IntRange(ticks, ticks);
    }
}

[StaticConstructorOnStartup]
internal static class ArchotechThumbStartup
{
    static ArchotechThumbStartup()
    {
        ArchotechThumbMod.Instance?.ApplyOrbitalBeamCooldown();
    }
}
