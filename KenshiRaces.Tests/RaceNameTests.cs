namespace KenshiRaces.Tests;

/// <summary>
/// Subrace and character names are one lowercase word (docs/kenshi-races.md "Races"), so the console's <c>spawn</c> takes them without quotes,
/// and each body's files are named after it.
/// </summary>
public sealed class RaceNameTests
{
    [Fact]
    public void Every_subrace_and_character_name_is_one_lowercase_word()
    {
        Assert.All(Races.All, s => Assert.Matches("^[a-z]+$", s.Name));
        Assert.All(Races.All.SelectMany(s => s.Characters), c => Assert.Matches("^[a-z]+$", c.Name));
    }

    [Fact]
    public void Every_body_stem_is_the_prefixed_name_of_its_character()
    {
        foreach (var subrace in Races.All)
        {
            foreach (var character in subrace.Characters)
                Assert.Equal("kenshiraces_" + character.Name, subrace.Body(character.Female).Stem);
        }
    }
}
