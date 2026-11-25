using System.Numerics;

namespace RaceImport;

/// <summary>
/// Puts the human head on a fitted body (docs/race-import.md "Headless"). The body's vertices within <see cref="Band"/> of the head's open neck
/// rim sink just under the human's skin (the closest point of its body or head, <see cref="Sink"/> inside) and take its weights there, so the
/// body's neck runs into the human's along the rim with no gap; the vertices nearer the head than that are dropped with their triangles (what
/// the cut leaves open lies under the head's skin), and the head's vertices, triangles and face poses are appended.
/// </summary>
public static class HeadGraft
{
    /// <summary>
    /// Body vertices this close to the rim sink onto the human's skin, in game units: more than a fitted body's triangle (about 0.4), so that
    /// no kept triangle reaches from below the rim to where the cut is.
    /// </summary>
    const float Band = 0.8f;

    /// <summary>
    /// How far under the human's skin the sunk vertices sit, so the head's skin is drawn over the body's where they overlap, in game units.
    /// </summary>
    const float Sink = 0.05f;

    /// <summary>
    /// The fitted body with the head grafted on, and for each of its vertices the body vertex it was (-1 for the head's). With
    /// <paramref name="earBone"/> the model's ears stay (<see cref="HeadEars"/>), carried by that bone.
    /// </summary>
    public static (SkinnedMesh Mesh, int[] Source) Apply(SkinnedMesh body, HumanBody human, HumanHead head, ushort? earBone = null)
    {
        var rim = Rim(head);
        var near = Enumerable.Range(0, body.Positions.Length).Where(v => Distance(body.Positions[v], rim) < Band).ToHashSet();
        var dropped = Dropped(body.Positions, near, human);
        var sunk = Sunk(body, near, human);
        if (earBone is { } bone) sunk = KeepEars(sunk, dropped, head, bone);
        var source = Enumerable.Range(0, body.Positions.Length).Where(v => !dropped[v]).ToArray();
        var mesh = Joined(sunk, source, head);
        return (mesh, [.. source, .. Enumerable.Repeat(-1, head.Positions.Length)]);
    }

    /// <summary>
    /// The body with its ears placed on the human head (<see cref="HeadEars"/>), no longer <paramref name="dropped"/>, carried by
    /// <paramref name="bone"/> alone.
    /// </summary>
    static SkinnedMesh KeepEars(SkinnedMesh body, bool[] dropped, HumanHead head, ushort bone)
    {
        var positions = (Vector3[])body.Positions.Clone();
        var weights = body.Weights.ToArray();
        foreach (int v in HeadEars.Place(positions, body.Indices, dropped, head))
        {
            dropped[v] = false;
            weights[v] = new Dictionary<ushort, float> { [bone] = 1 };
        }

        return body with { Positions = positions, Weights = weights };
    }

    /// <summary>
    /// The positions of the head's rim: the vertices on a welded edge that only one of its triangles has.
    /// </summary>
    static Vector3[] Rim(HumanHead head)
    {
        var weld = Surface.Weld(head.Positions);
        var count = new Dictionary<(int, int), int>();
        for (int t = 0; t < head.Indices.Length; t += 3)
            for (int k = 0; k < 3; k++)
            {
                int a = weld[head.Indices[t + k]], b = weld[head.Indices[t + (k + 1) % 3]];
                var key = (Math.Min(a, b), Math.Max(a, b));
                count[key] = count.GetValueOrDefault(key) + 1;
            }

        var open = count.Where(p => p.Value == 1).SelectMany(p => new[] { p.Key.Item1, p.Key.Item2 }).ToHashSet();
        return [.. Enumerable.Range(0, head.Positions.Length).Where(v => open.Contains(weld[v])).Select(v => head.Positions[v]).Distinct()];
    }

    /// <summary>
    /// A point's distance to the nearest of <paramref name="points"/>.
    /// </summary>
    static float Distance(Vector3 p, Vector3[] points) => points.Min(q => Vector3.Distance(p, q));

