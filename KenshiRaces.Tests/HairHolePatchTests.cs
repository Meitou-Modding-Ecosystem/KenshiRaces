using System.Numerics;
using Meitou.Data.Ogre;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The hair hole patch (docs/race-import.md "Hair holes") on a synthetic torso: a box of small triangles carried by <c>Bip01 Spine1</c>, the
/// human's skin, and a body copied from it.
/// </summary>
public sealed class HairHolePatchTests
{
    [Fact]
    public void A_closed_body_takes_no_patch()
    {
        var (kenshi, human) = Human();

        Assert.Empty(HairHolePatch.PatchTriangles(Body(human, _ => true), human, kenshi));
    }

    [Fact]
    public void A_hole_in_the_back_is_closed_with_the_humans_skin_under_it()
    {
        var (kenshi, human) = Human();
        var body = Body(human, t => !InHole(Centre(human, t)));

        var patch = HairHolePatch.PatchTriangles(body, human, kenshi);

        var hole = Enumerable.Range(0, human.Indices.Length / 3).Where(t => InHole(Centre(human, t))).ToList();
        Assert.NotEmpty(hole);
        Assert.Empty(hole.Where(t => !patch.Contains(t)).Select(t => Centre(human, t)));
        Assert.DoesNotContain(patch, t => Centre(human, t).Z > -0.9f);
    }

    [Fact]
    public void The_patch_sits_under_the_skin_with_the_humans_weights_and_one_skin_texel()
    {
        var (kenshi, human) = Human();
        var body = Body(human, t => !InHole(Centre(human, t)));
        var source = Enumerable.Range(0, body.Positions.Length).ToArray();
        var uv = new Vector2(0.25f, 0.5f);

        var (mesh, patchedSource, triangles) = HairHolePatch.Apply(body, source, human, kenshi, uv);

        Assert.True(triangles > 0);
        var added = Enumerable.Range(body.Positions.Length, mesh.Positions.Length - body.Positions.Length).ToList();
        Assert.All(added, v => Assert.Equal(HairHolePatch.Patched, patchedSource[v]));
        Assert.All(added, v => Assert.Equal(uv, mesh.Uvs[v]));
        Assert.All(added, v => Assert.Equal(1f, mesh.Weights[v][kenshi.Handle("Bip01 Spine1")]));
        Assert.Contains(added, v => mesh.Positions[v].Z > -1 && mesh.Positions[v].Z < -0.95f);
    }

    [Fact]
    public void Bare_skin_under_a_fuller_closed_body_is_not_copied()
    {
        var (kenshi, human) = Human();
        var fuller = Body(human, _ => true);
        fuller = fuller with { Positions = [.. fuller.Positions.Select(p => p with { Z = p.Z * 1.6f })] };

        Assert.Empty(HairHolePatch.PatchTriangles(fuller, human, kenshi));
    }

    /// <summary>
    /// Whether a point is in the hole cut out of the body's back.
    /// </summary>
    static bool InHole(Vector3 p) => p.Z < -0.9f && MathF.Abs(p.X) < 0.8f && p.Y > 11.5f && p.Y < 13.5f;

    /// <summary>
    /// A body copied from the human: the triangles <paramref name="keep"/> keeps, every vertex.
    /// </summary>
    static SkinnedMesh Body(HumanBody human, Func<int, bool> keep)
    {
        int[] indices = [.. Enumerable.Range(0, human.Indices.Length / 3).Where(keep).SelectMany(t => human.Indices.Skip(3 * t).Take(3))];
        return new SkinnedMesh([.. human.Positions], [.. human.Positions.Select(_ => Vector3.UnitY)], [.. human.Positions.Select(_ => new Vector2(0.5f))],
            [], indices, [.. human.Weights.Select(w => new Dictionary<ushort, float>(w))]);
    }

