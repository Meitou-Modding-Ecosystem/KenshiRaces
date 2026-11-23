namespace KenshiRaces;

/// <summary>
/// The races Kenshi Races adds (docs/kenshi-races.md "Races"). Dependents use their ids in their own records: a CHARACTER's <c>race</c>, a
/// squad's members. Names are one lowercase word, so the console's <c>spawn</c> takes them unquoted.
/// </summary>
public static class Races
{
    /// <summary>
    /// The mod's data file name and folder; record ids end with the file name.
    /// </summary>
    public const string FileName = "KenshiRaces.mod", Folder = "KenshiRaces";

    /// <summary>
    /// The talk that lets a character of these races join the player's squad (<see cref="RecruitTalk"/>).
    /// </summary>
    public const string Recruit = "1-" + FileName;

    /// <summary>
    /// The record ids: each race's RACE and its CHARACTER.
    /// </summary>
    public const string LeonidMaleRace = "10-" + FileName, LeonidMaleCharacter = "11-" + FileName, CrabmanRace = "12-" + FileName,
        CrabmanCharacter = "13-" + FileName, CrabQueenRace = "14-" + FileName, CrabQueenCharacter = "15-" + FileName,
        LeonidFemaleRace = "16-" + FileName, LeonidFemaleCharacter = "17-" + FileName, CorvidMaleRace = "18-" + FileName,
        CorvidMaleCharacter = "19-" + FileName, CorvidFemaleRace = "20-" + FileName, CorvidFemaleCharacter = "21-" + FileName;

    /// <summary>
    /// A lion-man with a mane and a tail.
    /// </summary>
    public static readonly RaceBody LeonidMale = Race("leonidmale", LeonidMaleRace, LeonidMaleCharacter, normalMap: true);

    /// <summary>
    /// A crab-shelled man.
    /// </summary>
    public static readonly RaceBody Crabman = Race("crabman", CrabmanRace, CrabmanCharacter, normalMap: false);

    /// <summary>
    /// The crab people's queen.
    /// </summary>
    public static readonly RaceBody CrabQueen = Race("crabqueen", CrabQueenRace, CrabQueenCharacter, normalMap: false);

    /// <summary>
    /// A lioness with a tail.
    /// </summary>
    public static readonly RaceBody LeonidFemale = Race("leonidfemale", LeonidFemaleRace, LeonidFemaleCharacter, normalMap: true);

    /// <summary>
    /// A feathered crow-man.
    /// </summary>
    public static readonly RaceBody CorvidMale = Race("corvidmale", CorvidMaleRace, CorvidMaleCharacter, normalMap: true);

    /// <summary>
    /// A feathered crow-woman.
    /// </summary>
    public static readonly RaceBody CorvidFemale = Race("corvidfemale", CorvidFemaleRace, CorvidFemaleCharacter, normalMap: true);

    /// <summary>
    /// Every race of the mod, in id order.
    /// </summary>
    public static IReadOnlyList<RaceBody> All { get; } = [LeonidMale, Crabman, CrabQueen, LeonidFemale, CorvidMale, CorvidFemale];

    /// <summary>
    /// One of the mod's races: its files are <c>kenshiraces_&lt;name&gt;*</c> in the mod's folder.
    /// </summary>
    static RaceBody Race(string name, string race, string character, bool normalMap) =>
        new(name, race, character, Folder, "kenshiraces_" + name, normalMap);
}
