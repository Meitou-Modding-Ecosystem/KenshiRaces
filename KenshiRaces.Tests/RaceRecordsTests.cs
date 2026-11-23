using Meitou.Data;
using Meitou.Data.Fcs;

namespace KenshiRaces.Tests;

/// <summary>
/// A subrace's records are the game's Greenlander and Drifter with the bodies' files, no kit and the recruit talk, built from a synthetic data
/// file standing in for the game.
/// </summary>
public sealed class RaceRecordsTests
{
    [Fact]
    public void A_subrace_is_a_playable_Greenlander_with_each_genders_body()
    {
        var race = new FcsRecord { StringId = Races.LeomaRace, RecordType = FcsRecordType.Race, Name = "leoma" };

        RaceRecords.FillRace(race, Game(), Races.Leoma);

        Assert.Equal(1.25f, race.Floats["walk speed"]);
        foreach (string gender in new[] { "male", "female" })
        {
            string stem = @".\mods\KenshiRaces\items\races\kenshiraces_leoma" + gender;
            Assert.Equal(stem + ".mesh", race.Filenames[$"{gender} mesh"]);
            Assert.Equal(stem + "_d.dds", race.Filenames[$"body texture {gender}"]);
            Assert.Equal(stem + "_n.dds", race.Filenames[$"nm {gender}"]);
            Assert.Equal(stem + "_parts.png", race.Filenames[$"part map {gender}"]);
            Assert.Empty(race.References[$"heads {gender}"]);
        }

        Assert.False(race.Bools["single gender"]);
        Assert.True(race.Bools["no shirts"]);
        Assert.True(race.Bools["playable"]);
        Assert.Equal(RaceRecords.EditorLimits, race.Filenames["editor limits"]);
        Assert.Empty(race.References["hairs"]);
    }

    [Fact]
    public void A_single_gender_subrace_wears_its_one_body_for_both_and_names_no_normal_map_without_one()
    {
        var race = new FcsRecord { StringId = Races.CrabmanRace, RecordType = FcsRecordType.Race, Name = "crabman" };

        RaceRecords.FillRace(race, Game(), Races.Crabman);

        Assert.True(race.Bools["single gender"]);
        Assert.Equal(race.Filenames["male mesh"], race.Filenames["female mesh"]);
        Assert.Equal("", race.Filenames["nm male"]);
    }

    [Fact]
    public void A_character_is_a_kitless_Drifter_of_its_gender_and_subrace_with_the_recruit_talk()
    {
        var female = Races.Leoma.Characters.Single(c => c.Female);
        var character = new FcsRecord { StringId = female.Id, RecordType = FcsRecordType.Character, Name = female.Name };

        RaceRecords.FillCharacter(character, Game(), Races.Leoma, female);

        Assert.Equal(7, character.Ints["strength"]);
        Assert.Equal(100, character.Ints["female chance"]);
        Assert.Empty(character.References["weapons"]);
        Assert.Equal(Races.LeomaRace, Assert.Single(character.References["race"]).TargetStringId);
        Assert.Equal(Races.Recruit, Assert.Single(character.References["dialogue"]).TargetStringId);
    }

    [Fact]
    public void Ids_are_unique()
    {
        string[] ids =
        [
            Races.Recruit,
            .. Races.Groups.Select(g => g.Id),
            .. Races.All.Select(s => s.RaceId),
            .. Races.All.SelectMany(s => s.Characters).Select(c => c.Id),
        ];

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
