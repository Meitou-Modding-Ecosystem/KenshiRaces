using Meitou.Data;
using Meitou.Data.Fcs;

namespace KenshiRaces.Introductions;

/// <summary>
/// The introducer's records (docs/kenshi-races.md "Getting the races"): his CHARACTER, a Drifter Greenlander in Armoured Rags and an Ashland Hat,
/// his SQUAD_TEMPLATE, his conversation (records 200 to 299 of the mod's ids), and one SQUAD_TEMPLATE per race character for the friend who
/// arrives (300 + the character's number). The kit and faction name the game's records by id; a linked record the game lacks is left out.
/// </summary>
static class IntroducerData
{
    #region Ids

    /// <summary>
    /// His CHARACTER, his SQUAD_TEMPLATE and his DIALOGUE.
    /// </summary>
    public const string Character = "200-" + Races.FileName, Squad = "201-" + Races.FileName, Talk = "202-" + Races.FileName;

    /// <summary>
    /// His name.
    /// </summary>
    public const string Name = "Marlo the Far-Walker";

    /// <summary>
    /// The introduction's price in cats.
    /// </summary>
    public const int Price = 10000;

    /// <summary>
    /// The dialogue codes the mod handles (README "Numbers"): condition 5000 holds when no introduction is on its way (value 1) or one is (value 0);
    /// effect 5000 orders the introduction of the race character whose record number is the value.
    /// </summary>
    public const int NoneOnTheWay = 5000, Introduce = 5000;

    /// <summary>
    /// The SQUAD_TEMPLATE of the friend who arrives as a race character: that character alone.
    /// </summary>
    public static string ArrivalSquad(RaceCharacter character) => $"{300 + Number(character)}-{Races.FileName}";

    /// <summary>
    /// The record number of a race character (the number before the file name in its id).
    /// </summary>
    public static int Number(RaceCharacter character) => int.Parse(character.Id[..character.Id.IndexOf('-')]);

    /// <summary>
    /// The race character of a record number; null for none of this mod's.
    /// </summary>
    public static RaceCharacter? Of(int number) =>
        Races.All.SelectMany(s => s.Characters).FirstOrDefault(c => c.Id == $"{number}-{Races.FileName}");

    /// <summary>
    /// What the player reads for each race character in messages: "crab queen", "Leoma woman".
    /// </summary>
    static readonly Dictionary<string, string> Labels = new()
    {
        [Races.LeomaMaleCharacter] = "Leoma man",
        [Races.LeomaFemaleCharacter] = "Leoma woman",
        [Races.CrevanMaleCharacter] = "Crevan man",
        [Races.CrevanFemaleCharacter] = "Crevan woman",
        [Races.CrabmanCharacter] = "crabman",
        [Races.CrabQueenCharacter] = "crab queen",
        [Races.SpidercrabCharacter] = "spider crab",
        [Races.GhostcrabCharacter] = "ghost crab",
        [Races.TieflingCharacter] = "tiefling",
        [Races.BatfolkMaleCharacter] = "Batfolk man",
        [Races.BatfolkFemaleCharacter] = "Batfolk woman",
        [Races.WolfkinMaleCharacter] = "Wolfkin man",
        [Races.WolfkinFemaleCharacter] = "Wolfkin woman",
        [Races.BeepbotCharacter] = "beepbot",
    };

    /// <summary>
    /// A race character as the player reads it in messages; its name for one without a label.
    /// </summary>
    public static string Label(RaceCharacter character) => Labels.GetValueOrDefault(character.Id, character.Name);

    /// <summary>
    /// The game's records he names: the Drifters (his faction and his friends'), Armoured Rags and an Ashland Hat.
    /// </summary>
    public const string Drifters = "51707-rebirth.mod", ArmouredRags = "2307-clothes_v1.mod", AshlandHat = "55393-rebirth.mod";

    /// <summary>
    /// The squad flags he and his friends have off: no home, no regrowth.
    /// </summary>
    static readonly string[] OffFlags = ["building public", "regenerates", "roaming military", "malnourished", "sell home"];

    #endregion

    #region Records

    /// <summary>
    /// Every record, in id order: his character, squad and conversation, then the friends' squads.
    /// </summary>
    public static IEnumerable<FcsRecord> Build(GameDatabase game) =>
        [CharacterRecord(game), SquadRecord(game, Squad, Name, Character), .. Conversation(), .. ArrivalSquads(game)];

