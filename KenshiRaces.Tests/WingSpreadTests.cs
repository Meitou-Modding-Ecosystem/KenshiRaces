using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The arm wings' spread (docs/race-import.md "Wings") on a synthetic body: T-posed in the model, its arms hanging down in the game's skeleton,
/// the torsos alike so the torso fit moves nothing.
/// </summary>
public sealed class WingSpreadTests
{
    /// <summary>
    /// The vertices: the left upper arm's skin, a membrane vertex just below it, one far below the forearm, a sole on the ground.
    /// </summary>
    const int Skin = 0, Near = 1, Far = 2;

    /// <summary>
    /// The left shoulder, and its elbow and wrist in the game's bind pose.
    /// </summary>
    static readonly Vector3 Shoulder = new(0.2f, 1.4f, 0), Elbow = new(0.2f, 1.1f, 0), Wrist = new(0.2f, 0.8f, 0);

    [Fact]
    public void The_membrane_shares_its_weight_between_arm_and_flank()
    {
        var (kenshi, weights, _) = Spread();

        ushort flank = kenshi.Handle("Bip01 Spine2");
        foreach (int v in new[] { Near, Far })
        {
            Assert.InRange(weights[v][flank], 0.01f, 0.99f);
            Assert.Equal(1, weights[v].Values.Sum(), 3);
            Assert.All(weights[v].Keys, h => Assert.True(h == flank || kenshi.Name(h)!.StartsWith("Bip01 L ", StringComparison.Ordinal)));
        }
    }

    [Fact]
    public void A_lowered_arm_holds_its_membrane_inside_itself()
    {
        var (_, _, positions) = Spread();

        foreach (int v in new[] { Near, Far }) Assert.InRange(ToArm(positions[v]), 0, 0.2501f);
    }

