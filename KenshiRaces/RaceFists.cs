using Meitou.Data;
using Meitou.Data.Fcs;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// The Crabfolk's fists (docs/kenshi-races.md "Traits"): a WEAPON copied from the skeletons' <c>FISTS- robo</c> (cut x1.2, bleed 0.2, full damage
/// to robots, against the human <c>FISTS</c>' x1, 0.05 and 0.6) with harder claws, cut x1.3 and blunt x1.1, which a race's <c>fists lv1</c> and
/// <c>fists lv100</c> name (engine docs/game/combat.md, "Fists' L").
/// </summary>
public static class RaceFists
{
    /// <summary>
    /// The skeletons' fists, and the people's as a fallback.
    /// </summary>
    const string RoboFists = "53668-rebirth.mod", Fists = "FISTS";

    /// <summary>
    /// Adds the Crabfolk's fists (<see cref="Races.CrabFists"/>). Throws when the game has neither fists record.
    /// </summary>
    public static void Add(ModDataContext c)
    {
        var model = c.Data.Find(RoboFists) ?? c.Data.Find(Fists) ?? throw new InvalidOperationException("The game data has no FISTS to copy.");
        var fists = c.Add(Races.CrabFists, FcsRecordType.Weapon, "FISTS- crab");
        RaceRecords.CopyFrom(model, fists);

        fists.Floats["cut damage multiplier"] = 1.3f;
        fists.Floats["blunt damage multiplier"] = 1.1f;
        fists.Floats["bleed mult"] = 0.2f;
        fists.Floats["robot damage mult"] = 1;
    }
}