    /// <summary>
    /// A human triangle's centre.
    /// </summary>
    static Vector3 Centre(HumanBody human, int t) =>
        (human.Positions[human.Indices[3 * t]] + human.Positions[human.Indices[3 * t + 1]] + human.Positions[human.Indices[3 * t + 2]]) / 3;

    /// <summary>
    /// The synthetic skeleton (pelvis, three spines, neck, head) and the human: a torso box from x -1.5 to 1.5, y 10 to 15, z -1 to 1, its faces
    /// split into 0.25 squares wound outward, all on <c>Bip01 Spine1</c>.
    /// </summary>
    static (KenshiSkeleton Kenshi, HumanBody Human) Human()
    {
        var skeleton = new OgreSkeleton();
        string[] names = ["Pelvis", "Spine", "Spine1", "Spine2", "Neck", "Head"];
        float[] heights = [10, 10.5f, 11.5f, 13, 15, 16];
        for (int i = 0; i < names.Length; i++)
        {
            var local = new Vector3(0, i == 0 ? heights[0] : heights[i] - heights[i - 1], 0);
            skeleton.Bones.Add(new OgreBone { Name = "Bip01 " + names[i], Handle = (ushort)i, Parent = i == 0 ? null : (ushort)(i - 1), Position = local });
        }

        var kenshi = KenshiSkeleton.Read(skeleton);
        var (positions, indices) = Box(new Vector3(-1.5f, 10, -1), new Vector3(1.5f, 15, 1), 0.25f);
        ushort spine = kenshi.Handle("Bip01 Spine1");
        var human = new HumanBody
        {
            Positions = [.. positions],
            Uvs = [.. positions.Select(_ => Vector2.Zero)],
            Bones = [.. positions.Select(_ => spine)],
            Weights = [.. positions.Select(_ => new Dictionary<ushort, float> { [spine] = 1 })],
            Indices = [.. indices],
        };
        return (kenshi, human);
    }

    /// <summary>
    /// A box's six faces, each a grid of <paramref name="step"/> squares (two triangles each) wound outward.
    /// </summary>
    static (List<Vector3> Positions, List<int> Indices) Box(Vector3 min, Vector3 max, float step)
    {
        var positions = new List<Vector3>();
        var indices = new List<int>();
        var size = max - min;
        (Vector3 Origin, Vector3 U, Vector3 V)[] faces =
        [
            (min, new(size.X, 0, 0), new(0, size.Y, 0)),
            (min with { Z = max.Z }, new(0, size.Y, 0), new(size.X, 0, 0)),
            (min, new(0, size.Y, 0), new(0, 0, size.Z)),
            (min with { X = max.X }, new(0, 0, size.Z), new(0, size.Y, 0)),
            (min, new(0, 0, size.Z), new(size.X, 0, 0)),
            (min with { Y = max.Y }, new(size.X, 0, 0), new(0, 0, size.Z)),
        ];
        foreach (var (origin, u, v) in faces) AddGrid(origin, u, v, step, positions, indices);

        return (positions, indices);
    }

    /// <summary>
    /// Adds the grid of one face: <paramref name="u"/> by <paramref name="v"/> from <paramref name="origin"/>, cut into squares of about
    /// <paramref name="step"/>.
    /// </summary>
    static void AddGrid(Vector3 origin, Vector3 u, Vector3 v, float step, List<Vector3> positions, List<int> indices)
    {
        int nu = (int)MathF.Round(u.Length() / step), nv = (int)MathF.Round(v.Length() / step);
        int first = positions.Count;
        for (int j = 0; j <= nv; j++)
            for (int i = 0; i <= nu; i++)
                positions.Add(origin + u * i / nu + v * j / nv);

        for (int j = 0; j < nv; j++)
            for (int i = 0; i < nu; i++)
            {
                int a = first + j * (nu + 1) + i, b = a + 1, c = a + nu + 1, d = c + 1;
                indices.AddRange([a, c, b, b, c, d]);
            }
    }
}