    [Fact]
    public void Raising_the_arm_to_the_models_pose_spreads_the_membrane_back()
    {
        var (kenshi, weights, positions) = Spread();

        float share = weights[Near][kenshi.Handle("Bip01 L UpperArm")];
        var turn = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI / 2);
        var raised = Shoulder + Vector3.Transform(positions[Near] - Shoulder, turn);
        var skinned = share * raised + (1 - share) * positions[Near];
        Assert.True(Vector3.Distance(skinned, new Vector3(0.35f, 1.3f, 0)) < 0.01f, $"spread back to {skinned}");
    }

    /// <summary>
    /// The synthetic body fitted and its wings spread: the game's skeleton, the fitted weights and positions.
    /// </summary>
    static (KenshiSkeleton Kenshi, Dictionary<ushort, float>[] Weights, Vector3[] Positions) Spread()
    {
        var body = Body();
        var kenshi = KenshiSkeleton.Read(Skeleton());
        var (membrane, _) = WingRig.Apply(body);
        var fit = new BodyFit(body, kenshi);
        var (positions, normals, weights) = fit.Repose();
        new WingSpread(fit, kenshi).Apply(body, membrane, positions, normals, weights);
        Assert.False(membrane[Skin]);
        return (kenshi, weights, positions);
    }

    /// <summary>
    /// The distance from a point to the left arm in the bind pose (shoulder to wrist).
    /// </summary>
    static float ToArm(Vector3 p) => MathF.Abs(p.X - Shoulder.X) + MathF.Abs(p.Z) + MathF.Max(0, Wrist.Y - p.Y) + MathF.Max(0, p.Y - Shoulder.Y);

    /// <summary>
    /// The joints both rigs share, at their model (T-pose) positions; the game's arms differ (<see cref="Skeleton"/>).
    /// </summary>
    static readonly (string Name, string? Parent, Vector3 At)[] Joints =
    [
        ("Pelvis", null, new(0, 1, 0)), ("Spine", "Pelvis", new(0, 1.1f, 0)), ("Spine1", "Spine", new(0, 1.2f, 0)),
        ("Spine2", "Spine1", new(0, 1.3f, 0)), ("Neck", "Spine2", new(0, 1.45f, 0)), ("Head", "Neck", new(0, 1.55f, 0)),
        ("L Clavicle", "Spine2", new(0.05f, 1.4f, 0)), ("L UpperArm", "L Clavicle", Shoulder), ("L Forearm", "L UpperArm", new(0.5f, 1.4f, 0)),
        ("L Hand", "L Forearm", new(0.8f, 1.4f, 0)), ("R Clavicle", "Spine2", new(-0.05f, 1.4f, 0)), ("R UpperArm", "R Clavicle", new(-0.2f, 1.4f, 0)),
        ("R Forearm", "R UpperArm", new(-0.5f, 1.4f, 0)), ("R Hand", "R Forearm", new(-0.8f, 1.4f, 0)),
        ("L Thigh", "Pelvis", new(0.1f, 0.95f, 0)), ("L Calf", "L Thigh", new(0.1f, 0.5f, 0)), ("L Foot", "L Calf", new(0.1f, 0.1f, 0)),
        ("L Toe0", "L Foot", new(0.1f, 0.02f, 0.1f)), ("R Thigh", "Pelvis", new(-0.1f, 0.95f, 0)), ("R Calf", "R Thigh", new(-0.1f, 0.5f, 0)),
        ("R Foot", "R Calf", new(-0.1f, 0.1f, 0)), ("R Toe0", "R Foot", new(-0.1f, 0.02f, 0.1f)),
    ];

    /// <summary>
    /// The model: the shared joints plus a head top, T-posed, its vertices weighted to the pelvis.
    /// </summary>
    static SourceBody Body()
    {
        string[] names = [.. Joints.Select(j => "Bip001 " + j.Name), "mixamorig:HeadTop_End"];
        int[] parents = [.. Joints.Select(j => j.Parent is null ? -1 : Array.FindIndex(Joints, k => k.Name == j.Parent)), 5];
        Vector3[] at = [.. Joints.Select(j => j.At), new(0, 1.75f, 0)];
        Vector3[] positions = [new(0.3f, 1.43f, 0), new(0.35f, 1.3f, 0), new(0.6f, 1.2f, 0), new(0.1f, 0, 0.05f)];
        return new SourceBody
        {
            Positions = positions,
            Normals = [.. positions.Select(_ => Vector3.UnitZ)],
            Uvs = [.. positions.Select(_ => Vector2.Zero)],
            Indices = [0, 1, 2],
            Influences = [.. positions.Select(_ => new (int, float)[] { (0, 1f) })],
            JointNames = names,
            JointParents = parents,
            JointPositions = at,
        };
    }

    /// <summary>
    /// The game's skeleton: the shared joints with the left and right arms hanging down, plus the head's nub; bones unturned.
    /// </summary>
    static OgreSkeleton Skeleton()
    {
        var at = Joints.ToDictionary(j => j.Name, j => j.At);
        at["L Forearm"] = Elbow;
        at["L Hand"] = Wrist;
        at["R Forearm"] = new(-0.2f, 1.1f, 0);
        at["R Hand"] = new(-0.2f, 0.8f, 0);
        var skeleton = new OgreSkeleton();
        (string Name, string? Parent)[] all = [.. Joints.Select(j => (j.Name, j.Parent)), ("HeadNub", "Head")];
        at["HeadNub"] = new(0, 1.75f, 0);
        for (int i = 0; i < all.Length; i++)
        {
            var (name, parent) = all[i];
            int p = parent is null ? -1 : Array.FindIndex(all, k => k.Name == parent);
            var local = p < 0 ? at[name] : at[name] - at[parent!];
            skeleton.Bones.Add(new OgreBone { Name = "Bip01 " + name, Handle = (ushort)i, Parent = p < 0 ? null : (ushort)p, Position = local });
        }

        return skeleton;
    }
}
