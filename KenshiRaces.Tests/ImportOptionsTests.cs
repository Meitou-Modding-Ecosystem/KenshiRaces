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

    [Theory]
    [InlineData("bust=crab")]
    [InlineData("neck=short")]
    [InlineData("tall")]
    public void An_unknown_option_is_refused(string option)
    {
        Assert.Null(ImportOptions.Parse([option]));
    }
}
