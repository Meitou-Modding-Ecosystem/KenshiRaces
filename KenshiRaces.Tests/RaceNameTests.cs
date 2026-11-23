namespace KenshiRaces.Tests;

/// <summary>
/// Race names are one lowercase word (docs/kenshi-races.md "Races"), so the console's <c>spawn</c> takes them without quotes.
/// </summary>
public sealed class RaceNameTests
{
    [Fact]
    public void Every_race_name_is_one_lowercase_word() =>
        Assert.All(Races.All, r => Assert.Matches("^[a-z]+$", r.Name));

    [Fact]
    public void Every_stem_is_the_prefixed_name() =>
        Assert.All(Races.All, r => Assert.Equal("kenshiraces_" + r.Name, r.Stem));
}
