using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// RaceImport's optional arguments (docs/race-import.md "Run it"): any order, defaults for the missing ones, nothing unknown accepted.
/// </summary>
public sealed class ImportOptionsTests
{
    [Fact]
    public void No_options_give_a_slimmed_male_fit_with_its_own_bust_and_neck()
    {
        Assert.Equal(new ImportOptions(HumanRig.Male, 1, null, 1), ImportOptions.Parse([]));
    }

    [Fact]
    public void Options_are_read_in_any_order()
    {
        var options = ImportOptions.Parse(["neck=0.7", "0.5", "bust=female", "male"]);

        Assert.Equal(new ImportOptions(HumanRig.Male, 0.5f, HumanRig.Female, 0.7f), options);
    }

    [Fact]
    public void Wings_and_headless_are_flags()
    {
        var options = ImportOptions.Parse(["female", "headless", "wings"]);

        Assert.Equal(new ImportOptions(HumanRig.Female, Wings: true, Headless: true), options);
    }

    [Fact]
    public void Ears_keep_the_head_human()
    {
        var options = ImportOptions.Parse(["ears"]);

        Assert.Equal(new ImportOptions(HumanRig.Male, Headless: true, Ears: true), options);
    }

    [Fact]
    public void Nohair_keeps_the_head_human_and_skintone_is_a_flag()
    {
        var options = ImportOptions.Parse(["female", "nohair", "skintone"]);

        Assert.Equal(new ImportOptions(HumanRig.Female, Headless: true, NoHair: true, SkinTone: true), options);
    }

    [Fact]
    public void Hairback_is_nohair_that_clears_the_back()
    {
        var options = ImportOptions.Parse(["female", "hairback"]);

        Assert.Equal(new ImportOptions(HumanRig.Female, Headless: true, NoHair: true, HairBack: true), options);
    }

    [Fact]
    public void Relief_takes_a_share_and_implies_skintone()
    {
        Assert.Equal(new ImportOptions(HumanRig.Male, SkinTone: true, Relief: 0.4f), ImportOptions.Parse(["relief=0.4"]));
        Assert.Equal(0, ImportOptions.Parse(["relief=-2"])!.Relief);
        Assert.Null(ImportOptions.Parse(["relief=soft"]));
    }

    [Fact]
    public void Armfill_is_a_flag()
    {
        Assert.Equal(new ImportOptions(HumanRig.Female, ArmFill: true), ImportOptions.Parse(["female", "armfill"]));
    }

    [Fact]
    public void Parts_are_attached_in_order()
    {
        var options = ImportOptions.Parse(["attach=ears.glb", "wings", "attach=parts/wings.glb"]);

        Assert.NotNull(options);
        Assert.Equal(["ears.glb", "parts/wings.glb"], options.Attach);
        Assert.Equal(new ImportOptions(HumanRig.Male, Wings: true), options with { Attach = Array.Empty<string>() });
    }

    [Fact]
    public void Palms_is_a_flag()
    {
        var options = ImportOptions.Parse(["palms", "female"]);

        Assert.Equal(new ImportOptions(HumanRig.Female, Palms: true), options);
    }

    [Theory]
    [InlineData("bust=crab")]
    [InlineData("attach=")]
    [InlineData("neck=short")]
    [InlineData("tall")]
    public void An_unknown_option_is_refused(string option)
    {
        Assert.Null(ImportOptions.Parse([option]));
    }
}
