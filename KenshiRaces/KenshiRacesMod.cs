using Meitou.Data.Fcs;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// Kenshi Races' entry (docs/kenshi-races.md): adds the recruit talk and every race of <see cref="Races"/> in the data phase, from the loaded
/// game's own records, so the shipped KenshiRaces.mod is empty. Dependents add their own races with <see cref="RaceRecords"/>
/// (docs/depending-on-kenshi-races.md).
/// </summary>
public sealed class KenshiRacesMod : IMod, IModDataPhase
{
    /// <summary>
    /// The mod's manifest id.
    /// </summary>
    public const string Id = "kenshi.races";

    public void EditData(ModDataContext c)
    {
        RecruitTalk.Add(c);
        AnimationRecords.Add(c);
        foreach (var group in Races.Groups) RaceRecords.AddGroup(c, group);
    }

    public void Initialize(ModContext c)
    {
        c.AddDebugCommand("Kenshi Races: races", _ => Describe(c));
        if (c.Data.Find(Races.LeomaRace) is null) c.LogError("Kenshi Races' records are missing: the data phase did not add them (see the log).");
    }

    /// <summary>
    /// Every fitted race loaded (this mod's and dependents'): name, id and mesh, one per line.
    /// </summary>
    static string Describe(ModContext c)
    {
        var lines = c.Records((int)FcsRecordType.Race)
            .Where(r => r.GetPath("male mesh").Contains(@"\items\races\", StringComparison.OrdinalIgnoreCase))
            .Select(r => $"{r.Name} ({r.StringId}): {r.GetPath("male mesh")}")
            .ToList();
        return lines.Count == 0 ? "No fitted races are loaded." : string.Join(Environment.NewLine, lines);
    }
}
