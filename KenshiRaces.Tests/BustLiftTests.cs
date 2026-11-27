using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The bust lift (docs/race-import.md "Bust") on a few chest points of a synthetic skeleton.
/// </summary>
public sealed class BustLiftTests
{
    [Fact]
    public void A_sash_hanging_from_the_waist_is_not_taken_for_the_chest()
    {
        var kenshi = Skeleton();
        var human = Human(kenshi, [new(1, 13.6f, 1.2f), new(1, 14.6f, 0.8f)]);
        ushort spine2 = kenshi.Handle("Bip01 Spine2"), pelvis = kenshi.Handle("Bip01 Pelvis");
        Vector3[] model = [new(1, 13.6f, 1.2f), new(1, 14.6f, 0.8f), new(1, 11.8f, 1.6f)];
        Dictionary<ushort, float>[] weights = [new() { [spine2] = 1 }, new() { [spine2] = 1 }, new() { [pelvis] = 1 }];

        var (positions, lift) = BustLift.Apply(model, weights, kenshi, human);

        Assert.Equal(0, lift);
        Assert.Equal(model, positions);
    }

    [Fact]
    public void A_low_chest_rises_to_the_humans()
    {
        var kenshi = Skeleton();
        var human = Human(kenshi, [new(1, 13.6f, 1.2f), new(1, 14.6f, 0.8f)]);
        ushort spine2 = kenshi.Handle("Bip01 Spine2");
        Vector3[] model = [new(1, 12.6f, 1.2f), new(1, 14.6f, 0.8f)];
        Dictionary<ushort, float>[] weights = [new() { [spine2] = 1 }, new() { [spine2] = 1 }];

        var (_, lift) = BustLift.Apply(model, weights, kenshi, human);

        Assert.Equal(1, lift, 0.01);
    }

    /// <summary>
    /// A human whose skin is <paramref name="points"/>, all on <c>Bip01 Spine2</c>.
    /// </summary>
    static HumanBody Human(KenshiSkeleton kenshi, Vector3[] points)
    {
        ushort spine2 = kenshi.Handle("Bip01 Spine2");
        return new HumanBody
        {
            Positions = points,
            Uvs = [.. points.Select(_ => Vector2.Zero)],
            Bones = [.. points.Select(_ => spine2)],
            Weights = [.. points.Select(_ => new Dictionary<ushort, float> { [spine2] = 1 })],
            Indices = [],
        };
    }

    /// <summary>
    /// Pelvis, spines, neck, clavicles, upper arms and thighs at the heights a human's sit at, in game units.
    /// </summary>
    static KenshiSkeleton Skeleton()
    {
        (string Name, int? Parent, Vector3 Position)[] bones =
        [
            ("Pelvis", null, new(0, 10, 0)), ("Spine", 0, new(0, 0.5f, 0)), ("Spine1", 1, new(0, 1, 0)), ("Spine2", 2, new(0, 1.5f, 0)),
            ("Neck", 3, new(0, 2, 0)), ("L Clavicle", 3, new(0.3f, 1.8f, 0)), ("R Clavicle", 3, new(-0.3f, 1.8f, 0)),
            ("L UpperArm", 5, new(1.2f, -0.3f, 0)), ("R UpperArm", 6, new(-1.2f, -0.3f, 0)), ("L Thigh", 0, new(0.8f, -0.5f, 0)),
            ("R Thigh", 0, new(-0.8f, -0.5f, 0)),
        ];
        var skeleton = new OgreSkeleton();
        for (int i = 0; i < bones.Length; i++)
        {
            var (name, parent, position) = bones[i];
            skeleton.Bones.Add(new OgreBone { Name = "Bip01 " + name, Handle = (ushort)i, Parent = (ushort?)parent, Position = position });
        }

        return KenshiSkeleton.Read(skeleton);
    }
}
