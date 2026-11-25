using Meitou.Data;
using Meitou.Data.Gameplay.Bodies;
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
    /// The character editor's limits for the races wearing the human head: <see cref="EditorLimits"/> with the human face sliders, the chest and the
    /// bust (docs/kenshi-races.md "Character editor").
    /// </summary>
    public const string FaceEditorLimits = $@".\mods\{Races.Folder}\items\races\kenshiraces_editor_faces.xml";

    /// <summary>
    /// The character editor's limits for the races whose skin takes the skin tone (<see cref="RaceBody.SkinTone"/>): <see cref="FaceEditorLimits"/>
    /// with a skin tone slider from human tones through red and purple to blue-grey (docs/kenshi-races.md "Character editor").
    /// </summary>
    public const string TintedEditorLimits = $@".\mods\{Races.Folder}\items\races\kenshiraces_editor_tinted.xml";

    /// <summary>
    /// The CHARACTER's lists emptied: no kit, no money to spend.
    /// </summary>
    internal static readonly string[] KitLists = ["backpack", "clothing", "inventory", "shopping", "weapons"];

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
    /// heads, hair or random faces (the body has its own head) unless it wears the human head, no shirts (they paint the human body's uv layout),
    /// playable in the character editor with <see cref="EditorLimits"/> (<see cref="FaceEditorLimits"/> with the human head), the animation
    /// library linked (<see cref="AnimationRecords"/>) and the subrace's traits.
    /// </summary>
    public static void FillRace(FcsRecord race, GameDatabase game, Subrace subrace)
    {
        CopyFrom(Model(game, FcsRecordType.Race, Greenlander, GreenlanderName), race);
        race.Bools["single gender"] = subrace.SingleGender;
        race.Bools["no shirts"] = true;
        race.Bools["playable"] = true;
        race.Filenames["editor limits"] = EditorLimitsOf(subrace);
        SetBody(race, "male", subrace.Body(female: false));
        SetBody(race, "female", subrace.Body(female: true));

        if (!subrace.HumanHead) DropHead(race);
        AnimationRecords.Link(race);
        if (subrace.Traits is { } traits) SetTraits(race, game, traits);
    }

    /// <summary>
    /// The editor limits a subrace's RACE uses: tinted with a skin tone, with faces with the human head, else the fitted bodies'.
    /// </summary>
    static string EditorLimitsOf(Subrace subrace) =>
        (subrace.Male ?? subrace.Female)?.SkinTone == true ? TintedEditorLimits : subrace.HumanHead ? FaceEditorLimits : EditorLimits;

    /// <summary>
    /// The RACE fields <see cref="RaceRules"/> reads back: head gear allowed by name, footwear refused by name, no weapons, damage taken.
    /// </summary>
    public const string HatsOnlyField = "kenshi races hats only", NoFootwearField = "kenshi races no footwear",
        NoWeaponsField = "kenshi races no weapons", DamageTakenField = "kenshi races damage taken";

    /// <summary>
    /// Writes <paramref name="traits"/> into a new RACE (docs/kenshi-races.md "Traits"): the aptitude lists (replacing the Greenlander's), part
    /// health, the slot flags, the fists and the fields <see cref="RaceRules"/> reads.
    /// </summary>
    static void SetTraits(FcsRecord race, GameDatabase game, RaceTraits traits)
    {
        SetAptitudes(race, traits);
        if (traits.Health is { } health) SetHealth(race, game, health);
        race.Bools["no hats"] = traits.NoHats;
        race.Bools["no shoes"] = traits.NoShoes;
        if (traits.Fists is { } fists)
        {
            race.References["fists lv1"] = [new FcsReference(fists, 0, 0, 0)];
            race.References["fists lv100"] = [new FcsReference(fists, 0, 0, 0)];
        }

        race.Strings[HatsOnlyField] = string.Join('|', traits.HatsOnly ?? []);
        race.Strings[NoFootwearField] = string.Join('|', traits.NoFootwear ?? []);
        race.Bools[NoWeaponsField] = traits.NoWeapons;
        race.Floats[DamageTakenField] = traits.DamageTaken;
    }

    /// <summary>
    /// The RACE's <c>stats good&lt;N&gt;</c> and <c>stats bad&lt;N&gt;</c> (x1.2 and x0.8), and <c>strength</c> for the strength entry, which the game
    /// lays over the lists (engine docs/game/character-stats.md "Races").
    /// </summary>
    static void SetAptitudes(FcsRecord race, RaceTraits traits)
    {
        foreach (string key in race.Ints.Keys.Where(k => k.StartsWith("stats good") || k.StartsWith("stats bad")).ToList()) race.Ints.Remove(key);

        var good = traits.Good.Where(s => s != StatsEnumerated.Strength).ToList();
        var bad = traits.Bad.Where(s => s != StatsEnumerated.Strength).ToList();
        for (int i = 0; i < good.Count; i++) race.Ints[$"stats good{i}"] = (int)good[i];
        for (int i = 0; i < bad.Count; i++) race.Ints[$"stats bad{i}"] = (int)bad[i];

        race.Floats["strength"] = traits.Good.Contains(StatsEnumerated.Strength) ? 1.2f
            : traits.Bad.Contains(StatsEnumerated.Strength) ? 0.8f
            : 1;
    }

    /// <summary>
    /// Each <c>combat anatomy</c> entry's value 1 (the part's base hit points) by its LOCATIONAL_DAMAGE record's name; the hit weights stay.
    /// </summary>
    static void SetHealth(FcsRecord race, GameDatabase game, PartHealth health)
    {
        if (!race.References.TryGetValue("combat anatomy", out var parts)) return;

        for (int i = 0; i < parts.Count; i++)
        {
            string name = game.Find(parts[i].TargetStringId)?.Name ?? "";
            parts[i] = parts[i] with { Value1 = HitPoints(name, health, parts[i].Value1) };
        }
    }

    /// <summary>
    /// A body part's hit points by its record's name (Head, Chest, Stomach, Left/Right Arm, Left/Right Leg); another part keeps
    /// <paramref name="current"/>.
    /// </summary>
    static int HitPoints(string part, PartHealth health, int current) => part switch
    {
        "Head" => health.Head,
        "Chest" => health.Chest,
        "Stomach" => health.Stomach,
        _ when part.EndsWith(" Arm") => health.Arms,
        _ when part.EndsWith(" Leg") => health.Legs,
        _ => current,
    };

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
    /// One gender's body files: the mesh, colour texture, normal map, part map and skin-tone mask (the human body mask without one); the strength
    /// normal maps cleared.
    /// </summary>
    static void SetBody(FcsRecord race, string gender, RaceBody body)
    {
        race.Filenames[$"{gender} mesh"] = body.Mesh;
        race.Filenames[$"body texture {gender}"] = body.Texture;
        race.Filenames[$"nm {gender}"] = body.Normals;
        race.Filenames[$"nm {gender} skinny"] = "";
        race.Filenames[$"nm {gender} strong"] = "";
        race.Filenames[$"body mask {gender}"] = body.Mask;
        race.Filenames[$"part map {gender}"] = body.PartMap;
    }

    /// <summary>
    /// No heads (a race listing none draws none), hair or random faces (<c>morph num</c>: a fitted head has no face poses).
    /// </summary>
    static void DropHead(FcsRecord race)
    {
        race.References["heads male"] = [];
        race.References["heads female"] = [];
        race.References["hairs"] = [];
        race.Ints["morph num"] = 0;
    }

    /// <summary>
    /// The game's record copied from: by id, else the lowest-id record of that type and name. Throws when there is neither.
    /// </summary>
    internal static GameRecord Model(GameDatabase game, FcsRecordType type, string id, string name) =>
        game.Find(id)
            ?? game.OfType(type).Where(r => r.Name == name).OrderBy(r => r.StringId, StringComparer.Ordinal).FirstOrDefault()
            ?? throw new InvalidOperationException($"The game data has no {type} '{name}' ({id}) to copy.");

    /// <summary>
    /// Copies every field and reference list of a loaded record into a new one.
    /// </summary>
    internal static void CopyFrom(GameRecord from, FcsRecord to)
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
