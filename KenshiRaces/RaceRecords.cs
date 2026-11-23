using Meitou.Data;
using Meitou.Data.Fcs;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// Builds a fitted race's records in the data phase (docs/kenshi-races.md "Records"): a RACE per subrace copied from the game's Greenlander with
/// the fitted bodies' files, a CHARACTER per character copied from the game's Drifter with no kit and the recruit talk, and a RACE_GROUP per race
/// so the character editor lists it. Copies of the loaded game's own records, so no game content ships with a mod. Dependents call these from
/// their own <c>EditData</c> for their own races and subraces.
/// </summary>
public static class RaceRecords
{
    /// <summary>
    /// The game's RACE the races are copied from (Greenlander: the human stats, animations, voices and limits), and its name as a fallback.
    /// </summary>
    public const string Greenlander = "17-gamedata.quack", GreenlanderName = "Greenlander";

    /// <summary>
    /// The game's CHARACTER a race's character is copied from (Drifter: a plain wanderer), and its name as a fallback.
    /// </summary>
    public const string Drifter = "2205-gamedata.base", DrifterName = "Drifter";

    /// <summary>
    /// The character editor's limits for fitted bodies (docs/kenshi-races.md "Character editor"): the human body sliders, no face sliders.
    /// Dependents' subraces may use it too.
    /// </summary>
    public const string EditorLimits = $@".\mods\{Races.Folder}\items\races\kenshiraces_editor.xml";

    /// <summary>
    /// The CHARACTER's lists emptied: no kit, no money to spend.
    /// </summary>
    static readonly string[] KitLists = ["backpack", "clothing", "inventory", "shopping", "weapons"];

    /// <summary>
    /// Adds <paramref name="group"/>'s RACE_GROUP and each of its subraces (<see cref="Add"/>).
    /// </summary>
    public static void AddGroup(ModDataContext c, RaceGroup group, string? talk = Races.Recruit)
    {
        var record = c.Add(group.Id, FcsRecordType.RaceGroup, group.Name);
        record.References["races"] = [.. group.Subraces.Select(s => new FcsReference(s.RaceId, 0, 0, 0))];
        foreach (var subrace in group.Subraces) Add(c, subrace, talk);
    }

    /// <summary>
    /// Adds <paramref name="subrace"/>'s RACE and its CHARACTERs to the data, each CHARACTER with <paramref name="talk"/> as its dialogue
    /// (default: Kenshi Races' recruit talk; null for none). The RACE is listed by the editor only once a RACE_GROUP holds it
    /// (<see cref="AddGroup"/>, <see cref="JoinGroup"/>). Throws when the game lacks Greenlander or Drifter.
    /// </summary>
    public static void Add(ModDataContext c, Subrace subrace, string? talk = Races.Recruit)
    {
        FillRace(c.Add(subrace.RaceId, FcsRecordType.Race, subrace.Name), c.Data, subrace);
        foreach (var character in subrace.Characters)
            FillCharacter(c.Add(character.Id, FcsRecordType.Character, character.Name), c.Data, subrace, character, talk);
    }

    /// <summary>
    /// Adds a RACE (another mod's subrace, e.g. a ghost crab) to a race's RACE_GROUP, e.g. <see cref="Races.CrabfolkGroup"/>, so the editor lists
    /// it as one of that race's subraces (an edit adds the entry to the group's list). The group must be in the data: Kenshi Races' groups are,
    /// for a dependent's data phase.
    /// </summary>
    public static void JoinGroup(ModDataContext c, string groupId, string raceId)
    {
        c.Edit(groupId).References["races"] = [new FcsReference(raceId, 0, 0, 0)];
    }