    /// <summary>
    /// The body vertices beyond the rim's band whose nearest human vertex is the head's.
    /// </summary>
    static bool[] Dropped(Vector3[] positions, HashSet<int> near, HumanBody human)
    {
        float low = Enumerable.Range(0, human.Positions.Length).Where(i => human.Uvs[i].Y < 0).Min(i => human.Positions[i].Y) - Band;
        var candidates = Enumerable.Range(0, human.Positions.Length).Where(i => human.Positions[i].Y >= low - Band).ToArray();
        var dropped = new bool[positions.Length];
        for (int v = 0; v < positions.Length; v++)
        {
            if (positions[v].Y < low || near.Contains(v)) continue;

            int nearest = candidates.MinBy(i => Vector3.DistanceSquared(positions[v], human.Positions[i]));
            dropped[v] = human.Uvs[nearest].Y < 0;
        }

        return dropped;
    }

    /// <summary>
    /// The body with each <paramref name="near"/> vertex moved <see cref="Sink"/> under the closest point of the human's skin, with the weights
    /// there (its triangle's corners' by barycentric share).
    /// </summary>
    static SkinnedMesh Sunk(SkinnedMesh body, HashSet<int> near, HumanBody human)
    {
        var positions = (Vector3[])body.Positions.Clone();
        var weights = body.Weights.Select(w => new Dictionary<ushort, float>(w)).ToArray();
        var triangles = NearTriangles(human, near.Select(v => body.Positions[v]).ToArray());
        foreach (int v in near)
        {
            var (point, normal, corners, shares) = Closest(body.Positions[v], human, triangles);
            positions[v] = point - normal * Sink;
            weights[v] = Blended(human, corners, shares);
        }

        return body with { Positions = positions, Weights = weights };
    }

    /// <summary>
    /// The human's triangles (by first index) with a corner within <see cref="Band"/> twice over of any of <paramref name="points"/>.
    /// </summary>
    static int[] NearTriangles(HumanBody human, Vector3[] points)
    {
        if (points.Length == 0) return [];

        var min = points.Aggregate(Vector3.Min) - new Vector3(2 * Band);
        var max = points.Aggregate(Vector3.Max) + new Vector3(2 * Band);
        bool Inside(int i) => human.Positions[i] == Vector3.Clamp(human.Positions[i], min, max);
        return [.. Enumerable.Range(0, human.Indices.Length / 3).Select(t => t * 3).Where(t => Inside(human.Indices[t]) || Inside(human.Indices[t + 1]) || Inside(human.Indices[t + 2]))];
    }

    /// <summary>
    /// The closest point to <paramref name="p"/> on the given human triangles: the point, the triangle's outward normal, its corners and their
    /// barycentric shares of the point.
    /// </summary>
    static (Vector3 Point, Vector3 Normal, int[] Corners, Vector3 Shares) Closest(Vector3 p, HumanBody human, int[] triangles)
    {
        var best = (Point: p, Normal: Vector3.UnitY, Corners: new int[3], Shares: Vector3.Zero, Distance: float.MaxValue);
        foreach (int t in triangles)
        {
            int[] corners = [human.Indices[t], human.Indices[t + 1], human.Indices[t + 2]];
            var (a, b, c) = (human.Positions[corners[0]], human.Positions[corners[1]], human.Positions[corners[2]]);
            var shares = Barycentric(p, a, b, c);
            var q = shares.X * a + shares.Y * b + shares.Z * c;
            float distance = Vector3.DistanceSquared(p, q);
            if (distance >= best.Distance) continue;

            var normal = Vector3.Cross(b - a, c - a);
            best = (q, normal == Vector3.Zero ? Vector3.UnitY : Vector3.Normalize(normal), corners, shares, distance);
        }

        return (best.Point, best.Normal, best.Corners, best.Shares);
    }

