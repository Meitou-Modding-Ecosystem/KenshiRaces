using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The skin conform (docs/race-import.md "Skin conform") on a synthetic human of boxes: a torso, a pelvis, arms hanging beside the torso and
/// hands. Each test vertex stands alone, so smoothing leaves its pull as it is.
/// </summary>
public sealed class SkinConformTests
{
    [Fact]
    public void An_arm_is_pulled_onto_the_arm_skin()
    {
        var moved = Conformed(new Vector3(3.4f, 12, 0), "Bip01 L UpperArm");

        Assert.InRange(moved.X, 3.0f, 3.1f);
    }

    [Fact]
    public void The_inside_of_an_arm_is_pulled_onto_its_own_arm_not_the_flank_beside_it()
    {
        var moved = Conformed(new Vector3(1.6f, 12, 0), "Bip01 L Forearm");

        Assert.InRange(moved.X, 1.9f, 2.0f);
    }

    [Fact]
    public void A_hand_stays()
    {
        var at = new Vector3(3.4f, 7.5f, 0);

        Assert.Equal(at, Conformed(at, "Bip01 L Hand"));
    }

    [Fact]
    public void The_front_of_the_crotch_is_pulled_onto_the_pelvis()
    {
        var moved = Conformed(new Vector3(0.2f, 9, 1.5f), "Bip01 Pelvis");

        Assert.InRange(moved.Z, 1.0f, 1.1f);
        Assert.Equal(9, moved.Y, 3);
    }

    [Fact]
    public void What_hangs_behind_the_crotch_stays()
    {
        var at = new Vector3(0.2f, 9, -1.5f);

        Assert.Equal(at, Conformed(at, "Bip01 Pelvis"));
    }

    [Theory]
    [InlineData(SkinPull.All, true)]
    [InlineData(SkinPull.Arms | SkinPull.Groin, false)]
    public void The_torso_is_pulled_only_when_asked(SkinPull pulls, bool pulled)
    {
        var at = new Vector3(0, 12, 1.5f);

        var moved = Conformed(at, "Bip01 Spine1", pulls);

        Assert.Equal(pulled, moved.Z < 1.1f);
    }

    [Fact]
    public void Arms_alone_leave_the_crotch()
    {
        var at = new Vector3(0.2f, 9, 1.5f);

        Assert.Equal(at, Conformed(at, "Bip01 Pelvis", SkinPull.Arms));
    }

    /// <summary>
    /// The position of one vertex at <paramref name="at"/> carried wholly by <paramref name="bone"/> after the conform of <paramref name="pulls"/>.
    /// </summary>
    static Vector3 Conformed(Vector3 at, string bone, SkinPull pulls = SkinPull.All)
    {
        var kenshi = KenshiSkeleton.Read(Skeleton());
        var weights = new[] { new Dictionary<ushort, float> { [kenshi.Handle(bone)] = 1 } };

        var (positions, _) = new SkinConform(kenshi, Human(kenshi), pulls).Apply([at], weights, [], 1, [false]);

        return positions[0];
    }

    /// <summary>
    /// The joints of the synthetic skeleton (game units): standing, arms hanging at the sides.
    /// </summary>
    static readonly (string Name, string? Parent, Vector3 At)[] Joints =
    [
        ("Pelvis", null, new(0, 10, 0)), ("Spine", "Pelvis", new(0, 10.5f, 0)), ("Spine1", "Spine", new(0, 11.5f, 0)),
        ("Spine2", "Spine1", new(0, 13, 0)), ("Neck", "Spine2", new(0, 15, 0)), ("Head", "Neck", new(0, 16, 0)),
        ("L Clavicle", "Spine2", new(0.3f, 14.5f, 0)), ("L UpperArm", "L Clavicle", new(2.5f, 14.5f, 0)),
        ("L Forearm", "L UpperArm", new(2.5f, 11.5f, 0)), ("L Hand", "L Forearm", new(2.5f, 8, 0)),
        ("R Clavicle", "Spine2", new(-0.3f, 14.5f, 0)), ("R UpperArm", "R Clavicle", new(-2.5f, 14.5f, 0)),
        ("R Forearm", "R UpperArm", new(-2.5f, 11.5f, 0)), ("R Hand", "R Forearm", new(-2.5f, 8, 0)),
        ("L Thigh", "Pelvis", new(0.8f, 9.5f, 0)), ("L Calf", "L Thigh", new(0.8f, 5, 0)),
        ("R Thigh", "Pelvis", new(-0.8f, 9.5f, 0)), ("R Calf", "R Thigh", new(-0.8f, 5, 0)),
    ];

    /// <summary>
    /// The synthetic skeleton, bones unturned.
    /// </summary>
    static OgreSkeleton Skeleton()
    {
        var skeleton = new OgreSkeleton();
        for (int i = 0; i < Joints.Length; i++)
        {
            var (name, parent, at) = Joints[i];
            int p = parent is null ? -1 : Array.FindIndex(Joints, j => j.Name == parent);
            var local = p < 0 ? at : at - Joints[p].At;
            skeleton.Bones.Add(new OgreBone { Name = "Bip01 " + name, Handle = (ushort)i, Parent = p < 0 ? null : (ushort)p, Position = local });
        }

        return skeleton;
    }

    /// <summary>
    /// The synthetic human: boxes for the torso, the pelvis, each arm and each hand, each carried by one bone.
    /// </summary>
    static HumanBody Human(KenshiSkeleton kenshi)
    {
        (Vector3 Min, Vector3 Max, string Bone)[] boxes =
        [
            (new(-1.5f, 10, -1), new(1.5f, 15, 1), "Bip01 Spine1"),
            (new(-1.5f, 8.5f, -1), new(1.5f, 10, 1), "Bip01 Pelvis"),
            (new(2, 8, -0.5f), new(3, 14.5f, 0.5f), "Bip01 L UpperArm"),
            (new(-3, 8, -0.5f), new(-2, 14.5f, 0.5f), "Bip01 R UpperArm"),
            (new(2, 6.5f, -0.5f), new(3, 8, 0.5f), "Bip01 L Hand"),
            (new(-3, 6.5f, -0.5f), new(-2, 8, 0.5f), "Bip01 R Hand"),
        ];
        var positions = new List<Vector3>();
        var indices = new List<int>();
        var bones = new List<ushort>();
        foreach (var (min, max, bone) in boxes) AddBox(min, max, kenshi.Handle(bone), positions, indices, bones);

        return new HumanBody
        {
            Positions = [.. positions],
            Uvs = [.. positions.Select(_ => Vector2.Zero)],
            Bones = [.. bones],
            Weights = [.. bones.Select(b => new Dictionary<ushort, float> { [b] = 1 })],
            Indices = [.. indices],
        };
    }

    /// <summary>
    /// Adds a box's eight corners and twelve triangles, wound outward.
    /// </summary>
    static void AddBox(Vector3 min, Vector3 max, ushort bone, List<Vector3> positions, List<int> indices, List<ushort> bones)
    {
        int first = positions.Count;
        for (int c = 0; c < 8; c++)
        {
            positions.Add(new((c & 1) == 0 ? min.X : max.X, (c & 2) == 0 ? min.Y : max.Y, (c & 4) == 0 ? min.Z : max.Z));
            bones.Add(bone);
        }

        int[] faces = [0, 2, 3, 0, 3, 1, 4, 5, 7, 4, 7, 6, 0, 1, 5, 0, 5, 4, 2, 6, 7, 2, 7, 3, 0, 4, 6, 0, 6, 2, 1, 3, 7, 1, 7, 5];
        indices.AddRange(faces.Select(f => first + f));
    }
}
