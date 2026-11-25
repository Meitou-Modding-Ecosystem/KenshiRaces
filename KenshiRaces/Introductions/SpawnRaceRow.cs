using System.Numerics;
using Meitou.Modding;
using Meitou.Simulation;

namespace KenshiRaces.Introductions;

/// <summary>
/// The debug menu's spawn of a race character (<see cref="SpawnRaceRow"/>): the character's record number, whether it is hostile, the camera
/// target, and the selected characters (the first player character's squad takes a recruit).
/// </summary>
sealed record SpawnRaceCommand(int Number, bool Hostile, Vector3 At, IReadOnlyList<CharacterId> Selected) : SimCommand;

/// <summary>
/// The F10 row "Kenshi Races: spawn race" (docs/kenshi-races.md "Getting the races"): its field takes a race character's or subrace's name from
/// the docs (<c>crabman</c>, <c>leomafemale</c>, <c>leoma female</c>), then <c>join</c> (the default: into the selected character's squad) or
/// <c>hostile</c> (a Dust Bandit), and spawns it at the camera target.
/// </summary>
static class SpawnRaceRow
{
    /// <summary>
    /// The row's label.
    /// </summary>
    public const string Label = "Kenshi Races: spawn race (name [male|female] [join|hostile])";

    /// <summary>
    /// What the row asks for when its field names no race.
    /// </summary>
    static string Usage => "Type a race, e.g. 'crabman', 'leoma female' or 'tiefling hostile'. Races: "
        + string.Join(", ", Races.All.SelectMany(s => s.Characters).Select(c => c.Name)) + ".";

    /// <summary>
    /// Runs the row: queues the spawn, or says what it needs.
    /// </summary>
    public static string Run(DebugCommandContext d)
    {
        if (Parse(d.Field) is not var (character, hostile)) return Usage;

        d.Enqueue(new SpawnRaceCommand(IntroducerData.Number(character), hostile, d.Target, d.Selected) { Tick = d.Tick });
        return $"Spawning a {IntroducerData.Label(character)}{(hostile ? " (hostile)" : "")} at the camera target.";
    }

    /// <summary>
    /// The race character and whether it is hostile, from the field's words in any order and case: a character's name, or a subrace's name with
    /// <c>male</c> or <c>female</c> (its first character without); null when no word names one.
    /// </summary>
    public static (RaceCharacter Character, bool Hostile)? Parse(string field)
    {
        var words = field.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(w => w.ToLowerInvariant()).ToList();
        bool hostile = words.Contains("hostile") || words.Contains("enemy");
        bool? female = words.Contains("female") ? true : words.Contains("male") ? false : null;
        var all = Races.All.SelectMany(s => s.Characters).ToList();

        if (all.FirstOrDefault(c => words.Contains(c.Name)) is { } named) return (named, hostile);
        if (Races.All.FirstOrDefault(s => words.Contains(s.Name)) is not { } subrace) return null;

        var character = subrace.Characters.FirstOrDefault(c => female is null || c.Female == female) ?? subrace.Characters[0];
        return (character, hostile);
    }
}