    /// <summary>
    /// The barycentric shares of the closest point to <paramref name="p"/> on triangle abc (Ericson, Real-Time Collision Detection 5.1.5).
    /// </summary>
    static Vector3 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a, ac = c - a, ap = p - a;
        float d1 = Vector3.Dot(ab, ap), d2 = Vector3.Dot(ac, ap);
        if (d1 <= 0 && d2 <= 0) return new Vector3(1, 0, 0);

        var bp = p - b;
        float d3 = Vector3.Dot(ab, bp), d4 = Vector3.Dot(ac, bp);
        if (d3 >= 0 && d4 <= d3) return new Vector3(0, 1, 0);

        float vc = d1 * d4 - d3 * d2;
        if (vc <= 0 && d1 >= 0 && d3 <= 0) return Edge(d1 / (d1 - d3), 0);

        var cp = p - c;
        float d5 = Vector3.Dot(ab, cp), d6 = Vector3.Dot(ac, cp);
        if (d6 >= 0 && d5 <= d6) return new Vector3(0, 0, 1);

        float vb = d5 * d2 - d1 * d6;
        if (vb <= 0 && d2 >= 0 && d6 <= 0) return Edge(d2 / (d2 - d6), 1);

        float va = d3 * d6 - d5 * d4;
        if (va <= 0 && d4 - d3 >= 0 && d5 - d6 >= 0) return Edge((d4 - d3) / (d4 - d3 + (d5 - d6)), 2);

        float denominator = 1 / (va + vb + vc);
        float v = vb * denominator, w = vc * denominator;
        return new Vector3(1 - v - w, v, w);
    }

    /// <summary>
    /// The shares of a point a share <paramref name="t"/> along edge ab (0), ac (1) or bc (2).
    /// </summary>
    static Vector3 Edge(float t, int edge) => edge switch
    {
        0 => new Vector3(1 - t, t, 0),
        1 => new Vector3(1 - t, 0, t),
        _ => new Vector3(0, 1 - t, t),
    };

    /// <summary>
    /// The weights of three human vertices blended by their shares.
    /// </summary>
    static Dictionary<ushort, float> Blended(HumanBody human, int[] corners, Vector3 shares)
    {
        var weights = new Dictionary<ushort, float>();
        float[] share = [shares.X, shares.Y, shares.Z];
        for (int k = 0; k < 3; k++)
            foreach (var (bone, weight) in human.Weights[corners[k]]) weights[bone] = weights.GetValueOrDefault(bone) + weight * share[k];

        return weights;
    }

    /// <summary>
    /// The kept body vertices (<paramref name="source"/>) and their whole triangles, renumbered, then the head's.
    /// </summary>
    static SkinnedMesh Joined(SkinnedMesh body, int[] source, HumanHead head)
    {
        var index = new int[body.Positions.Length];
        Array.Fill(index, -1);
        for (int i = 0; i < source.Length; i++) index[source[i]] = i;

        int offset = source.Length;
        var indices = new List<int>();
        for (int t = 0; t < body.Indices.Length; t += 3)
        {
            int a = index[body.Indices[t]], b = index[body.Indices[t + 1]], c = index[body.Indices[t + 2]];
            if (a >= 0 && b >= 0 && c >= 0) indices.AddRange([a, b, c]);
        }

        indices.AddRange(head.Indices.Select(i => i + offset));
        var poses = head.Poses.Select(p => p with { Offsets = [.. p.Offsets.Select(o => (o.Vertex + offset, o.Offset))] }).ToArray();
        return new SkinnedMesh(
            [.. source.Select(v => body.Positions[v]), .. head.Positions],
            [.. source.Select(v => body.Normals[v]), .. head.Normals],
            [.. source.Select(v => body.Uvs[v]), .. head.Uvs],
            [],
            [.. indices],
            [.. source.Select(v => body.Weights[v]), .. head.Weights.Select(w => new Dictionary<ushort, float>(w))])
        {
            Poses = poses,
        };
    }
}
