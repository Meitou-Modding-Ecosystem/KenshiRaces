using System.Numerics;
using System.Runtime.CompilerServices;
using Meitou.Modding;
using Meitou.Simulation;
using Meitou.Simulation.Construction;
using Meitou.Simulation.Factions;
using Meitou.Simulation.Population;

namespace KenshiRaces.Introductions;

/// <summary>
/// The race character the introducer has sent for (docs/kenshi-races.md "Getting the races"): its record number, when it sets out (game hours),
/// and how far it has got.
/// </summary>
sealed class Introduction(int number, double due)
{
    /// <summary>
    /// The race character's record number (<see cref="IntroducerData.Of"/>).
    /// </summary>
    public int Number { get; } = number;

    /// <summary>
    /// When the friend sets out for the player's outpost, in game hours.
    /// </summary>
    public double Due { get; } = due;

    /// <summary>
    /// Whether the friend's platoon is in the world.
    /// </summary>
    public bool Spawned { get; set; }

    /// <summary>
    /// Whether the player was told the friend waits for an outpost.
    /// </summary>
    public bool ToldWaiting { get; set; }

    /// <summary>
    /// Whether the player was told the friend is at the outpost.
    /// </summary>
    public bool ToldArrived { get; set; }
}

/// <summary>
/// Kenshi Races' world system (docs/kenshi-races.md "Getting the races"): keeps the introducer in Black Scratch's bar (<see cref="Introducer"/>),
/// runs the introduction he sells (dialogue effect and condition <see cref="IntroducerData.Introduce"/>: three days after the deal the friend sets
/// out for the player's outpost, waiting until there is one, walks up to it and offers to join with the recruit talk; one at a time), and the
/// debug menu's race spawns (<see cref="SpawnRaceCommand"/>). Serial steps only.
/// </summary>
sealed class IntroductionSystem : ITickSystem, IStateHashed, IModSaveData
{
    /// <summary>
    /// Made for each world, with the world's population (null in a world without one: then it does nothing).
    /// </summary>
    public IntroductionSystem(PopulationSystem? population, string modId, Action<string> log, EditorQueue editor)
    {
        _population = population;
        _editor = editor;
        _modId = modId;
        _introducer = population is null ? null : new Introducer(population, modId, log);
    }

    #region Ticking

    /// <summary>
    /// The system of each running world, for the dialogue handlers (which get the world, not the system).
    /// </summary>
    static readonly ConditionalWeakTable<World, IntroductionSystem> ByWorld = [];

    /// <summary>
    /// Ticks between checks.
    /// </summary>
    const int CheckTicks = 20;

    /// <summary>
    /// The world's population.
    /// </summary>
    readonly PopulationSystem? _population;

    /// <summary>
    /// The mod's id, the owner of its holds and tags.
    /// </summary>
    readonly string _modId;

    /// <summary>
    /// The introducer's keeper.
    /// </summary>
    readonly Introducer? _introducer;

    /// <summary>
    /// The system of a world; null when it has none (Kenshi Races not running in it, or before its first tick).
    /// </summary>
    public static IntroductionSystem? For(World world) => ByWorld.TryGetValue(world, out var system) ? system : null;

    /// <summary>
    /// Binds the system to its world and runs the debug spawns.
    /// </summary>
    public void Inputs(World world, IReadOnlyList<SimCommand> commands)
    {
        ByWorld.AddOrUpdate(world, this);
        foreach (var command in commands.OfType<SpawnRaceCommand>()) Spawn(world, command);
        foreach (var command in commands.OfType<TryRacesCommand>()) TryRaces(world, command);
    }

    /// <summary>
    /// One check now and then: the introducer, then the introduction.
    /// </summary>
    public void SlowWorld(World world)
    {
        if (_population is null || world.Tick % CheckTicks != 0) return;

        _introducer!.Run(world);
        if (Pending is { } introduction) Advance(world, introduction);
    }

    #endregion

    #region Introduction

    /// <summary>
    /// Game hours from the deal until the friend sets out: three days.
    /// </summary>
    public const double DelayHours = 72;

    /// <summary>
    /// The tag that finds the friend's platoon again after a load.
    /// </summary>
    public const string Tag = "introduction";

    /// <summary>
    /// How far from the outpost the friend is added, in game units (it walks the rest).
    /// </summary>
    const float ArriveFrom = 300, ArriveTo = 500;

    /// <summary>
    /// How near the outpost's centre the friend counts as there.
    /// </summary>
    const float OutpostReach = 120;

    /// <summary>
    /// The introduction on its way; null when none.
    /// </summary>
    public Introduction? Pending { get; private set; }

    /// <summary>
    /// The friend while its squad is loaded, to tell a join from a death once it has left the squad (not saved: a load before it leaves just
    /// ends the introduction quietly).
    /// </summary>
    CharacterId _friend = CharacterId.None;

    /// <summary>
    /// When the next try to add the friend may run, in game hours.
    /// </summary>
    double _nextTry;

    /// <summary>
    /// The dialogue condition: whether no introduction is on its way.
    /// </summary>
    public static bool NoneOnTheWay(DialogueCheck check) => check.Matches(For(check.World)?.Pending is null);

