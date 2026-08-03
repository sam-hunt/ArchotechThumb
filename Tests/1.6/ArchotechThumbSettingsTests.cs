using Xunit;

namespace ArchotechThumb.Tests;

// Unit coverage for ArchotechThumbSettings: the days-to-ticks conversion,
// the field-initializer default, and ResetToDefaults() restoring that
// default after mutation. DoWindowContents/SliderRow are UI (Verse/Unity
// at runtime) and are not covered here.
public class ArchotechThumbSettingsTests
{
    [Fact]
    public void NewInstance_FieldMatchesDefaultConstant()
    {
        var settings = new ArchotechThumbSettings();

        Assert.Equal(ArchotechThumbSettings.DefaultCooldownDays, settings.orbitalBeamCooldownDays);
    }

    [Fact]
    public void ResetToDefaults_RestoresFieldAfterMutation()
    {
        var settings = new ArchotechThumbSettings
        {
            orbitalBeamCooldownDays = ArchotechThumbSettings.MaxCooldownDays,
        };

        settings.ResetToDefaults();

        Assert.Equal(ArchotechThumbSettings.DefaultCooldownDays, settings.orbitalBeamCooldownDays);
    }

    [Fact]
    public void DefaultCooldownDays_FallsWithinItsSliderRange()
    {
        // Guards against a future default edit landing outside the slider
        // range it's meant to be clamped by.
        Assert.InRange(ArchotechThumbSettings.DefaultCooldownDays,
            ArchotechThumbSettings.MinCooldownDays, ArchotechThumbSettings.MaxCooldownDays);
    }

    // ---- OrbitalBeamCooldownTicks: days -> ticks conversion --------------------

    [Theory]
    [InlineData(ArchotechThumbSettings.MinCooldownDays, 0)]
    [InlineData(ArchotechThumbSettings.DefaultCooldownDays, ArchotechThumbSettings.DefaultCooldownDays * ArchotechThumbSettings.TicksPerDay)]
    [InlineData(ArchotechThumbSettings.MaxCooldownDays, ArchotechThumbSettings.MaxCooldownDays * ArchotechThumbSettings.TicksPerDay)]
    public void OrbitalBeamCooldownTicks_ConvertsDaysToTicks(int days, int expectedTicks)
    {
        var settings = new ArchotechThumbSettings { orbitalBeamCooldownDays = days };

        Assert.Equal(expectedTicks, settings.OrbitalBeamCooldownTicks);
    }
}
