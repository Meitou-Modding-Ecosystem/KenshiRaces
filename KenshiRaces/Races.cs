namespace KenshiRaces;

/// <summary>
/// The races Kenshi Races adds (docs/kenshi-races.md "Races"). Dependents use their ids in their own records: a CHARACTER's <c>race</c>, a
/// squad's members.
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
    public const string LeonidRace = "10-" + FileName, LeonidCharacter = "11-" + FileName, CrabmanRace = "12-" + FileName,
        CrabmanCharacter = "13-" + FileName, CrabQueenRace = "14-" + FileName, CrabQueenCharacter = "15-" + FileName;

    /// <summary>
    /// A lion-man with a mane and a tail.
    /// </summary>
    public static readonly RaceBody Leonid = new("Leonid", LeonidRace, LeonidCharacter, Folder, "kenshiraces_leonid", NormalMap: true);

    /// <summary>
    /// A crab-shelled man.
    /// </summary>
    public static readonly RaceBody Crabman = new("Crabman", CrabmanRace, CrabmanCharacter, Folder, "kenshiraces_crabman", NormalMap: false);

    /// <summary>
    /// The crab people's queen.
    /// </summary>
    public static readonly RaceBody CrabQueen = new("Crab Queen", CrabQueenRace, CrabQueenCharacter, Folder, "kenshiraces_crabqueen", NormalMap: false);

    /// <summary>
    /// Every race of the mod, in id order.
    /// </summary>
    public static IReadOnlyList<RaceBody> All { get; } = [Leonid, Crabman, CrabQueen];
}
