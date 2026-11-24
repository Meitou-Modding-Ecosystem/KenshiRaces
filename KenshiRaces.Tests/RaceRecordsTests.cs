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
        Assert.Contains(race.References["animation files"], f => f.TargetStringId == AnimationRecords.File);
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
    public void A_subraces_traits_replace_the_Greenlanders_aptitudes_and_set_its_part_health_slots_and_fists()
    {
        var race = new FcsRecord { StringId = Races.CrabmanRace, RecordType = FcsRecordType.Race, Name = "crabman" };

        RaceRecords.FillRace(race, Game(), Races.Crabman);

        int[] good = [.. race.Ints.Where(p => p.Key.StartsWith("stats good")).Select(p => p.Value)];
        int[] bad = [.. race.Ints.Where(p => p.Key.StartsWith("stats bad")).Select(p => p.Value)];
        Assert.Equal([30, 21, 2, 19, 23, 3, 5], good);
        Assert.Equal(12, bad.Length);
        Assert.Equal(1.2f, race.Floats["strength"]);
        Assert.Equal([(80, 100), (40, 250), (140, 200), (80, 120)], race.References["combat anatomy"].Select(p => (p.Value0, p.Value1)));
        Assert.True(race.Bools["no hats"]);
        Assert.False(race.Bools["no shoes"]);
        Assert.Equal(Races.CrabFists, Assert.Single(race.References["fists lv1"]).TargetStringId);
        Assert.True(race.Bools[RaceRecords.NoWeaponsField]);
        Assert.Equal(0.8f, race.Floats[RaceRecords.DamageTakenField]);
    }

    [Fact]
    public void Gear_limits_refuse_head_gear_off_the_list_named_footwear_and_weapons()
    {
        var db = Gear();
        var armour = db.OfType(FcsRecordType.Armour).ToList();
        string[] weapons = ["9-test.mod"];

        var crevan = RaceRules.RefusedBy(db.Find(Races.CrevanRace)!, armour, [.. weapons]);
        var crab = RaceRules.RefusedBy(db.Find(Races.CrabmanRace)!, armour, [.. weapons]);
        var leoma = RaceRules.RefusedBy(db.Find(Races.LeomaRace)!, armour, [.. weapons]);

        Assert.Equal(["h2-test.mod"], crevan.Order());
        Assert.Equal(["9-test.mod", "b2-test.mod"], crab.Order());
        Assert.Empty(leoma);
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
            AnimationRecords.File,
            Races.CrabFists,
            .. RaceAnimations.All.Select(a => a.TechniqueId),
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
        greenlander.Ints["stats good0"] = 4;
        greenlander.Ints["stats good1"] = 12;
        greenlander.Ints["stats bad0"] = 0;
        greenlander.References["combat anatomy"] =
            [new("32-test.mod", 80, 100, 0), new("29-test.mod", 40, 100, 0), new("101-test.mod", 140, 100, 0), new("31-test.mod", 80, 100, 0)];
        FcsRecord[] parts =
        [
            Part("32-test.mod", "Head"), Part("29-test.mod", "Right Arm"), Part("101-test.mod", "Chest"), Part("31-test.mod", "Right Leg"),
        ];
        var drifter = new FcsRecord { StringId = RaceRecords.Drifter, RecordType = FcsRecordType.Character, Name = "Drifter" };
        drifter.Ints["strength"] = 7;
        drifter.Ints["female chance"] = 50;
        drifter.References["weapons"] = [new FcsReference("9-test.mod", 1, 0, 0)];
        var sword = new FcsRecord { StringId = "9-test.mod", RecordType = FcsRecordType.Weapon, Name = "Sword" };

        string path = Path.Combine(Directory.CreateTempSubdirectory("kenshiraces").FullName, "test.mod");
        return Load([greenlander, drifter, sword, .. parts]);
    }

    /// <summary>
    /// The game of <see cref="Game"/> with the Crevan, Crabman and Leoma races and two hats (one an Ashland one) and two pairs of footwear (one
    /// sandals).
    /// </summary>
    static GameDatabase Gear()
    {
        var game = Game();
        var records = new List<FcsRecord>();
        foreach (var subrace in new[] { Races.Crevan, Races.Crabman, Races.Leoma })
        {
            var race = new FcsRecord { StringId = subrace.RaceId, RecordType = FcsRecordType.Race, Name = subrace.Name };
            RaceRecords.FillRace(race, game, subrace);
            records.Add(race);
        }

        records.Add(Armour("h1-test.mod", "Ashland Hat", 3));
        records.Add(Armour("h2-test.mod", "Straw Hat", 3));
        records.Add(Armour("b1-test.mod", "Samurai Boots", 9));
        records.Add(Armour("b2-test.mod", "Wooden Sandals", 9));
        return Load(records);
    }

    /// <summary>
    /// A LOCATIONAL_DAMAGE record.
    /// </summary>
    static FcsRecord Part(string id, string name) => new() { StringId = id, RecordType = FcsRecordType.LocationalDamage, Name = name };

    /// <summary>
    /// An ARMOUR record of an FCS slot.
    /// </summary>
    static FcsRecord Armour(string id, string name, int slot)
    {
        var record = new FcsRecord { StringId = id, RecordType = FcsRecordType.Armour, Name = name };
        record.Ints["slot"] = slot;
        return record;
    }

    /// <summary>
    /// A database of <paramref name="records"/>, from a data file written to a temporary folder.
    /// </summary>
    static GameDatabase Load(IEnumerable<FcsRecord> records)
    {
        string path = Path.Combine(Directory.CreateTempSubdirectory("kenshiraces").FullName, "test.mod");
        var file = new FcsFile();
        file.Records.AddRange(records);
        FcsWriter.WriteFile(file, path);
        var order = new LoadOrder();
        order.Entries.Add(new LoadOrderEntry("test.mod", path));
        return GameDatabase.Load(order);
    }
}