    /// <summary>
    /// The introducer: a Drifter (<see cref="RaceRecords.FillCharacter"/>'s model) of the Greenlander race, a man, in Armoured Rags and an Ashland
    /// Hat, with his conversation.
    /// </summary>
    static FcsRecord CharacterRecord(GameDatabase game)
    {
        var record = New(Character, FcsRecordType.Character, Name);
        RaceRecords.CopyFrom(RaceRecords.Model(game, FcsRecordType.Character, RaceRecords.Drifter, RaceRecords.DrifterName), record);
        record.Ints["female chance"] = 0;
        record.Ints["money min"] = 0;
        record.Ints["money max"] = 0;
        record.Ints["bounty chance"] = 0;
        foreach (string list in RaceRecords.KitLists) record.References[list] = [];

        record.References["clothing"] = Links(game, [ArmouredRags, AshlandHat], 1, 100);
        record.References["race"] = Links(game, [RaceRecords.Greenlander], 100, 0);
        record.References["dialogue"] = [new FcsReference(Talk, 0, 0, 0)];
        return record;
    }

    /// <summary>
    /// A squad of one character, of the Drifters, with no AI package: the mod holds and walks it.
    /// </summary>
    static FcsRecord SquadRecord(GameDatabase game, string id, string name, string leader)
    {
        var record = New(id, FcsRecordType.SquadTemplate, name);
        foreach (string flag in OffFlags) record.Bools[flag] = false;

        record.Bools["dont multiply"] = true;
        record.Ints["num random chars"] = 0;
        record.Ints["num random chars max"] = 0;
        record.Bools["is trader"] = false;
        record.Strings["building name"] = "";
        record.Strings["layout interior"] = "";
        record.Strings["layout exterior"] = "";
        record.References["leader"] = [new FcsReference(leader, 1, 0, 0)];
        record.References["faction"] = Links(game, [Drifters], 0, 0);
        return record;
    }

    /// <summary>
    /// One squad per race character, named for it.
    /// </summary>
    static IEnumerable<FcsRecord> ArrivalSquads(GameDatabase game) =>
        Races.All.SelectMany(s => s.Characters).Select(c => SquadRecord(game, ArrivalSquad(c), "Marlo's friend: " + c.Name, c.Id));

    /// <summary>
    /// Links to those of the game's records that exist, each with the values given.
    /// </summary>
    static List<FcsReference> Links(GameDatabase game, IEnumerable<string> ids, int value0, int value1) =>
        [.. ids.Where(id => game.Find(id) is not null).Select(id => new FcsReference(id, value0, value1, 0))];

    /// <summary>
    /// A new record of the mod.
    /// </summary>
    static FcsRecord New(string id, FcsRecordType type, string name) => new() { StringId = id, RecordType = type, Name = name };

    #endregion

    #region Conversation

    /// <summary>
    /// The DIALOG_ACTIONs: DA 8 take money, the mod's introduce effect, and the conditions DC 2 player money and the mod's none-on-the-way.
    /// </summary>
    const int PayAction = 203, IntroduceAction = 204, FreeCondition = 205, MoneyCondition = 206;

    /// <summary>
    /// The game's DIALOG_ACTION numbers used: DA 8 TAKE_MONEY, DC 2 PLAYERMONEY (compare by 0: at least the value, 1: below it).
    /// </summary>
    const int TakeMoney = 8, PlayerMoney = 2, AtLeast = 0;

    /// <summary>
    /// His lines and the player's (from 210), in record number order.
    /// </summary>
    const int Greeting = 210, AskWhere = 211, Where = 212, AskFriends = 213, Friends = 214, Doubt = 215, Proof = 216, AskMeet = 217, Offer = 218,
        Busy = 219, Bye = 220, Decline = 221, Shrug = 222, Deal = 223;

    /// <summary>
    /// The first line number of each race group's pair (the player's pick, then his answer listing its characters), two per group from 230; the
    /// player's pick of a character is 260 + its record number.
    /// </summary>
    const int FirstGroupLine = 230, FirstCharacterLine = 260;

    /// <summary>
    /// The speakers: him, then the player.
    /// </summary>
    const int Him = 0, Player = 1;

    /// <summary>
    /// The player's choices at his greeting and after each of his answers.
    /// </summary>
    static readonly int[] Choices = [AskWhere, AskFriends, AskMeet, Bye];

    /// <summary>
    /// The DIALOGUE, its actions and lines.
    /// </summary>
    static IEnumerable<FcsRecord> Conversation()
    {
        var dialogue = New(Talk, FcsRecordType.Dialogue, "kenshi races introducer");
        dialogue.Bools["for enemies"] = false;
        dialogue.Bools["locked"] = false;
        dialogue.Bools["monologue"] = true;
        dialogue.Bools["one at a time"] = false;
        dialogue.Strings["tag"] = "";
        Chances(dialogue);
        dialogue.References["lines"] = [Ref(Greeting)];

        return [dialogue, .. Actions(), .. Lines(), .. Groups()];
    }

