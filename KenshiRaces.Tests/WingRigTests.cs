using System.Numerics;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The arm wings' rebinding (docs/race-import.md "Wings") on a synthetic T-posed pair of arms with a membrane hanging below each.
/// </summary>
public sealed class WingRigTests
{
    /// <summary>
    /// The vertices: the left arm's skin above the upper arm, a membrane vertex below it, one below the hand's tip, a torso vertex.
    /// </summary>
    const int Skin = 0, Membrane = 1, Tip = 2, Torso = 3;

    [Fact]
    public void The_membrane_below_the_arm_is_marked_and_left_in_place()
    {
        var body = Arms();

        var (membrane, sleeved) = WingRig.Apply(body);

        Assert.True(membrane[Membrane]);
        Assert.True(sleeved[Membrane]);
        Assert.Equal(new Vector3(0.4f, 1.22f, 0), body.Positions[Membrane]);
    }

    [Fact]
    public void The_arms_skin_stays_put_and_everything_past_the_shoulder_rides_the_arm_bones()
    {
        var body = Arms();

        var (membrane, _) = WingRig.Apply(body);

        Assert.False(membrane[Skin]);
        Assert.Equal(new Vector3(0.3f, 1.43f, 0), body.Positions[Skin]);
        Assert.All(body.Influences[Membrane], i => Assert.Contains(i.Joint, new[] { 1, 2, 3 }));
        Assert.Equal(1, Assert.Single(body.Influences[Skin]).Joint);
        Assert.Equal(0, Assert.Single(body.Influences[Torso]).Joint);
    }

    [Fact]
    public void The_membrane_past_the_wrist_is_not_sleeved()
    {
        var body = Arms();

        var (membrane, sleeved) = WingRig.Apply(body);

        Assert.True(membrane[Tip]);
        Assert.False(sleeved[Tip]);
    }

    /// <summary>
    /// A body with a spine and two arms along x at height 1.4 (shoulder 0.2, elbow 0.5, wrist 0.8), weighted to the spine.
    /// </summary>
    static SourceBody Arms()
    {
        string[] names =
        [
            "Bip001 Spine", "Bip001 L UpperArm", "Bip001 L Forearm", "Bip001 L Hand", "Bip001 R UpperArm", "Bip001 R Forearm", "Bip001 R Hand",
        ];
        Vector3[] joints =
        [
            new(0, 1.1f, 0), new(0.2f, 1.4f, 0), new(0.5f, 1.4f, 0), new(0.8f, 1.4f, 0), new(-0.2f, 1.4f, 0), new(-0.5f, 1.4f, 0), new(-0.8f, 1.4f, 0),
        ];
        Vector3[] positions = [new(0.3f, 1.43f, 0), new(0.4f, 1.22f, 0), new(0.95f, 1.2f, 0), new(0, 1.2f, 0.1f)];
        return new SourceBody
        {
            Positions = positions,
            Normals = [.. positions.Select(_ => Vector3.UnitZ)],
            Uvs = [.. positions.Select(_ => Vector2.Zero)],
            Indices = [0, 1, 2],
            Influences = [.. positions.Select(_ => new (int, float)[] { (0, 1f) })],
            JointNames = names,
            JointParents = [-1, 0, 1, 2, 0, 4, 5],
            JointPositions = joints,
        };
    }
}
