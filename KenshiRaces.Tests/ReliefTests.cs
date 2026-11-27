using RaceImport;
using StructureImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The relief option (docs/race-import.md "Relief"): the skin's fine shading and the normal map's slopes scaled down, the broad shading kept.
/// </summary>
public sealed class ReliefTests
{
    [Fact]
    public void Fine_shading_shrinks_by_the_relief_and_its_mean_stays()
    {
        const int side = 64;
        float[] values = [.. Enumerable.Range(0, side * side).Select(i => 100f + (i % 2 == 0 ? 20 : -20))];
        bool[] mask = [.. values.Select(_ => true)];

        var softened = Relief.Softened(values, mask, side, 0.5f);

        Assert.All(softened, v => Assert.InRange(MathF.Abs(v - 100), 8, 12));
        Assert.Equal(100, softened.Average(), 0.5);
    }

    [Fact]
    public void Texels_outside_the_mask_and_full_relief_are_unchanged()
    {
        const int side = 32;
        float[] values = [.. Enumerable.Range(0, side * side).Select(i => (float)(i % 7) * 10)];
        bool[] mask = [.. values.Select((_, i) => i % side < side / 2)];

        var softened = Relief.Softened(values, mask, side, 0);

        Assert.All(Enumerable.Range(0, values.Length).Where(i => !mask[i]), i => Assert.Equal(values[i], softened[i]));
        Assert.Same(values, Relief.Softened(values, mask, side, 1));
    }

    [Fact]
    public void The_normal_maps_slopes_move_towards_flat()
    {
        var image = new Rgba(2, 1, (228, 28, 200, 255));

        var flattened = Rgba.Decode(Relief.FlattenedNormal(PartMap.Png(image), 0.5f)!);

        Assert.Equal([178, 78, 200, 255], flattened.Pixels[..4]);
        Assert.Null(Relief.FlattenedNormal(null, 0.5f));
    }
}
