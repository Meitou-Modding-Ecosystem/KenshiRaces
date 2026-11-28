using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The arm fill (docs/race-import.md "Arm fill") on a synthetic right arm hanging down: upper arm from y 15 to 12, forearm to 9, at x -2.
/// </summary>
public sealed class ArmFillTests
{
    [Fact]
    public void A_thin_arm_is_widened_to_the_humans_width_about_the_humans_middle()
    {
        var (kenshi, human) = Arm(-2.5f, -1.5f);
        ushort upper = kenshi.Handle("Bip01 R UpperArm");
        var (positions, weights) = Ring(-2.4f, -2.0f, upper);

        var (filled, most) = ArmFill.Apply(positions, weights, kenshi, human, new bool[positions.Length]);

        Assert.Equal(2, most, 0.01);
        // Twice its 0.4 width (the most), about the human's middle at -2.
        Assert.Equal(-2.4f, filled.Min(p => p.X), 0.05);
        Assert.Equal(-1.6f, filled.Max(p => p.X), 0.05);
    }

    [Fact]
    public void An_arm_as_wide_as_the_humans_and_spared_vertices_stay()
    {
        var (kenshi, human) = Arm(-2.5f, -1.5f);
        ushort upper = kenshi.Handle("Bip01 R UpperArm");
        var (positions, weights) = Ring(-2.5f, -1.5f, upper);
        var (thin, thinWeights) = Ring(-2.2f, -1.8f, upper);

        Assert.Equal(positions, ArmFill.Apply(positions, weights, kenshi, human, new bool[positions.Length]).Positions);
        Assert.Equal(thin, ArmFill.Apply(thin, thinWeights, kenshi, human, [.. thin.Select(_ => true)]).Positions);
    }

    /// <summary>
    /// Points from x <paramref name="low"/> to <paramref name="high"/> down the upper arm, wholly on <paramref name="bone"/>.
    /// </summary>
    static (Vector3[] Positions, Dictionary<ushort, float>[] Weights) Ring(float low, float high, ushort bone)
    {
        var positions = new List<Vector3>();
        for (float y = 14.9f; y > 12.1f; y -= 0.2f)
            for (int k = 0; k <= 4; k++) positions.Add(new Vector3(low + (high - low) * k / 4, y, -0.5f + 0.25f * k));

        return ([.. positions], [.. positions.Select(_ => new Dictionary<ushort, float> { [bone] = 1 })]);
    }

    /// <summary>
    /// The skeleton and a human whose upper arm spans x <paramref name="low"/> to <paramref name="high"/>.
    /// </summary>
    static (KenshiSkeleton Kenshi, HumanBody Human) Arm(float low, float high)
    {
        (string Name, int? Parent, Vector3 Position)[] bones =
        [
            ("Pelvis", null, new(0, 10, 0)), ("Spine2", 0, new(0, 3, 0)), ("R Clavicle", 1, new(-0.2f, 2, 0)),
            ("R UpperArm", 2, new(-1.8f, 0, 0)), ("R Forearm", 3, new(0, -3, 0)), ("R Hand", 4, new(0, -3, 0)),
        ];
        var skeleton = new OgreSkeleton();
        for (int i = 0; i < bones.Length; i++)
        {
            var (name, parent, position) = bones[i];
            skeleton.Bones.Add(new OgreBone { Name = "Bip01 " + name, Handle = (ushort)i, Parent = (ushort?)parent, Position = position });
        }

        var kenshi = KenshiSkeleton.Read(skeleton);
        ushort upper = kenshi.Handle("Bip01 R UpperArm");
        var (points, _) = Ring(low, high, upper);
        var human = new HumanBody
        {
            Positions = points,
            Uvs = [.. points.Select(_ => Vector2.Zero)],
            Bones = [.. points.Select(_ => upper)],
            Weights = [.. points.Select(_ => new Dictionary<ushort, float> { [upper] = 1 })],
            Indices = [],
        };
        return (kenshi, human);
    }
}