    /// <summary>
    /// The dialogue effect: orders the introduction of the race character whose record number is the value, unless one is on its way.
    /// </summary>
    public static void Introduce(DialogueAct act)
    {
        if (For(act.World) is not { Pending: null } system || IntroducerData.Of(act.Effect.Value) is not { } character) return;

        system.Pending = new Introduction(act.Effect.Value, act.World.GameHours + DelayHours);
        system._nextTry = 0;
        act.World.Post($"{IntroducerData.Name} will send word to a friend of his: a {Label(character)}. Expect them at your outpost in three days.");
    }

    /// <summary>
    /// The friend's way: added near the player's outpost once due, walked up to it, and done once it joined, died or left the world.
    /// </summary>
    void Advance(World world, Introduction introduction)
    {
        if (!introduction.Spawned)
        {
            if (world.GameHours >= introduction.Due && world.GameHours >= _nextTry) SetOut(world, introduction);
            return;
        }

        if (world.Platoons.FindByTag(_modId, Tag) is not { } platoon)
        {
            End(world, introduction, null);
            return;
        }

        if (platoon.SquadId < 0 || world.Squads.Find(platoon.SquadId) is not { } squad) return;

        if (Placing.Member(world, squad) is { } friend) Approach(world, introduction, friend);
        else End(world, introduction, platoon);
    }

    /// <summary>
    /// Adds the friend's platoon near the player's outpost and sends it there; when the player has none, tells them once and waits.
    /// </summary>
    void SetOut(World world, Introduction introduction)
    {
        var character = IntroducerData.Of(introduction.Number)!;
        if (Outpost(world) is not { } outpost)
        {
            if (!introduction.ToldWaiting) world.Post($"Marlo's friend, the {Label(character)}, is waiting for word of your outpost. Build one and they will come.");
            introduction.ToldWaiting = true;
            return;
        }

        var at = new Vector2(outpost.Position.X, outpost.Position.Z);
        int id = Add(world, IntroducerData.Drifters, IntroducerData.ArrivalSquad(character), at, ArriveFrom, ArriveTo);
        if (id < 0)
        {
            _nextTry = world.GameHours + 1;
            return;
        }

        world.Platoons.Find(id)!.ModTags[_modId] = Tag;
        PopulationSystem.SendPlatoon(world, id, _modId, at);
        introduction.Spawned = true;
        world.Post($"Marlo's friend, the {Label(character)}, is on the road to {outpost.Name}.");
    }

    /// <summary>
    /// Walks the loaded friend up to the outpost when it stands idle away from it, and tells the player once it is there.
    /// </summary>
    void Approach(World world, Introduction introduction, int friend)
    {
        _friend = world.Characters.IdOf(friend);
        if (Outpost(world) is not { } outpost || !Placing.Idle(world, friend, world.Characters.Cold(friend)!)) return;

        if (Placing.Flat(world.Characters.Next[friend].Position, outpost.Position) > OutpostReach)
        {
            Placing.Walk(world, friend, outpost.Position);
            return;
        }

        if (introduction.ToldArrived) return;

        introduction.ToldArrived = true;
        world.Post($"Marlo's friend, the {Label(IntroducerData.Of(introduction.Number)!)}, has reached {outpost.Name}. Talk to them.");
    }

    /// <summary>
    /// Ends the introduction (its friend joined, died or left the world), releases the platoon and frees the introducer for another.
    /// </summary>
    void End(World world, Introduction introduction, Platoon? platoon)
    {
        if (platoon is not null) PopulationSystem.HoldPlatoon(world, platoon.Id, _modId, false);

        var table = world.Characters;
        bool joined = table.TryResolveNext(_friend, out int s) && table.Cold(s) is { IsPlayer: true };
        string name = Label(IntroducerData.Of(introduction.Number)!);
        world.Post(joined ? $"The {name} Marlo sent has joined you." : $"The {name} Marlo sent will not be joining you. Marlo can send another.");
        Pending = null;
        _friend = CharacterId.None;
    }

    /// <summary>
    /// The player's first outpost; null when they have none.
    /// </summary>
    static PlayerTown? Outpost(World world) =>
        world.Player.Exists ? world.Buildings.Towns.Where(t => t.Owner == world.Player.Faction).MinBy(t => t.Number) : null;

    /// <summary>
    /// A race character as the player reads it: "crab queen", "Leoma woman".
    /// </summary>
    static string Label(RaceCharacter character) => IntroducerData.Label(character);

    #endregion

    #region Debug spawns

    /// <summary>
    /// The faction a hostile debug spawn joins: the Dust Bandits.
    /// </summary>
    public const string Hostile = "200-gamedata.base";

    /// <summary>
    /// How far round the camera target a debug spawn is put.
    /// </summary>
    const float SpawnSpread = 30;

    /// <summary>
    /// The tag of hostile debug spawns.
    /// </summary>
    const string HostileTag = "debug spawn";

