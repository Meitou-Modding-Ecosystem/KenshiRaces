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
    public void A_subrace_wearing_the_human_head_keeps_the_Greenlanders_heads_hair_and_random_faces_with_the_face_sliders()
    {
        var race = new FcsRecord { StringId = Races.BatfolkRace, RecordType = FcsRecordType.Race, Name = "batfolk" };

        RaceRecords.FillRace(race, Game(), Races.Batfolk);

        Assert.Equal("1-heads.mod", Assert.Single(race.References["heads male"]).TargetStringId);
        Assert.Equal("2-heads.mod", Assert.Single(race.References["heads female"]).TargetStringId);
        Assert.Equal("3-hair.mod", Assert.Single(race.References["hairs"]).TargetStringId);
        Assert.Equal(5, race.Ints["morph num"]);
        Assert.Equal(RaceRecords.FaceEditorLimits, race.Filenames["editor limits"]);
        Assert.False(race.Bools["single gender"]);
    }

    [Fact]
    public void A_skin_toned_subrace_takes_its_bodies_masks_and_the_tinted_editor_limits()
    {
        var race = new FcsRecord { StringId = Races.TieflingRace, RecordType = FcsRecordType.Race, Name = "tiefling" };

        RaceRecords.FillRace(race, Game(), Races.Tiefling);

        Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_tieflingmale_mask.dds", race.Filenames["body mask male"]);
        Assert.Equal(@".\mods\KenshiRaces\items\races\kenshiraces_tieflingfemale_mask.dds", race.Filenames["body mask female"]);
        Assert.Equal(RaceRecords.TintedEditorLimits, race.Filenames["editor limits"]);
        Assert.Equal("1-heads.mod", Assert.Single(race.References["heads male"]).TargetStringId);
        Assert.False(race.Bools["single gender"]);
    }

    [Fact]
    public void A_subrace_without_a_skin_tone_has_no_body_mask()
    {
        var race = new FcsRecord { StringId = Races.BatfolkRace, RecordType = FcsRecordType.Race, Name = "batfolk" };

        RaceRecords.FillRace(race, Game(), Races.Batfolk);

        Assert.Equal("", race.Filenames["body mask male"]);
    }

    [Fact]
    public void A_subrace_with_its_own_head_has_no_heads_hair_or_random_faces()
    {
        var race = new FcsRecord { StringId = Races.LeomaRace, RecordType = FcsRecordType.Race, Name = "leoma" };

        RaceRecords.FillRace(race, Game(), Races.Leoma);

        Assert.Empty(race.References["heads male"]);
        Assert.Empty(race.References["heads female"]);
        Assert.Empty(race.References["hairs"]);
        Assert.Equal(0, race.Ints["morph num"]);
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

    [Theory]
    [InlineData("beepbot")]
    [InlineData("katlon")]
    [InlineData("chibot")]
    public void A_skeleton_subrace_copies_the_Skeletons_mechanics_with_its_own_body_and_gear_slots(string name)
    {
        var subrace = Races.All.Single(r => r.Name == name);
        var race = new FcsRecord { StringId = subrace.RaceId, RecordType = FcsRecordType.Race, Name = name };

        RaceRecords.FillRace(race, Game(), subrace);

        Assert.True(race.Bools["is robot"]);
        Assert.Equal(0, race.Floats["hunger rate"]);
        Assert.Equal(6, race.Ints["heal stat"]);
        Assert.Equal([28, 11], race.Ints.Where(p => p.Key.StartsWith("stats good")).Select(p => p.Value));
        Assert.Equal(200, Assert.Single(race.References["combat anatomy"]).Value1);
        Assert.Equal("robo-fists.mod", Assert.Single(race.References["fists lv1"]).TargetStringId);
        Assert.False(race.Bools["no hats"]);
        Assert.True(race.Bools["no shoes"]);
        Assert.Empty(race.References["heads male"]);
        Assert.Equal(subrace.Body(female: false).Mesh, race.Filenames["male mesh"]);
    }

    [Fact]
    public void The_beepbot_the_Kat_Lon_and_the_chibot_join_the_games_Skeleton_race()
    {
        Assert.Equal([Races.Beepbot, Races.Katlon, Races.Chibot], Races.SkeletonRaces.Subraces);
        Assert.Equal("42903-rebirth.mod", Races.SkeletonRaces.Id);
        Assert.True(Races.SkeletonRaces.Existing);
        Assert.All(Races.Groups.Where(g => g != Races.SkeletonRaces), g => Assert.False(g.Existing));
        Assert.Same(Races.BeepbotTraits, Races.Katlon.Traits);
        Assert.Same(Races.BeepbotTraits, Races.Chibot.Traits);
    }

    [Fact]
    public void The_subraces_keep_their_record_ids_so_saves_find_them()
    {
        Assert.Equal("16-KenshiRaces.mod", Races.BeepbotRace);
        Assert.Equal("20-KenshiRaces.mod", Races.BeepbotCharacter);
        Assert.Equal("47-KenshiRaces.mod", Races.KatlonRace);
        Assert.Equal("48-KenshiRaces.mod", Races.KatlonCharacter);
        Assert.Equal("49-KenshiRaces.mod", Races.ChibotRace);
        Assert.Equal("51-KenshiRaces.mod", Races.ChibotCharacter);
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
    /// A database holding a Greenlander, a Skeleton and a Drifter (with a sword in its kit), from a data file written to a temporary folder.
    /// </summary>
    static GameDatabase Game()
    {
        var greenlander = new FcsRecord { StringId = RaceRecords.Greenlander, RecordType = FcsRecordType.Race, Name = "Greenlander" };
        greenlander.Floats["walk speed"] = 1.25f;
        greenlander.Filenames["male mesh"] = "human_male.mesh";
        greenlander.Ints["morph num"] = 5;
        greenlander.References["heads male"] = [new("1-heads.mod", 100, 0, 0)];
        greenlander.References["heads female"] = [new("2-heads.mod", 100, 0, 0)];
        greenlander.References["hairs"] = [new("3-hair.mod", 100, 0, 0)];
        greenlander.Ints["stats good0"] = 4;
        greenlander.Ints["stats good1"] = 12;
        greenlander.Ints["stats bad0"] = 0;
        greenlander.References["combat anatomy"] =
            [new("32-test.mod", 80, 100, 0), new("29-test.mod", 40, 100, 0), new("101-test.mod", 140, 100, 0), new("31-test.mod", 80, 100, 0)];
        FcsRecord[] parts =
        [
            Part("32-test.mod", "Head"), Part("29-test.mod", "Right Arm"), Part("101-test.mod", "Chest"), Part("31-test.mod", "Right Leg"),
        ];
        var skeleton = new FcsRecord { StringId = RaceModel.Skeleton.Id, RecordType = FcsRecordType.Race, Name = "Skeleton" };
        skeleton.Bools["is robot"] = true;
        skeleton.Bools["no hats"] = true;
        skeleton.Bools["no shoes"] = true;
        skeleton.Floats["hunger rate"] = 0;
        skeleton.Ints["heal stat"] = 6;
        skeleton.Ints["stats good0"] = 28;
        skeleton.Ints["stats good1"] = 11;
        skeleton.References["combat anatomy"] = [new("32-test.mod", 80, 200, 0)];
        skeleton.References["heads male"] = [new("4-heads.mod", 100, 0, 0)];
        skeleton.References["fists lv1"] = [new("robo-fists.mod", 0, 0, 0)];
        var drifter = new FcsRecord { StringId = RaceRecords.Drifter, RecordType = FcsRecordType.Character, Name = "Drifter" };
        drifter.Ints["strength"] = 7;
        drifter.Ints["female chance"] = 50;
        drifter.References["weapons"] = [new FcsReference("9-test.mod", 1, 0, 0)];
        var sword = new FcsRecord { StringId = "9-test.mod", RecordType = FcsRecordType.Weapon, Name = "Sword" };

        string path = Path.Combine(Directory.CreateTempSubdirectory("kenshiraces").FullName, "test.mod");
        return Load([greenlander, skeleton, drifter, sword, .. parts]);
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
