using Meitou.Data;
using Meitou.Data.Fcs;

namespace KenshiRaces.Tests;

/// <summary>
/// A race's records are the game's Greenlander and Drifter with the body's files, no kit and the recruit talk, built from a synthetic data
/// file standing in for the game.
/// </summary>
public sealed class RaceRecordsTests
{
    [Fact]
    public void A_race_is_Greenlander_with_the_body_files_for_both_genders()
    {
        var race = new FcsRecord { StringId = Races.LeonidMaleRace, RecordType = FcsRecordType.Race, Name = "leonidmale" };

        RaceRecords.FillRace(race, Game(), Races.LeonidMale);

        Assert.Equal(1.25f, race.Floats["walk speed"]);
        foreach (string gender in new[] { "male", "female" })
        {
            Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_leonidmale.mesh", race.Filenames[$"{gender} mesh"]);
            Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_leonidmale_d.dds", race.Filenames[$"body texture {gender}"]);
            Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_leonidmale_n.dds", race.Filenames[$"nm {gender}"]);
            Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_leonidmale_parts.png", race.Filenames[$"part map {gender}"]);
            Assert.Empty(race.References[$"heads {gender}"]);
        }

        Assert.True(race.Bools["no shirts"]);
        Assert.False(race.Bools["playable"]);
        Assert.Empty(race.References["hairs"]);
    }

    [Fact]
    public void A_body_without_a_normal_map_names_none()
    {
        var race = new FcsRecord { StringId = Races.CrabmanRace, RecordType = FcsRecordType.Race, Name = "crabman" };

        RaceRecords.FillRace(race, Game(), Races.Crabman);

        Assert.Equal("", race.Filenames["nm male"]);
    }

    [Fact]
    public void A_character_is_a_kitless_male_Drifter_of_the_race_with_the_recruit_talk()
    {
        var character = new FcsRecord { StringId = Races.LeonidMaleCharacter, RecordType = FcsRecordType.Character, Name = "leonidmale" };

        RaceRecords.FillCharacter(character, Game(), Races.LeonidMale);

        Assert.Equal(7, character.Ints["strength"]);
        Assert.Equal(0, character.Ints["female chance"]);
        Assert.Empty(character.References["weapons"]);
        Assert.Equal(Races.LeonidMaleRace, Assert.Single(character.References["race"]).TargetStringId);
        Assert.Equal(Races.Recruit, Assert.Single(character.References["dialogue"]).TargetStringId);
    }

    [Fact]
    public void Ids_are_unique()
    {
        string[] ids = [Races.Recruit, .. Races.All.SelectMany(r => new[] { r.RaceId, r.CharacterId! })];

        Assert.Equal(ids.Length, ids.Distinct().Count());
    }

    /// <summary>
    /// A database holding a Greenlander and a Drifter (with a sword in its kit), from a data file written to a temporary folder.
    /// </summary>
    static GameDatabase Game()
    {
        var greenlander = new FcsRecord { StringId = RaceRecords.Greenlander, RecordType = FcsRecordType.Race, Name = "Greenlander" };
        greenlander.Floats["walk speed"] = 1.25f;
        greenlander.Filenames["male mesh"] = "human_male.mesh";
        var drifter = new FcsRecord { StringId = RaceRecords.Drifter, RecordType = FcsRecordType.Character, Name = "Drifter" };
        drifter.Ints["strength"] = 7;
        drifter.Ints["female chance"] = 50;
        drifter.References["weapons"] = [new FcsReference("9-test.mod", 1, 0, 0)];
        var sword = new FcsRecord { StringId = "9-test.mod", RecordType = FcsRecordType.Weapon, Name = "Sword" };

        string path = Path.Combine(Directory.CreateTempSubdirectory("kenshiraces").FullName, "test.mod");
        FcsWriter.WriteFile(new FcsFile { Records = { greenlander, drifter, sword } }, path);
        var order = new LoadOrder();
        order.Entries.Add(new LoadOrderEntry("test.mod", path));
        return GameDatabase.Load(order);
    }
}
