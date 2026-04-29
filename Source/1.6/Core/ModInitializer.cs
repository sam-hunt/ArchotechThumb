using HarmonyLib;
using Verse;

namespace ArchotechThumb;

/// <summary>
/// Mod entry point. Applies all Harmony patches at startup.
/// </summary>
public class ArchotechThumbMod : Mod
{
    public ArchotechThumbMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("shunter.archotechthumb");
        harmony.PatchAll();
        Log.Message($"[Archotech Thumb] Initialized with {harmony.GetPatchedMethods().EnumerableCount()} patches.");
    }
}
