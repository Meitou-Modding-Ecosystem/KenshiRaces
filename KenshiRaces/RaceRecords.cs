using Meitou.Data;
using Meitou.Data.Fcs;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// Builds a fitted race's records in the data phase (docs/kenshi-races.md "Records"): a RACE copied from the game's Greenlander with the fitted
/// body's files, and a CHARACTER copied from the game's Drifter, of that race, with no kit and the recruit talk. Copies of the loaded game's own
/// records, so no game content ships with a mod. Dependents call <see cref="Add"/> from their own <c>EditData</c> for their own races.
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
    /// The CHARACTER's lists emptied: no kit, no money to spend.
    /// </summary>
    static readonly string[] KitLists = ["backpack", "clothing", "inventory", "shopping", "weapons"];

    /// <summary>
    /// Adds <paramref name="body"/>'s RACE and (when it names one) its CHARACTER to the data, the CHARACTER with <paramref name="talk"/> as its
    /// dialogue (default: Kenshi Races' recruit talk; null for none). Throws when the game lacks Greenlander or Drifter.
    /// </summary>
    public static void Add(ModDataContext c, RaceBody body, string? talk = Races.Recruit)
    {
        FillRace(c.Add(body.RaceId, FcsRecordType.Race, body.Name), c.Data, body);
        if (body.CharacterId is { } character) FillCharacter(c.Add(character, FcsRecordType.Character, body.Name), c.Data, body, talk);
    }

    /// <summary>
    /// Fills a new RACE: Greenlander's fields and lists, then the body's mesh, textures and part map for both genders, no heads or hair (the body
    /// has its own), no body morphs, no shirts (they paint the human body's uv layout) and not offered at character creation.
    /// </summary>
    public static void FillRace(FcsRecord race, GameDatabase game, RaceBody body)
    {
        CopyFrom(Model(game, FcsRecordType.Race, Greenlander, GreenlanderName), race);
        race.Bools["single gender"] = true;
        race.Bools["no shirts"] = true;
        race.Bools["playable"] = false;
        race.Ints["morph num"] = 0;
        foreach (string gender in new[] { "male", "female" }) SetBody(race, gender, body);

        race.References["hairs"] = [];
    }

    /// <summary>
    /// Fills a new CHARACTER: Drifter's fields and lists, always male (the bodies are fitted to the male skeleton), no kit, money or bounty, of
    /// the race, with <paramref name="talk"/> as its dialogue when given.
    /// </summary>
    public static void FillCharacter(FcsRecord character, GameDatabase game, RaceBody body, string? talk = Races.Recruit)
    {
        CopyFrom(Model(game, FcsRecordType.Character, Drifter, DrifterName), character);
        character.Ints["female chance"] = 0;
        character.Ints["money min"] = 0;
        character.Ints["money max"] = 0;
        character.Ints["bounty chance"] = 0;
        foreach (string list in KitLists) character.References[list] = [];

        character.References["race"] = [new FcsReference(body.RaceId, 100, 0, 0)];
        character.References["dialogue"] = talk is null ? [] : [new FcsReference(talk, 0, 0, 0)];
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
