using Meitou.Data;
using Meitou.Data.Fcs;
using Meitou.Modding;
using Meitou.Simulation;
using Meitou.Simulation.Combat;

namespace KenshiRaces;

/// <summary>
/// The race traits the game's RACE fields can't hold (docs/kenshi-races.md "Traits"), read back from the fields <see cref="RaceRecords.FillRace"/>
/// writes, for every loaded race (a dependent's too): gear a race may not wear or hold (engine docs/modding.md "Wear rules") and the share of a
/// blow's damage that reaches its body ("Hits before they land").
/// </summary>
public static class RaceRules
{
    /// <summary>
    /// The FCS slots of head gear and footwear.
    /// </summary>
    const int HeadSlot = 3, BootsSlot = 9;

    /// <summary>
    /// Registers the wear rule and, when a race takes less damage, the hit filter.
    /// </summary>
    public static void Register(ModContext c)
    {
        var races = c.Records((int)FcsRecordType.Race).ToList();
        var refused = Refused(c, races);
        if (refused.Count > 0) c.AddWearRule((item, race, _) => !(refused.TryGetValue(race, out var items) && items.Contains(item)));

        var damage = races
            .Where(r => r.Floats.TryGetValue(RaceRecords.DamageTakenField, out float share) && share != 1)
            .ToDictionary(r => r.StringId, r => r.Floats[RaceRecords.DamageTakenField], StringComparer.Ordinal);
        if (damage.Count > 0) c.AddSystem("damage taken", _ => new DamageTaken(damage), SystemAnchor.End);
    }

    /// <summary>
    /// Per race with gear limits, the item ids it may not wear or hold.
    /// </summary>
    static Dictionary<string, HashSet<string>> Refused(ModContext c, List<GameRecord> races)
    {
        var armour = c.Records((int)FcsRecordType.Armour).ToList();
        var weapons = c.Records((int)FcsRecordType.Weapon)
            .Concat(c.Records((int)FcsRecordType.Crossbow))
            .Select(r => r.StringId)
            .ToList();

        var result = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        foreach (var race in races)
        {
            var items = RefusedBy(race, armour, weapons);
            if (items.Count > 0) result[race.StringId] = items;
        }

        return result;
    }

    /// <summary>
    /// The items one race refuses: head gear not named by its hats-only list, footwear named in its no-footwear list, and every weapon and
    /// crossbow when it holds none.
    /// </summary>
    internal static HashSet<string> RefusedBy(GameRecord race, List<GameRecord> armour, List<string> weapons)
    {
        var hatsOnly = Names(race, RaceRecords.HatsOnlyField);
        var noFootwear = Names(race, RaceRecords.NoFootwearField);
        var items = new HashSet<string>(StringComparer.Ordinal);

        foreach (var item in armour)
        {
            int slot = item.Ints.GetValueOrDefault("slot");
            bool hat = slot == HeadSlot && hatsOnly.Length > 0 && !hatsOnly.Any(n => item.Name.StartsWith(n, StringComparison.OrdinalIgnoreCase));
            bool shoe = slot == BootsSlot && noFootwear.Any(n => item.Name.Contains(n, StringComparison.OrdinalIgnoreCase));
            if (hat || shoe) items.Add(item.StringId);
        }

        if (race.Bools.GetValueOrDefault(RaceRecords.NoWeaponsField)) items.UnionWith(weapons);
        return items;
    }

    /// <summary>
    /// A '|'-separated name list field, empty when absent.
    /// </summary>
    static string[] Names(GameRecord race, string field) =>
        race.Strings.GetValueOrDefault(field, "").Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

/// <summary>
/// Scales each blow that lands on a character of a listed race (melee, bolt or execution, before armour and toughness) by that race's share.
/// </summary>
sealed class DamageTaken(Dictionary<string, float> shares) : ICombatHitFilter
{
    public void HitLanding(World world, ref PendingHit hit)
    {
        if (!world.Characters.TryResolveNext(hit.Victim, out int slot)) return;
        if (world.Characters.Cold(slot)?.Race?.StringId is not { } race || !shares.TryGetValue(race, out float share)) return;

        var p = hit.Packet;
        hit.Packet = p with { Cut = p.Cut * share, Blunt = p.Blunt * share, Pierce = p.Pierce * share, Stun = p.Stun * share };
    }
}
