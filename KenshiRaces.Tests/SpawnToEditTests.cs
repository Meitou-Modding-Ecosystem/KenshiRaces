using KenshiRaces.Introductions;
using Meitou.Modding;
using Meitou.Simulation;

namespace KenshiRaces.Tests;

/// <summary>
/// A joining "spawn race" debug spawn: it puts on a Standard dustcoat, and the character editor opens once on it.
/// </summary>
public sealed class SpawnToEditTests
{
    [Fact]
    public void The_editor_opens_once_on_the_offered_characters_after_refusals()
    {
        var editor = new EditorQueue();
        CharacterId[] made = [new(1, 1), new(2, 1)];
        var opened = new List<IReadOnlyList<CharacterId>>();
        bool ready = false;

        editor.Open(c => throw new InvalidOperationException("nothing offered yet"));
        editor.Offer(made);
        editor.Open(c => ready);
        ready = true;
        editor.Open(c => { opened.Add(c); return ready; });
        editor.Open(c => { opened.Add(c); return true; });

        Assert.Equal(made, Assert.Single(opened));
    }

    [Fact]
    public void A_joining_spawn_wears_a_standard_dustcoat_the_next_tick()
    {
        var world = new World(new WorldSettings { Seed = 1, PublishSnapshots = false }, new OpenGroundWalkability((_, _) => 0));
        var id = new CharacterId(7, 1);

        var coat = IntroductionSystem.Coat(world, id);

        Assert.Equal(new DebugGiveItem(id, "3061-gamedata.base", 1, 40) { Tick = world.Tick + 1, Equip = true }, coat);
    }
}
