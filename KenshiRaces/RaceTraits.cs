using Meitou.Data.Gameplay.Bodies;

namespace KenshiRaces;

/// <summary>
/// What sets a subrace apart from the Greenlander it is copied from (docs/kenshi-races.md "Traits"): aptitudes, body part health, gear limits and
/// fists, written into its RACE record by <see cref="RaceRecords.FillRace"/>. The gear limits and <see cref="DamageTaken"/> are kept as fields of
/// the RACE (<see cref="RaceRules"/> reads them back), so a dependent's subrace gets them too.
/// </summary>
/// <param name="Good">Stats learned 20% faster (and started 20% higher): the RACE's <c>stats good</c>. <see cref="StatsEnumerated.Strength"/> sets
/// <c>strength</c> 1.2 instead, which the game lays over the list.</param>
/// <param name="Bad">Stats learned 20% slower: the RACE's <c>stats bad</c> (<see cref="StatsEnumerated.Strength"/>: <c>strength</c> 0.8).</param>
public sealed record RaceTraits(IReadOnlyList<StatsEnumerated> Good, IReadOnlyList<StatsEnumerated> Bad)
{
    /// <summary>
    /// Base hit points of each body part (the RACE's <c>combat anatomy</c> value 1), null for the Greenlander's 100.
    /// </summary>
    public PartHealth? Health { get; init; }

    /// <summary>
    /// The RACE's <c>no hats</c>: the head slot is closed.
    /// </summary>
    public bool NoHats { get; init; }

    /// <summary>
    /// The RACE's <c>no shoes</c>: the boots slot is closed.
    /// </summary>
    public bool NoShoes { get; init; }

    /// <summary>
    /// When set, the only head gear the race wears: items whose name starts with one of these (e.g. <c>Ashland</c>: the Ashland Hat and the
    /// Ashlander Stormgoggles).
    /// </summary>
    public IReadOnlyList<string>? HatsOnly { get; init; }

    /// <summary>
    /// Footwear the race doesn't wear: items in the boots slot whose name contains one of these (e.g. <c>Sandals</c>).
    /// </summary>
    public IReadOnlyList<string>? NoFootwear { get; init; }

    /// <summary>
    /// Whether the race holds no weapons or crossbows: it fights with its fists only.
    /// </summary>
    public bool NoWeapons { get; init; }

    /// <summary>
    /// The WEAPON record the race punches with (the RACE's <c>fists lv1</c> and <c>fists lv100</c>), null for the game's <c>FISTS</c>.
    /// </summary>
    public string? Fists { get; init; }

    /// <summary>
    /// The share of a blow's damage that reaches the race's body (before armour): 1 takes it all, 0.8 a fifth less.
    /// </summary>
    public float DamageTaken { get; init; } = 1;
}

/// <summary>
/// Base hit points per body part.
/// </summary>
public sealed record PartHealth(int Head, int Chest, int Stomach, int Arms, int Legs)
{
    /// <summary>
    /// The same hit points for every part but the head.
    /// </summary>
    public static PartHealth AllBut(int head, int rest) => new(head, rest, rest, rest, rest);
}