    /// <summary>
    /// The four DIALOG_ACTIONs.
    /// </summary>
    static IEnumerable<FcsRecord> Actions()
    {
        var pay = New(Id(PayAction), FcsRecordType.DialogAction, "DIALOG_ACTION");
        pay.Ints["action name"] = TakeMoney;
        var introduce = New(Id(IntroduceAction), FcsRecordType.DialogAction, "DIALOG_ACTION");
        introduce.Ints["action name"] = Introduce;
        return [pay, introduce, Condition(FreeCondition, NoneOnTheWay), Condition(MoneyCondition, PlayerMoney)];
    }

    /// <summary>
    /// A condition DIALOG_ACTION (compare by 0, who 0).
    /// </summary>
    static FcsRecord Condition(int number, int code)
    {
        var record = New(Id(number), FcsRecordType.DialogAction, "DIALOG_ACTION");
        record.Ints["condition name"] = code;
        record.Ints["compare by"] = AtLeast;
        record.Ints["who"] = 0;
        return record;
    }

    /// <summary>
    /// The talk before the choice of a race: greeting, his travels, his friends, the offer, and the refusal while a friend is on the way.
    /// </summary>
    static IEnumerable<FcsRecord> Lines() =>
    [
        Line(Greeting, Him, Choices,
            "Psst. You there. You've the look of someone who's walked a few roads. Not as many as old Marlo, mind. Nobody has."),
        Line(AskWhere, Player, [Where], "Where have you been, then?"),
        Line(Where, Him, Choices,
            "Past the edge of every map the Hub ever sold. South over the acid sea, east where the Ashlands just... stop. Sand like glass, skies"
                + " the wrong colour. Lost a boot in a place the old machines never reached. Came back with stories nobody wants to hear."),
        Line(AskFriends, Player, [Friends], "Strange places. Strange people too?"),
        Line(Friends, Him, [Doubt, .. Choices],
            "Friends, I'd call them. Lion folk with manes like a beak thing's ruff. Crow people, feathers and all, who'd steal the teeth from"
                + " your mouth. Crabs that walk like men and think like them too. A lass with horns who smiles like she knows how you'll die."
                + " Bats with faces like ours, and wolves who'll hear you before you see them."),
        Line(Doubt, Player, [Proof], "You expect me to believe that?"),
        Line(Proof, Him, Choices,
            "No. Nobody does. That's why I'm drinking alone in Black Scratch. But a man gets lonely out there, and a friend owes a favour."
                + " Pay me and I'll send word. They'll come and find you. Then you can tell me I'm a liar."),
        Line(AskMeet, Player, [Offer, Busy], "Introduce me to one of your friends."),
        Line(Offer, Him, [.. GroupPicks(), Decline],
            "Ten thousand cats. Don't look at me like that, the road's long and the messages cost. Who'd you like to meet?",
            conditions: [(FreeCondition, 1)]),
        Line(Busy, Him, Choices,
            "One at a time, friend. The last one's still on the road to you, and I'm not sending another until you've met them.",
            conditions: [(FreeCondition, 0)]),
        Line(Bye, Player, [], "I'll be going."),
        Line(Decline, Player, [Shrug], "Not today."),
        Line(Shrug, Him, Choices, "Suit yourself. I'll be here. Nobody else listens to me."),
        Line(Deal, Him, Choices,
            "Done. I'll send word tonight. Give it three days and they'll turn up at your outpost. No outpost? Then they'll wait until you've built"
                + " one. Patient folk, my friends. Mostly."),
    ];

    /// <summary>
    /// The player's pick of each race group.
    /// </summary>
    static int[] GroupPicks() => [.. Races.Groups.Index().Select(g => FirstGroupLine + 2 * g.Index)];

    /// <summary>
    /// What he says of each group.
    /// </summary>
    static readonly Dictionary<string, string> Sayings = new()
    {
        [Races.LeomaGroup] = "The Leoma. Proud as a Shek and twice as hairy. Don't stare at the mane. A man or a woman?",
        [Races.CrevanGroup] = "The Crevan. Clever, light-fingered, and they never forget a face. Count your cats after. A man or a woman?",
        [Races.CrabfolkGroup] = "Crab people, from the shallows past the end of the world. Which shell do you want?",
        [Races.TieflingGroup] = "Her. I'll warn her you're coming. Don't ask about the horns.",
        [Races.BatfolkGroup] = "The Batfolk. Faces like yours, wings like nothing you've seen. They sleep all day and see in the dark. A man or a woman?",
        [Races.WolfkinGroup] =
            "The Wolfkin. They hear you coming a mile off, and they run in packs. Loyal to the bone, once you're one of theirs. A man or a woman?",
        [Races.BeepbotGroup] = "A beepbot. Scrap and springs that walks and talks, more or less. Nobody knows who built them, and they don't say.",
    };

