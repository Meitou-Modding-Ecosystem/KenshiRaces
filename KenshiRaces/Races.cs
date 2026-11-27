using Meitou.Data.Gameplay.Bodies;

namespace KenshiRaces;

/// <summary>
/// The races Kenshi Races adds (docs/kenshi-races.md "Races"): eight races of the character editor, Leoma, Crevan, Crabfolk, Tiefling, Batfolk,
/// Wolfkin, Beepbot and Owlkind, with their subraces. Dependents use the ids in their own records (a CHARACTER's <c>race</c>, a squad's members) and add subraces with
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
        CrevanRace = "18-" + FileName, SpidercrabRace = "22-" + FileName, GhostcrabRace = "24-" + FileName, TieflingRace = "26-" + FileName,
        BatfolkRace = "28-" + FileName, WolfkinRace = "36-" + FileName, BeepbotRace = "16-" + FileName, OwlkindRace = "43-" + FileName,
        KatlonRace = "47-" + FileName;

    /// <summary>
    /// The CHARACTER ids.
    /// </summary>
    public const string LeomaMaleCharacter = "11-" + FileName, CrabmanCharacter = "13-" + FileName, CrabQueenCharacter = "15-" + FileName,
        LeomaFemaleCharacter = "17-" + FileName, CrevanMaleCharacter = "19-" + FileName, CrevanFemaleCharacter = "21-" + FileName,
        SpidercrabCharacter = "23-" + FileName, GhostcrabCharacter = "25-" + FileName, TieflingFemaleCharacter = "27-" + FileName,
        TieflingMaleCharacter = "42-" + FileName,
        BatfolkMaleCharacter = "29-" + FileName, BatfolkFemaleCharacter = "35-" + FileName,
        WolfkinMaleCharacter = "37-" + FileName, WolfkinFemaleCharacter = "38-" + FileName, BeepbotCharacter = "20-" + FileName,
        OwlkindFemaleCharacter = "44-" + FileName, OwlkindMaleCharacter = "46-" + FileName, KatlonCharacter = "48-" + FileName;

    /// <summary>
    /// The RACE_GROUP ids: the races the character editor lists.
    /// </summary>
    public const string LeomaGroup = "30-" + FileName, CrevanGroup = "31-" + FileName, CrabfolkGroup = "32-" + FileName, TieflingGroup = "33-" + FileName,
        BatfolkGroup = "34-" + FileName, WolfkinGroup = "39-" + FileName, BeepbotGroup = "41-" + FileName, OwlkindGroup = "45-" + FileName;

    /// <summary>
    /// The WEAPON the Crabfolk punch with (<see cref="RaceFists"/>).
    /// </summary>
    public const string CrabFists = "50-" + FileName;

    /// <summary>
    /// A Skeleton's mechanics (no hunger, robot health and repairs, the Skeleton's aptitudes and fists), but every hat and no boots.
    /// </summary>
    public static readonly RaceTraits BeepbotTraits = new([], [])
    {
        Model = RaceModel.Skeleton,
        NoHats = false,
        NoShoes = true,
    };

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
    /// The spider crabs (male fit, single gender), with the Crabfolk's traits until their own are settled.
    /// </summary>
    public static readonly Subrace Spidercrab = new("spidercrab", SpidercrabRace, Body("spidercrab", true), null,
        [new("spidercrab", SpidercrabCharacter, false)], CrabfolkTraits);

    /// <summary>
    /// The ghost crabs (male fit, single gender), with the Crabfolk's traits until their own are settled.
    /// </summary>
    public static readonly Subrace Ghostcrab = new("ghostcrab", GhostcrabRace, Body("ghostcrab", true), null,
        [new("ghostcrab", GhostcrabCharacter, false)], CrabfolkTraits);

    /// <summary>
    /// The tieflings: a human head and face (the Greenlander's heads, hair and face sliders) with ram horns, on horned-tailed bodies whose skin
    /// takes the editor's skin tone with the head (<see cref="RaceBody.SkinTone"/>); a Greenlander's traits.
    /// </summary>
    public static readonly Subrace Tiefling = new("tiefling", TieflingRace, TonedBody("tieflingmale", true), TonedBody("tieflingfemale", false),
        [new("tieflingmale", TieflingMaleCharacter, false), new("tieflingfemale", TieflingFemaleCharacter, true)]);

    /// <summary>
    /// Bat people: a human head and face (the Greenlander's heads, hair and face sliders) on a furred body with wing membranes along the arms,
    /// folded behind them; a Greenlander's traits.
    /// </summary>
    public static readonly Subrace Batfolk = new("batfolk", BatfolkRace, HeadedBody("batfolkmale", true), HeadedBody("batfolkfemale", false),
        [new("batfolkmale", BatfolkMaleCharacter, false), new("batfolkfemale", BatfolkFemaleCharacter, true)]);

    /// <summary>
    /// Wolf people: a human head and face (the Greenlander's heads, hair and face sliders) with the model's wolf ears on the skull, on a furred
    /// body; a Greenlander's traits.
    /// </summary>
    public static readonly Subrace Wolfkin = new("wolfkin", WolfkinRace, HeadedBody("wolfkinmale", true), HeadedBody("wolfkinfemale", true),
        [new("wolfkinmale", WolfkinMaleCharacter, false), new("wolfkinfemale", WolfkinFemaleCharacter, true)]);

    /// <summary>
    /// Scrap-built skeletal robots in a rag skirt (male fit, single gender), imported <c>rigid</c> so their ribs and joints keep their shape;
    /// a skeleton subrace (<see cref="BeepbotTraits"/>).
    /// </summary>
    public static readonly Subrace Beepbot = new("beepbot", BeepbotRace, Body("beepbot", true), null, [new("beepbot", BeepbotCharacter, false)],
        BeepbotTraits);

    /// <summary>
    /// Kat-Lon: skeletal robots with a broad, swept-back helmet head (male fit, single gender), imported <c>rigid</c> like the beepbot; a
    /// skeleton subrace (<see cref="BeepbotTraits"/>).
    /// </summary>
    public static readonly Subrace Katlon = new("katlon", KatlonRace, Body("katlon", true), null, [new("katlon", KatlonCharacter, false)],
        BeepbotTraits);

    /// <summary>
    /// Owl people: their own feathered, beaked heads and taloned hands and feet, imported <c>palms</c> so their hands hang palm to thigh; a
    /// Greenlander's traits.
    /// </summary>
    public static readonly Subrace Owlkind = new("owlkind", OwlkindRace, Body("owlkindmale", true), Body("owlkindfemale", true),
        [new("owlkindmale", OwlkindMaleCharacter, false), new("owlkindfemale", OwlkindFemaleCharacter, true)]);

    /// <summary>
    /// The character editor's races.
    /// </summary>
    public static readonly RaceGroup LeomaRaces = new("Leoma", LeomaGroup, [Leoma]), CrevanRaces = new("Crevan", CrevanGroup, [Crevan]),
        Crabfolk = new("Crabfolk", CrabfolkGroup, [Crabman, CrabQueen, Spidercrab, Ghostcrab]),
        TieflingRaces = new("Tiefling", TieflingGroup, [Tiefling]), BatfolkRaces = new("Batfolk", BatfolkGroup, [Batfolk]),
        WolfkinRaces = new("Wolfkin", WolfkinGroup, [Wolfkin]), BeepbotRaces = new("Beepbot", BeepbotGroup, [Beepbot, Katlon]),
        OwlkindRaces = new("Owlkind", OwlkindGroup, [Owlkind]);

    /// <summary>
    /// Every race of the mod, in the editor's order.
    /// </summary>
    public static IReadOnlyList<RaceGroup> Groups { get; } = [LeomaRaces, CrevanRaces, Crabfolk, TieflingRaces, BatfolkRaces, WolfkinRaces,
        BeepbotRaces, OwlkindRaces];

    /// <summary>
    /// Every subrace of the mod.
    /// </summary>
    public static IReadOnlyList<Subrace> All { get; } = [.. Groups.SelectMany(g => g.Subraces)];

    /// <summary>
    /// One of the mod's bodies: its files are <c>kenshiraces_&lt;name&gt;*</c> in the mod's folder.
    /// </summary>
    static RaceBody Body(string name, bool normalMap) => new(Folder, "kenshiraces_" + name, normalMap);

    /// <summary>
    /// One of the mod's bodies wearing the human head (<see cref="RaceBody.HumanHead"/>).
    /// </summary>
    static RaceBody HeadedBody(string name, bool normalMap) => Body(name, normalMap) with { HumanHead = true };

    /// <summary>
    /// One of the mod's bodies wearing the human head whose skin takes the editor's skin tone (<see cref="RaceBody.SkinTone"/>).
    /// </summary>
    static RaceBody TonedBody(string name, bool normalMap) => HeadedBody(name, normalMap) with { SkinTone = true };
}
