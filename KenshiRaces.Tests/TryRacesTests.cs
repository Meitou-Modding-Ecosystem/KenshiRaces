using System.Numerics;
using KenshiRaces.Introductions;
using Meitou.Modding;
using Meitou.Simulation;

namespace KenshiRaces.Tests;

/// <summary>
/// The F10 row "Kenshi Races: try every race in the character editor": it queues the spawns, and the editor opens once on the characters made.
/// </summary>
public sealed class TryRacesTests
{
    [Fact]
    public void The_row_queues_one_spawn_at_the_camera_target_for_the_selected_squad()
    {
        var queued = new List<SimCommand>();
        CharacterId[] selected = [new(3, 1)];
        var target = new Vector3(10, 0, 20);

        string said = TryRacesRow.Run(new DebugCommandContext("", selected, target, 42, queued.Add));

        var command = Assert.IsType<TryRacesCommand>(Assert.Single(queued));
        Assert.Equal((target, 42L), (command.At, command.Tick));
        Assert.Equal(selected, command.Selected);
        Assert.Contains(Races.All.Sum(s => s.Characters.Count).ToString(), said);
    }

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
}