    /// <summary>
    /// A debug spawn: into a player squad (the selected character's, else the first), or as a held Dust Bandit squad of one, at the camera target.
    /// </summary>
    void Spawn(World world, SpawnRaceCommand command)
    {
        if (_population is null || IntroducerData.Of(command.Number) is not { } character) return;

        var at = new Vector2(command.At.X, command.At.Z);
        bool done = command.Hostile ? SpawnHostile(world, character, at) : !SpawnRecruit(world, character, at, command.Selected).IsNone;
        if (!done) world.Post($"Kenshi Races: could not spawn a {Label(character)} there (pick a walkable spot near your squad).");
    }

    /// <summary>
    /// A race character in a player squad.
    /// </summary>
    CharacterId SpawnRecruit(World world, RaceCharacter character, Vector2 at, IReadOnlyList<CharacterId> selected)
    {
        var table = world.Characters;
        int squad = selected
            .Select(id => table.TryResolveNext(id, out int s) ? table.Cold(s) : null)
            .FirstOrDefault(c => c is { IsPlayer: true })?.SquadId
            ?? SquadManagement.PlayerSquads(world).FirstOrDefault()?.Id
            ?? -1;
        return _population!.SpawnIntoPlayerSquad(world, squad, character.Id, at);
    }

    /// <summary>
    /// The characters the editor opens on next (<see cref="TryRaces"/>).
    /// </summary>
    readonly EditorQueue _editor;

    /// <summary>
    /// How far apart round the camera target the race characters of <see cref="TryRaces"/> stand.
    /// </summary>
    const float TryRing = 25;

    /// <summary>
    /// One recruit of every race character in a ring round the camera target, handed to the character editor (<see cref="TryRacesRow"/>).
    /// </summary>
    void TryRaces(World world, TryRacesCommand command)
    {
        if (_population is null) return;

        var characters = Races.All.SelectMany(s => s.Characters).ToList();
        var made = new List<CharacterId>();
        for (int i = 0; i < characters.Count; i++)
        {
            float angle = 2 * MathF.PI * i / characters.Count;
            var at = new Vector2(command.At.X + TryRing * MathF.Cos(angle), command.At.Z + TryRing * MathF.Sin(angle));
            var id = SpawnRecruit(world, characters[i], at, command.Selected);
            if (!id.IsNone) made.Add(id);
        }

        if (made.Count < characters.Count) world.Post($"Kenshi Races: {characters.Count - made.Count} race characters could not be placed there.");
        if (made.Count > 0) _editor.Offer(made);
    }

    /// <summary>
    /// A race character alone in a Dust Bandit squad held where it stands, which fights the player's on sight.
    /// </summary>
    bool SpawnHostile(World world, RaceCharacter character, Vector2 at)
    {
        int id = Add(world, Hostile, IntroducerData.ArrivalSquad(character), at, 0, SpawnSpread);
        if (id < 0) return false;

        world.Platoons.Find(id)!.ModTags[_modId] = HostileTag;
        return true;
    }

    /// <summary>
    /// Adds a platoon of a template and faction round a point, homed at the nearest town, and holds it in the mod's name; -1 when that fails.
    /// </summary>
    int Add(World world, string faction, string template, Vector2 at, float minRadius, float maxRadius)
    {
        var data = _population!.Data;
        int index = data.FactionIndex(faction);
        var site = data.Sites.MinBy(s => Vector2.DistanceSquared(new Vector2(s.Position.X, s.Position.Z), at));
        int id = index < 0 || site is null ? -1 : _population.SpawnPlatoon(world, index, template, site.Index, at, minRadius, maxRadius);
        if (id >= 0) PopulationSystem.HoldPlatoon(world, id, _modId, true);

        return id;
    }

    #endregion

    #region Saving

    /// <summary>
    /// The save format's version.
    /// </summary>
    const byte Version = 1;

    /// <summary>
    /// Writes the introduction on its way.
    /// </summary>
    public void Write(Stream stream)
    {
        using var w = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
        w.Write(Version);
        w.Write(Pending is not null);
        if (Pending is not { } p) return;

        w.Write(p.Number);
        w.Write(p.Due);
        w.Write(p.Spawned);
        w.Write(p.ToldWaiting);
        w.Write(p.ToldArrived);
    }

    /// <summary>
    /// Reads the introduction on its way (a newer version's data is ignored).
    /// </summary>
    public void Read(Stream stream)
    {
        using var r = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);
        if (r.ReadByte() != Version || !r.ReadBoolean()) return;

        Pending = new Introduction(r.ReadInt32(), r.ReadDouble())
        {
            Spawned = r.ReadBoolean(),
            ToldWaiting = r.ReadBoolean(),
            ToldArrived = r.ReadBoolean(),
        };
    }

    /// <summary>
    /// Adds the introduction on its way to the state hash.
    /// </summary>
    public void Hash(ref StateHasher hasher)
    {
        hasher.Add(Pending is not null);
        if (Pending is not { } p) return;

        hasher.Add(p.Number);
        hasher.Add(p.Due);
        hasher.Add(p.Spawned);
        hasher.Add(p.ToldWaiting);
        hasher.Add(p.ToldArrived);
    }

    #endregion
}
