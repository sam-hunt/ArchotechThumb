using UnityEngine;
using Verse;

namespace ArchotechThumb;

public class ArchotechThumbSettings : ModSettings
{
    public const int MinCooldownDays = 0;
    public const int MaxCooldownDays = 60;
    public const int DefaultCooldownDays = 30;
    public const int TicksPerDay = 60000;

    public int orbitalBeamCooldownDays = DefaultCooldownDays;

    public int OrbitalBeamCooldownTicks => orbitalBeamCooldownDays * TicksPerDay;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref orbitalBeamCooldownDays, "orbitalBeamCooldownDays", DefaultCooldownDays);
    }

    public void DoWindowContents(Rect inRect)
    {
        var listing = new Listing_Standard();
        listing.Begin(inRect);

        listing.Label(
            "ArchotechThumb_OrbitalBeamCooldown".Translate(orbitalBeamCooldownDays),
            tooltip: "ArchotechThumb_OrbitalBeamCooldown_Tip".Translate());
        orbitalBeamCooldownDays = (int)listing.Slider(
            orbitalBeamCooldownDays,
            MinCooldownDays,
            MaxCooldownDays);

        listing.End();
    }
}