    /// <summary>
    /// The player's name for each group.
    /// </summary>
    static readonly Dictionary<string, string> Picks = new()
    {
        [Races.LeomaGroup] = "The lion folk.",
        [Races.CrevanGroup] = "The crow people.",
        [Races.CrabfolkGroup] = "The crabs.",
        [Races.TieflingGroup] = "The woman with horns.",
        [Races.BatfolkGroup] = "The bat people.",
        [Races.WolfkinGroup] = "The wolf people.",
        [Races.BeepbotGroup] = "The machine.",
    };

    /// <summary>
    /// The player's name for each race character.
    /// </summary>
    static readonly Dictionary<string, string> Characters = new()
    {
        [Races.LeomaMaleCharacter] = "A Leoma man.",
        [Races.LeomaFemaleCharacter] = "A Leoma woman.",
        [Races.CrevanMaleCharacter] = "A Crevan man.",
        [Races.CrevanFemaleCharacter] = "A Crevan woman.",
        [Races.CrabmanCharacter] = "A crabman.",
        [Races.CrabQueenCharacter] = "A crab queen.",
        [Races.SpidercrabCharacter] = "A spider crab.",
        [Races.GhostcrabCharacter] = "A ghost crab.",
        [Races.TieflingCharacter] = "Bring her.",
        [Races.BatfolkMaleCharacter] = "A Batfolk man.",
        [Races.BatfolkFemaleCharacter] = "A Batfolk woman.",
        [Races.WolfkinMaleCharacter] = "A Wolfkin man.",
        [Races.WolfkinFemaleCharacter] = "A Wolfkin woman.",
        [Races.BeepbotCharacter] = "Bring the machine.",
    };

    /// <summary>
    /// Per race group: the player's pick, his answer, and the player's pick of each of its characters, shown only with the price in hand, which
    /// pays him and orders the introduction.
    /// </summary>
    static IEnumerable<FcsRecord> Groups()
    {
        foreach (var (index, group) in Races.Groups.Index())
        {
            int pick = FirstGroupLine + 2 * index;
            var characters = group.Subraces.SelectMany(s => s.Characters).ToList();
            int[] picks = [.. characters.Select(c => FirstCharacterLine + Number(c)), Decline];
            yield return Line(pick, Player, [pick + 1], Picks.GetValueOrDefault(group.Id, group.Name + "."));
            yield return Line(pick + 1, Him, picks, Sayings.GetValueOrDefault(group.Id, "Them? I know a few. Which one?"));
            foreach (var character in characters) yield return CharacterPick(character);
        }
    }

    /// <summary>
    /// The player's pick of a race character: with at least the price (DC 2) and none on the way, it pays him (DA 8) and orders the introduction.
    /// </summary>
    static FcsRecord CharacterPick(RaceCharacter character)
    {
        int number = Number(character);
        var line = Line(FirstCharacterLine + number, Player, [Deal], Characters.GetValueOrDefault(character.Id, character.Name + "."),
            conditions: [(MoneyCondition, Price), (FreeCondition, 1)]);
        line.References["effects"] = [new FcsReference(Id(PayAction), Price, 0, 0), new FcsReference(Id(IntroduceAction), number, 0, 0)];
        return line;
    }

    /// <summary>
    /// A DIALOGUE_LINE: who says it, its text, the lines that may follow and its conditions (each a condition record's number and its value).
    /// </summary>
    static FcsRecord Line(int number, int speaker, int[] next, string text, (int Record, int Value)[]? conditions = null)
    {
        var line = New(Id(number), FcsRecordType.DialogueLine, "DIALOGUE_LINE");
        Chances(line);
        line.Ints["speaker"] = speaker;
        line.Strings["text0"] = text;
        if (next.Length > 0) line.References["lines"] = [.. next.Select(Ref)];
        if (conditions is not null) line.References["conditions"] = [.. conditions.Select(c => new FcsReference(Id(c.Record), c.Value, 0, 0))];

        return line;
    }

    /// <summary>
    /// The fields every line and dialogue of the game carries: always chosen, no repetition limit, no score bonus, any target.
    /// </summary>
    static void Chances(FcsRecord record)
    {
        record.Floats["chance permanent"] = 100;
        record.Floats["chance temporary"] = 100;
        record.Ints["repetition limit"] = 0;
        record.Ints["score bonus"] = 0;
        record.Ints["target is type"] = 0;
    }

    /// <summary>
    /// A link to the record of a number.
    /// </summary>
    static FcsReference Ref(int number) => new(Id(number), 0, 0, 0);

    /// <summary>
    /// The string id of a record number.
    /// </summary>
    static string Id(int number) => $"{number}-{Races.FileName}";

    #endregion
}
