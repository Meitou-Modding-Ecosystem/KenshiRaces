using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// Static parts merged into a body (docs/race-import.md "Attachments") on a synthetic spine and a one-triangle body.
/// </summary>
public sealed class AttachmentsTests
{
    /// <summary>
    /// The spine's joints, bottom to top.
    /// </summary>
    static readonly (string Name, Vector3 At)[] Joints =
    [
        ("Bip01 Pelvis", new(0, 10, 0)), ("Bip01 Spine", new(0, 11, 0)), ("Bip01 Spine1", new(0, 12, 0)), ("Bip01 Spine2", new(0, 13, 0)),
        ("Bip01 Head", new(0, 16, 0)),
    ];

    [Fact]
    public void Each_piece_lands_its_anchor_at_its_place_and_the_left_mirrors_the_right()
    {
        var kenshi = Skeleton();
        var spec = new AttachmentSpec
        {
            Bones = ["Bip01 Head"], Height = 2, Anchor = [0, 0, 0], RelativeTo = "Bip01 Spine2", At = [0.5f, 1, -1], Roll = 20, Yaw = 30,
        };

        var (positions, normals) = Attachments.Place(Pair(spec), Body(kenshi), kenshi);

        Assert.Equal(new Vector3(0.5f, 14, -1), positions[0]);
        Assert.Equal(new Vector3(-0.5f, 14, -1), positions[3]);
        for (int v = 0; v < 3; v++)
        {
            Assert.Equal(positions[v] * new Vector3(-1, 1, 1), positions[v + 3]);
            Assert.Equal(normals[v] * new Vector3(-1, 1, 1), normals[v + 3]);
        }
    }

    [Fact]
    public void A_part_scales_to_its_height_and_turns_outward()
    {
        var kenshi = Skeleton();
        var spec = new AttachmentSpec { Bones = ["Bip01 Head"], Height = 2, Anchor = [0, 0, 0], RelativeTo = "Bip01 Spine2", Roll = 90 };

        var (positions, _) = Attachments.Place(Pair(spec), Body(kenshi), kenshi);

        // the right piece's top corner, 2 up from its anchor, falls outward to lie 2 out from it
        Assert.True(Vector3.Distance(positions[2], new Vector3(2, 13, 0)) < 1e-5f, $"top corner at {positions[2]}");
    }

    [Fact]
    public void A_part_between_two_bones_is_weighted_between_them_by_height()
    {
        var kenshi = Skeleton();
        string[] bones = ["Bip01 Spine2", "Bip01 Spine1", "Bip01 Pelvis"];

        var between = Attachments.Weights(bones, 12.25f, kenshi);
        Assert.Equal(0.25f, between[kenshi.Handle("Bip01 Spine2")], 4);
        Assert.Equal(0.75f, between[kenshi.Handle("Bip01 Spine1")], 4);
        Assert.Equal(1, Attachments.Weights(bones, 15, kenshi)[kenshi.Handle("Bip01 Spine2")]);
        Assert.Equal(1, Attachments.Weights(bones, 5, kenshi)[kenshi.Handle("Bip01 Pelvis")]);
    }

    [Fact]
    public void A_back_part_sits_its_gap_behind_the_body()
    {
        var kenshi = Skeleton();
        var spec = new AttachmentSpec
        {
            Bones = ["Bip01 Spine2", "Bip01 Pelvis"], Height = 0.2f, Anchor = [0, 0.5f, 1], RelativeTo = "Bip01 Spine2", At = [0, -1, 5],
            Behind = 0.3f,
        };

        var (positions, _) = Attachments.Place(Pair(spec), Back(kenshi), kenshi);

        Assert.Equal(-1.3f, positions.Max(p => p.Z), 4);
    }

