using KenshiRaces.Introductions;
using Meitou.Data;
using Meitou.Data.Fcs;

namespace KenshiRaces.Tests;

/// <summary>
/// The introducer's records (built from a synthetic data file standing in for the game) and the debug row's field.
/// </summary>
public sealed class IntroductionTests
{
    #region Records

    [Fact]
    public void The_introducer_is_a_Greenlander_in_armoured_rags_and_an_ashland_hat_with_his_talk()
    {
        var him = Records()[IntroducerData.Character];

        Assert.Equal(IntroducerData.Name, him.Name);
        Assert.Equal(RaceRecords.Greenlander, Assert.Single(him.References["race"]).TargetStringId);
        Assert.Equal([IntroducerData.ArmouredRags, IntroducerData.AshlandHat], him.References["clothing"].Select(r => r.TargetStringId));
        Assert.Equal(IntroducerData.Talk, Assert.Single(him.References["dialogue"]).TargetStringId);
        Assert.Equal(0, him.Ints["female chance"]);
        Assert.Empty(him.References["weapons"]);
    }

    [Fact]
    public void Every_link_of_the_conversation_names_one_of_its_records()
    {
        var records = Records();
        var missing = records.Values
            .Where(r => r.RecordType is FcsRecordType.Dialogue or FcsRecordType.DialogueLine)
            .SelectMany(r => new[] { "lines", "conditions", "effects" }.SelectMany(list => r.References.GetValueOrDefault(list) ?? []))
            .Select(l => l.TargetStringId)
            .Where(id => !records.ContainsKey(id))
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Each_race_character_is_offered_for_the_price_only_while_none_is_on_its_way_and_pays_and_orders_it()
    {
        var records = Records();
        var picks = records.Values.Where(r => r.References.GetValueOrDefault("effects") is { Count: 2 }).ToList();

        foreach (var character in Races.All.SelectMany(s => s.Characters))
        {
            int number = IntroducerData.Number(character);
            var pick = Assert.Single(picks, p => p.References["effects"][1].Value0 == number);
            var conditions = pick.References["conditions"].Select(c => (records[c.TargetStringId].Ints["condition name"], c.Value0));
            Assert.Equal([(2, IntroducerData.Price), (IntroducerData.NoneOnTheWay, 1)], conditions);
            Assert.Equal(8, records[pick.References["effects"][0].TargetStringId].Ints["action name"]);
            Assert.Equal(IntroducerData.Price, pick.References["effects"][0].Value0);
            Assert.Equal(IntroducerData.Introduce, records[pick.References["effects"][1].TargetStringId].Ints["action name"]);
        }
    }

    [Fact]
    public void Asking_for_a_friend_gets_the_offer_while_none_is_on_its_way_else_a_refusal()
    {
        var records = Records();
        var ask = records.Values.Single(r => r.Strings.GetValueOrDefault("text0") == "Introduce me to one of your friends.");

        var answers = ask.References["lines"].Select(l => Assert.Single(records[l.TargetStringId].References["conditions"]).Value0);

        Assert.Equal([1, 0], answers);
    }

    [Fact]
    public void Each_race_character_has_a_squad_of_its_own_that_it_leads()
    {
        var records = Records();

        foreach (var character in Races.All.SelectMany(s => s.Characters))
        {
            var squad = records[IntroducerData.ArrivalSquad(character)];
            Assert.Equal(FcsRecordType.SquadTemplate, squad.RecordType);
            Assert.Equal(character.Id, Assert.Single(squad.References["leader"]).TargetStringId);
            Assert.Equal(IntroducerData.Drifters, Assert.Single(squad.References["faction"]).TargetStringId);
            Assert.Same(character, IntroducerData.Of(IntroducerData.Number(character)));
        }
    }

    [Fact]
    public void The_records_take_no_id_twice_nor_one_of_the_races()
    {
        var ids = IntroducerData.Build(Game()).Select(r => r.StringId).ToList();
        var races = Races.All.SelectMany(s => s.Characters.Select(c => c.Id).Append(s.RaceId)).ToHashSet();

        Assert.Equal(ids.Count, ids.Distinct().Count());
        Assert.DoesNotContain(ids, races.Contains);
    }

    #endregion

    #region Debug row

    [Theory]
    [InlineData("crabman", Races.CrabmanCharacter, false)]
    [InlineData("leomafemale hostile", Races.LeomaFemaleCharacter, true)]
    [InlineData("Leoma Female", Races.LeomaFemaleCharacter, false)]
    [InlineData("hostile crevan", Races.CrevanMaleCharacter, true)]
    [InlineData("tiefling join", Races.TieflingMaleCharacter, false)]
    [InlineData("tieflingfemale", Races.TieflingFemaleCharacter, false)]
    [InlineData("crabqueen female", Races.CrabQueenCharacter, false)]
    [InlineData("beepbot hostile", Races.BeepbotCharacter, true)]
    public void The_spawn_field_names_a_character_or_a_subrace_and_hostile_or_not(string field, string id, bool hostile)
    {
        var parsed = SpawnRaceRow.Parse(field);

        Assert.Equal((id, hostile), (parsed?.Character.Id, parsed?.Hostile));
    }

    [Theory]
    [InlineData("")]
    [InlineData("hostile")]
    [InlineData("shek")]
    public void A_field_naming_no_race_spawns_nothing(string field) => Assert.Null(SpawnRaceRow.Parse(field));

    #endregion

    #region Game

    /// <summary>
    /// The introducer's records by id.
    /// </summary>
    static Dictionary<string, FcsRecord> Records() => IntroducerData.Build(Game()).ToDictionary(r => r.StringId);

    /// <summary>
    /// A data file with what the records copy and name: the Drifter, the Greenlander, the Drifters, Armoured Rags and an Ashland Hat.
    /// </summary>
    static GameDatabase Game()
    {
        var drifter = new FcsRecord { StringId = RaceRecords.Drifter, RecordType = FcsRecordType.Character, Name = RaceRecords.DrifterName };
        drifter.Ints["female chance"] = 50;
        drifter.References["weapons"] = [new FcsReference("9-test.mod", 1, 0, 0)];
        FcsRecord[] records =
        [
            drifter,
            new() { StringId = RaceRecords.Greenlander, RecordType = FcsRecordType.Race, Name = "Greenlander" },
            new() { StringId = IntroducerData.Drifters, RecordType = FcsRecordType.Faction, Name = "Drifters" },
            new() { StringId = IntroducerData.ArmouredRags, RecordType = FcsRecordType.Armour, Name = "Armoured Rags" },
            new() { StringId = IntroducerData.AshlandHat, RecordType = FcsRecordType.Armour, Name = "Ashland Hat" },
            new() { StringId = "9-test.mod", RecordType = FcsRecordType.Weapon, Name = "Sword" },
        ];

        string path = Path.Combine(Directory.CreateTempSubdirectory("kenshiraces").FullName, "test.mod");
        var file = new FcsFile();
        file.Records.AddRange(records);
        FcsWriter.WriteFile(file, path);
        var order = new LoadOrder();
        order.Entries.Add(new LoadOrderEntry("test.mod", path));
        return GameDatabase.Load(order);
    }

    #endregion
}
