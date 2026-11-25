using System.Numerics;
using Meitou.Data.World;
using Meitou.Simulation;
using Meitou.Simulation.Factions;
using Meitou.Simulation.Population;

namespace KenshiRaces.Introductions;

/// <summary>
/// Keeps the introducer (<see cref="IntroducerData"/>) in the bar of Black Scratch, the Tech Hunters' town: once the town's zone is loaded the mod
/// adds his platoon at the bar and holds it in its name (saved, so he is added once), walks him back to his spot whenever he stands idle away
/// from it, and puts him back when he is lost far from it. Serial steps only.
/// </summary>
sealed class Introducer(PopulationSystem population, string modId, Action<string> log)
{
    /// <summary>
    /// Black Scratch's TOWN record and the SQUAD_TEMPLATE of its bar's residents ("Adventurer Waystation Bar"), whose home is the bar.
    /// </summary>
    public const string Town = "55656-rebirth.mod", BarResidents = "44856-rebirth.mod";

    /// <summary>
    /// The tag that finds his platoon again after a load (<see cref="Platoon.ModTags"/>).
    /// </summary>
    public const string Tag = "introducer";

    /// <summary>
    /// How near his spot counts as there, and how far from it he is put straight back instead of walked, in game units.
    /// </summary>
    const float SpotReach = 20, LostReach = 150;

    /// <summary>
    /// How far round his spot his platoon may be added.
    /// </summary>
    const float SpawnSpread = 10;

    /// <summary>
    /// Game hours between tries to place him when a try fails.
    /// </summary>
    const double RetryHours = 1;

    /// <summary>
    /// His spot in the bar, found once while the town is loaded.
    /// </summary>
    Vector3? _spot;

    /// <summary>
    /// Black Scratch's site.
    /// </summary>
    TownSite? _site;

    /// <summary>
    /// When the next try to place him may run, in game hours.
    /// </summary>
    double _nextTry;

    /// <summary>
    /// Whether a failed try was logged (once).
    /// </summary>
    bool _toldFailed;

    /// <summary>
    /// One check: places him while the bar is loaded and his platoon is not in the world yet, and keeps his loaded self at the bar.
    /// </summary>
    public void Run(World world)
    {
        var platoon = world.Platoons.FindByTag(modId, Tag);
        if (platoon is null && Spot(world) is { } spot) platoon = Spawn(world, spot);
        if (platoon is null || _spot is not { } at) return;

        PopulationSystem.HoldPlatoon(world, platoon.Id, modId, true);
        if (platoon.SquadId < 0 || world.Squads.Find(platoon.SquadId) is not { } squad || Placing.Member(world, squad) is not { } him) return;

        KeepAtBar(world, him, at);
    }

    /// <summary>
    /// His spot: the bar's first general node, else its first bar node, else its origin; null while Black Scratch, its bar or the bar's doors are
    /// not in the world.
    /// </summary>
    Vector3? Spot(World world)
    {
        if (_spot is not null) return _spot;
        if (population.Data.Sites.FirstOrDefault(s => s.Record.Id == Town) is not { } site || Bar(world, site) is not { } bar) return null;
        if (!world.Fixtures.DoorsByBuilding[bar.Key].Any()) return null;

        var node = bar.Of(HomeSpotKind.General).Concat(bar.Of(HomeSpotKind.Bar)).Select(s => (HomeSpot?)s).FirstOrDefault();
        _site = site;
        _spot = node?.Position ?? bar.Position;
        return _spot;
    }

    /// <summary>
    /// The building the bar's residents live in, else the town's first bar.
    /// </summary>
    static TownBuilding? Bar(World world, TownSite site) =>
        site.Buildings.FirstOrDefault(b => world.Fixtures.ResidentsTemplate(b) == BarResidents)
        ?? site.Buildings.FirstOrDefault(b => b.IsBar);

    /// <summary>
    /// Adds his platoon at his spot, tagged; null when that fails, tried again an hour later.
    /// </summary>
    Platoon? Spawn(World world, Vector3 spot)
    {
        if (world.GameHours < _nextTry) return null;

        int faction = population.Data.FactionIndex(IntroducerData.Drifters);
        var around = new Vector2(spot.X, spot.Z);
        int id = faction < 0 ? -1 : population.SpawnPlatoon(world, faction, IntroducerData.Squad, _site!.Index, around, 0, SpawnSpread);
        if (world.Platoons.Find(id) is not { } platoon)
        {
            _nextTry = world.GameHours + RetryHours;
            if (!_toldFailed) log("kenshi races: could not place the introducer in Black Scratch's bar (are the records loaded?); trying hourly");
            _toldFailed = true;
            return null;
        }

        platoon.ModTags[modId] = Tag;
        log($"kenshi races: {IntroducerData.Name} takes his seat in Black Scratch's bar at {spot.X:0}, {spot.Z:0}");
        return platoon;
    }

    /// <summary>
    /// Walks him back to his spot whenever he stands idle away from it, and puts him there when he is far from it out of a fight.
    /// </summary>
    static void KeepAtBar(World world, int him, Vector3 spot)
    {
        var cold = world.Characters.Cold(him)!;
        float away = Placing.Flat(world.Characters.Next[him].Position, spot);
        if (away > LostReach && Placing.Idle(world, him, cold))
        {
            Placing.PutAt(world, him, spot);
            return;
        }

        if (away <= SpotReach || !Placing.Idle(world, him, cold)) return;

        Placing.Walk(world, him, spot);
    }
}