    [Fact]
    public void Merged_parts_take_a_tile_of_the_atlas_and_double_sided_ones_face_both_ways()
    {
        var kenshi = Skeleton();
        var body = Body(kenshi);
        var part = Pair(new AttachmentSpec { Bones = ["Bip01 Head"], Height = 1 }, doubleSided: true);

        var (mesh, source) = Attachments.Apply(body, [0, 1, 2], [part], kenshi);

        Assert.Equal(3 + 12, mesh.Positions.Length);
        Assert.Equal([0, 1, 2, .. Enumerable.Repeat(-1, 12)], source);
        Assert.Equal(new Vector2(0.5f, 0.5f), mesh.Uvs[2]);
        Assert.Equal(new Vector2(-1, -0.5f), mesh.Uvs[1]);
        Assert.Equal(new Vector2(0.75f, 0), mesh.Uvs[3 + 1]);
        Assert.Equal(new Vector2(0.5f, 0.5f), mesh.Uvs[3 + 2]);
        Assert.Equal(-mesh.Normals[3], mesh.Normals[3 + 6]);
        Assert.Equal([3, 4, 5, 6, 8, 7, 9, 11, 10, 12, 13, 14], mesh.Indices[3..]);
        Assert.All(mesh.Weights[3..], w => Assert.Equal(1, w[kenshi.Handle("Bip01 Head")]));
    }

    /// <summary>
    /// A part of two mirrored triangles, one each side of X = 0, 1 high: bottom inner, bottom outer and top inner corners.
    /// </summary>
    static Attachment Pair(AttachmentSpec spec, bool doubleSided = false)
    {
        Vector3[] right = [new(0.2f, 0, 0), new(0.7f, 0, 0), new(0.2f, 1, 0)];
        return new Attachment
        {
            Spec = spec,
            Positions = [.. right, .. right.Select(p => p * new Vector3(-1, 1, 1))],
            Normals = [.. Enumerable.Repeat(new Vector3(0.6f, 0, 0.8f), 3), .. Enumerable.Repeat(new Vector3(-0.6f, 0, 0.8f), 3)],
            Uvs = [new(0, 0), new(1, 0), new(0, 1), new(0, 0), new(1, 0), new(0, 1)],
            Indices = [0, 1, 2, 3, 5, 4],
            DoubleSided = doubleSided,
            BaseColourImage = [],
        };
    }

    /// <summary>
    /// One triangle of back at Z = -1 across the spine, its second vertex on the grafted head's uvs (below 0).
    /// </summary>
    static SkinnedMesh Body(KenshiSkeleton kenshi)
    {
        ushort spine = kenshi.Handle("Bip01 Spine1");
        return new SkinnedMesh([new(-1, 11, -1), new(1, 11, -1), new(0, 14, -1)], [.. Enumerable.Repeat(-Vector3.UnitZ, 3)],
            [new(0, 0), new(-1, -0.5f), new(1, 0.5f)], [], [0, 1, 2], [.. Enumerable.Range(0, 3).Select(_ => new Dictionary<ushort, float> { [spine] = 1 })]);
    }

    /// <summary>
    /// A flat back at Z = -1 carried by the upper spine: vertices every 0.25 across and up.
    /// </summary>
    static SkinnedMesh Back(KenshiSkeleton kenshi)
    {
        ushort spine = kenshi.Handle("Bip01 Spine2");
        Vector3[] positions = [.. Enumerable.Range(0, 9).SelectMany(x => Enumerable.Range(0, 17).Select(y => new Vector3(x * 0.25f - 1, 10 + y * 0.25f, -1)))];
        return new SkinnedMesh(positions, [.. positions.Select(_ => -Vector3.UnitZ)], [.. positions.Select(_ => Vector2.Zero)], [], [0, 1, 2],
            [.. positions.Select(_ => new Dictionary<ushort, float> { [spine] = 1 })]);
    }

    /// <summary>
    /// The spine as a game skeleton, bones unturned.
    /// </summary>
    static KenshiSkeleton Skeleton()
    {
        var skeleton = new OgreSkeleton();
        for (int i = 0; i < Joints.Length; i++)
        {
            var local = i == 0 ? Joints[i].At : Joints[i].At - Joints[i - 1].At;
            skeleton.Bones.Add(new OgreBone { Name = Joints[i].Name, Handle = (ushort)i, Parent = i == 0 ? null : (ushort)(i - 1), Position = local });
        }

        return KenshiSkeleton.Read(skeleton);
    }
}
