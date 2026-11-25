using System.Numerics;
using Meitou.Data.Gameplay.Ai;
using Meitou.Simulation;
using Meitou.Simulation.Ai;
using Meitou.Simulation.Bodies;
using Meitou.Simulation.Factions;
using Meitou.Simulation.Movement;

namespace KenshiRaces.Introductions;

/// <summary>
/// Moving the mod's own NPCs: a walk with a move order, a stand at a point, and the checks before either. Serial steps only.
/// </summary>
static class Placing
{
    /// <summary>
    /// Walks a character to a point with a move order, which holds it against its squad's mind until it arrives; with no AI package in its squad
    /// it then stands idle.
    /// </summary>
    public static void Walk(World world, int slot, Vector3 point) =>
        world.GiveOrder(world.Characters.IdOf(slot), TaskType.MoveCusOrdered, point: point);

    /// <summary>
    /// Stands a character at a point, on the floor nearest its height there, as the debug menu's teleport does.
    /// </summary>
    public static void PutAt(World world, int slot, Vector3 point)
    {
        var table = world.Characters;
        TaskStart.Stand(table, slot);
        table.Next[slot].Position = new Vector3(point.X, world.Walkability.GroundHeight(point.X, point.Z, point.Y), point.Z);
        table.Next[slot].Velocity = Vector3.Zero;
    }

    /// <summary>
    /// Whether a character stands idle and able, out of a fight.
    /// </summary>
    public static bool Idle(World world, int slot, CharacterCold cold) =>
        !CharacterFacts.Down(cold) && !cold.InCombat && (CharacterTask)world.Characters.Next[slot].Task == CharacterTask.Idle;

    /// <summary>
    /// The first living member of a squad that is not the player's; null when none.
    /// </summary>
    public static int? Member(World world, Squad squad)
    {
        var table = world.Characters;
        foreach (var id in squad.Members)
        {
            if (!table.TryResolveNext(id, out int s) || !table.Next[s].Alive) continue;
            if (table.Cold(s) is { Medical.Dead: false, IsPlayer: false }) return s;
        }

        return null;
    }

    /// <summary>
    /// The horizontal distance between two points.
    /// </summary>
    public static float Flat(Vector3 a, Vector3 b) => Vector2.Distance(new Vector2(a.X, a.Z), new Vector2(b.X, b.Z));
}
