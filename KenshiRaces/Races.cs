using Meitou.Data.Gameplay.Bodies;

namespace KenshiRaces;

/// <summary>
/// The races Kenshi Races adds (docs/kenshi-races.md "Races"): three races of the character editor, Leoma, Crevan and Crabfolk, with their
/// subraces. Dependents use the ids in their own records (a CHARACTER's <c>race</c>, a squad's members) and add subraces with
/// <see cref="RaceRecords.JoinGroup"/>. Subrace and character names are one lowercase word, so the console's <c>spawn</c> takes them unquoted.
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
    /// The RACE ids (one per subrace).
    /// </summary>
    public const string LeomaRace = "10-" + FileName, CrabmanRace = "12-" + FileName, CrabQueenRace = "14-" + FileName,
        CrevanRace = "18-" + FileName;

    /// <summary>
    /// The CHARACTER ids.
    /// </summary>
    public const string LeomaMaleCharacter = "11-" + FileName, CrabmanCharacter = "13-" + FileName, CrabQueenCharacter = "15-" + FileName,
        LeomaFemaleCharacter = "17-" + FileName, CrevanMaleCharacter = "19-" + FileName, CrevanFemaleCharacter = "21-" + FileName;

    /// <summary>
    /// The RACE_GROUP ids: the races the character editor lists.
    /// </summary>
    public const string LeomaGroup = "30-" + FileName, CrevanGroup = "31-" + FileName, CrabfolkGroup = "32-" + FileName;

    /// <summary>
    /// The WEAPON the Crabfolk punch with (<see cref="RaceFists"/>).
    /// </summary>
    public const string CrabFists = "50-" + FileName;

    /// <summary>
    /// Strong, sword-loving lions: tougher all over, bad at armour, farming, polearms, heavy weapons and robotics.
    /// </summary>
    public static readonly RaceTraits LeomaTraits = new(
        [StatsEnumerated.Strength, StatsEnumerated.Sabres, StatsEnumerated.Athletics, StatsEnumerated.SmithingWeapon, StatsEnumerated.Labouring],
        [StatsEnumerated.SmithingArmour, StatsEnumerated.Farming, StatsEnumerated.Polearms, StatsEnumerated.HeavyWeapons, StatsEnumerated.Robotics])
    {
        Health = PartHealth.AllBut(head: 100, rest: 120),
    };

    /// <summary>
    /// Sharp-eyed, light-fingered crows: frail limbs, a hard skull, no footwear (talons), and of head gear only the Ashland Hat and its goggles.
    /// </summary>
    public static readonly RaceTraits CrevanTraits = new(
        [
            StatsEnumerated.Perception, StatsEnumerated.Stealth, StatsEnumerated.Lockpicking, StatsEnumerated.Thieving, StatsEnumerated.Assassination,
            StatsEnumerated.Robotics, StatsEnumerated.Dodge,
        ],
        [
            StatsEnumerated.Katanas, StatsEnumerated.Sabres, StatsEnumerated.Hackers, StatsEnumerated.HeavyWeapons, StatsEnumerated.Blunt,
            StatsEnumerated.Polearms, StatsEnumerated.Farming, StatsEnumerated.Labouring,
        ])
    {
        Health = new PartHealth(Head: 140, Chest: 100, Stomach: 100, Arms: 90, Legs: 90),
        NoShoes = true,
        HatsOnly = ["Ashland"],
    };

    /// <summary>
    /// Shelled brawlers: martial arts only (no weapons or crossbows), no helmets or sandals, heavy claws, a shell that turns a fifth of each blow.
    /// </summary>
    public static readonly RaceTraits CrabfolkTraits = new(
        [
            StatsEnumerated.MartialArts, StatsEnumerated.Strength, StatsEnumerated.Toughness, StatsEnumerated.MeleeAttack, StatsEnumerated.MeleeDefence,
            StatsEnumerated.Swimming, StatsEnumerated.Labouring, StatsEnumerated.Engineering,
        ],
        [
            StatsEnumerated.Dodge, StatsEnumerated.Turrets, StatsEnumerated.Crossbows, StatsEnumerated.FriendlyFire, StatsEnumerated.Stealth,
            StatsEnumerated.Lockpicking, StatsEnumerated.Thieving, StatsEnumerated.Assassination, StatsEnumerated.Medic, StatsEnumerated.SmithingWeapon,
            StatsEnumerated.SmithingArmour, StatsEnumerated.SmithingBow,
        ])
    {
        Health = new PartHealth(Head: 100, Chest: 200, Stomach: 200, Arms: 250, Legs: 120),
        NoHats = true,
        NoFootwear = ["Sandals"],
        NoWeapons = true,
        Fists = CrabFists,
        DamageTaken = 0.8f,
    };

    /// <summary>
    /// Lion people with manes and tails.
    /// </summary>
    public static readonly Subrace Leoma = new("leoma", LeomaRace, Body("leomamale", true), Body("leomafemale", true),
        [new("leomamale", LeomaMaleCharacter, false), new("leomafemale", LeomaFemaleCharacter, true)], LeomaTraits);

    /// <summary>
    /// Feathered crow people.
    /// </summary>
    public static readonly Subrace Crevan = new("crevan", CrevanRace, Body("crevanmale", true), Body("crevanfemale", true),
        [new("crevanmale", CrevanMaleCharacter, false), new("crevanfemale", CrevanFemaleCharacter, true)], CrevanTraits);

    /// <summary>
    /// Crab-shelled men.
    /// </summary>
    public static readonly Subrace Crabman = new("crabman", CrabmanRace, Body("crabman", false), null, [new("crabman", CrabmanCharacter, false)],
        CrabfolkTraits);

    /// <summary>
    /// The crab people's queens (fitted to the male skeleton, so single gender as the crabmen).
    /// </summary>
    public static readonly Subrace CrabQueen = new("crabqueen", CrabQueenRace, Body("crabqueen", false), null,
        [new("crabqueen", CrabQueenCharacter, false)], CrabfolkTraits);

    /// <summary>
    /// The character editor's races.
    /// </summary>
    public static readonly RaceGroup LeomaRaces = new("Leoma", LeomaGroup, [Leoma]), CrevanRaces = new("Crevan", CrevanGroup, [Crevan]),
        Crabfolk = new("Crabfolk", CrabfolkGroup, [Crabman, CrabQueen]);

    /// <summary>
    /// Every race of the mod, in the editor's order.
    /// </summary>
    public static IReadOnlyList<RaceGroup> Groups { get; } = [LeomaRaces, CrevanRaces, Crabfolk];

    /// <summary>
    /// Every subrace of the mod.
    /// </summary>
    public static IReadOnlyList<Subrace> All { get; } = [.. Groups.SelectMany(g => g.Subraces)];

    /// <summary>
    /// One of the mod's bodies: its files are <c>kenshiraces_&lt;name&gt;*</c> in the mod's folder.
    /// </summary>
    static RaceBody Body(string name, bool normalMap) => new(Folder, "kenshiraces_" + name, normalMap);
}
