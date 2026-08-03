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

    private Vector2 scrollPosition;
    private float contentHeight;

    public void ResetToDefaults()
    {
        orbitalBeamCooldownDays = DefaultCooldownDays;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref orbitalBeamCooldownDays, "orbitalBeamCooldownDays", DefaultCooldownDays);
    }

    public void DoWindowContents(Rect inRect)
    {
        const float buttonHeight = 30f;
        const float buttonGap = 10f;
        Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height - buttonHeight - buttonGap);
        Rect buttonRect = new Rect(inRect.x, inRect.yMax - buttonHeight, 200f, buttonHeight);

        // Self-measuring scroll view: innerRect height comes from the previous
        // frame's CurHeight, so a scrollbar only appears once content overflows.
        float innerWidth = viewRect.width - 16f;
        Rect innerRect = new Rect(0f, 0f, innerWidth, Mathf.Max(contentHeight, viewRect.height));
        Widgets.BeginScrollView(viewRect, ref scrollPosition, innerRect);

        var listing = new Listing_Standard();
        listing.Begin(new Rect(0f, 0f, innerWidth - 8f, 99999f));

        orbitalBeamCooldownDays = SliderRow(listing,
            "ArchotechThumb_OrbitalBeamCooldown", "ArchotechThumb_OrbitalBeamCooldownDesc",
            orbitalBeamCooldownDays, DefaultCooldownDays,
            MinCooldownDays, MaxCooldownDays);

        contentHeight = listing.CurHeight;
        listing.End();
        Widgets.EndScrollView();

        if (Widgets.ButtonText(buttonRect, "ArchotechThumb_ResetToDefaults".Translate()))
        {
            ResetToDefaults();
        }
    }

    // House-style slider row: label carries the current value plus a "(default)"
    // suffix while at the shipped default, description as hover tooltip. Returns
    // the slider value snapped to step, measured from min.
    private static int SliderRow(Listing_Standard listing, string labelKey, string descKey,
        int value, int defaultValue, int min, int max, int step = 1)
    {
        string label = labelKey.Translate(value);
        if (value == defaultValue)
        {
            label += "ArchotechThumb_DefaultSuffix".Translate();
        }
        listing.Label(label, tooltip: descKey.Translate(defaultValue));
        float raw = listing.Slider(value, min, max);
        return Mathf.RoundToInt((raw - min) / step) * step + min;
    }
}