    /// <summary>
    /// Fills a new RACE: Greenlander's fields and lists, then each gender's body files (a single-gender race wears its one body for both), no
    /// heads or hair (the body has its own), no body morphs, no shirts (they paint the human body's uv layout), playable in the character
    /// editor with <see cref="EditorLimits"/>, and the animation library linked (<see cref="AnimationRecords"/>).
    /// </summary>
    public static void FillRace(FcsRecord race, GameDatabase game, Subrace subrace)
    {
        CopyFrom(Model(game, FcsRecordType.Race, Greenlander, GreenlanderName), race);
        race.Bools["single gender"] = subrace.SingleGender;
        race.Bools["no shirts"] = true;
        race.Bools["playable"] = true;
        race.Filenames["editor limits"] = EditorLimits;
        race.Ints["morph num"] = 0;
        SetBody(race, "male", subrace.Body(female: false));
        SetBody(race, "female", subrace.Body(female: true));

        race.References["hairs"] = [];
        AnimationRecords.Link(race);
    }

    /// <summary>
    /// Fills a new CHARACTER: Drifter's fields and lists, of the character's gender, no kit, money or bounty, of the subrace, with
    /// <paramref name="talk"/> as its dialogue when given.
    /// </summary>
    public static void FillCharacter(FcsRecord record, GameDatabase game, Subrace subrace, RaceCharacter character, string? talk = Races.Recruit)
    {
        CopyFrom(Model(game, FcsRecordType.Character, Drifter, DrifterName), record);
        record.Ints["female chance"] = character.Female && !subrace.SingleGender ? 100 : 0;
        record.Ints["money min"] = 0;
        record.Ints["money max"] = 0;
        record.Ints["bounty chance"] = 0;
        foreach (string list in KitLists) record.References[list] = [];

        record.References["race"] = [new FcsReference(subrace.RaceId, 100, 0, 0)];
        record.References["dialogue"] = talk is null ? [] : [new FcsReference(talk, 0, 0, 0)];
    }

    /// <summary>
    /// One gender's body files: the mesh, colour texture, normal map and part map; the human body mask and strength normal maps cleared.
    /// </summary>
    static void SetBody(FcsRecord race, string gender, RaceBody body)
    {
        race.Filenames[$"{gender} mesh"] = body.Mesh;
        race.Filenames[$"body texture {gender}"] = body.Texture;
        race.Filenames[$"nm {gender}"] = body.Normals;
        race.Filenames[$"nm {gender} skinny"] = "";
        race.Filenames[$"nm {gender} strong"] = "";
        race.Filenames[$"body mask {gender}"] = "";
        race.Filenames[$"part map {gender}"] = body.PartMap;
        race.References[$"heads {gender}"] = [];
    }

    /// <summary>
    /// The game's record copied from: by id, else the lowest-id record of that type and name. Throws when there is neither.
    /// </summary>
    static GameRecord Model(GameDatabase game, FcsRecordType type, string id, string name) =>
        game.Find(id)
            ?? game.OfType(type).Where(r => r.Name == name).OrderBy(r => r.StringId, StringComparer.Ordinal).FirstOrDefault()
            ?? throw new InvalidOperationException($"The game data has no {type} '{name}' ({id}) to copy.");

    /// <summary>
    /// Copies every field and reference list of a loaded record into a new one.
    /// </summary>
    static void CopyFrom(GameRecord from, FcsRecord to)
    {
        foreach (var (key, value) in from.Bools) to.Bools[key] = value;
        foreach (var (key, value) in from.Floats) to.Floats[key] = value;
        foreach (var (key, value) in from.Ints) to.Ints[key] = value;
        foreach (var (key, value) in from.Vector3S) to.Vector3S[key] = value;
        foreach (var (key, value) in from.Vector4S) to.Vector4S[key] = value;
        foreach (var (key, value) in from.Strings) to.Strings[key] = value;
        foreach (var (key, value) in from.Filenames) to.Filenames[key] = value;
        foreach (string list in from.ReferenceLists) to.References[list] = [.. from.GetReferences(list).Select(Link)];
    }

    /// <summary>
    /// A loaded link as a data file's.
    /// </summary>
    static FcsReference Link(GameReference link) => new(link.TargetStringId, link.Values.Value0, link.Values.Value1, link.Values.Value2);
}
