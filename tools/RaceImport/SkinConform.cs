using System.Numerics;

namespace RaceImport;

/// <summary>
/// Pulls the torso, hips, legs and feet onto the human body's skin, so the game's trousers, skirts, coats and boots (modelled around the human)
/// cover them (docs/race-import.md "Skin conform"). Runs after slimming, which only pulls to the human's widest extent per slice: a vertex outside
/// the human surface by more than a cloth margin moves onto it, unless it is far out (a tail, a claw), carried by the arms or head, or near a tail.
/// </summary>
public sealed class SkinConform
{
    /// <summary>
    /// The bones whose vertices are pulled (a vertex is pulled by its weight on them).
    /// </summary>
    static readonly string[] Bones =
    [
        "Bip01 Pelvis", "Bip01 Spine", "Bip01 Spine1", "Bip01 Spine2", "L Boob", "R Boob",
        "Bip01 L Thigh", "Bip01 L Calf", "Bip01 L Foot", "Bip01 L Toe0", "Bip01 R Thigh", "Bip01 R Calf", "Bip01 R Foot", "Bip01 R Toe0",
    ];

    /// <summary>
    /// Room left for tight cloth beyond the human skin (game units, 0.1 m).
    /// </summary>
    const float Margin = 0.05f;

    /// <summary>
    /// A vertex outside the skin by up to <see cref="Full"/> is pulled fully; the pull fades out to <see cref="Reach"/>, beyond which the vertex is
    /// a shape no clothing covers (a tail, a claw) and stays.
    /// </summary>
    const float Full = 1.0f, Reach = 1.6f;

    /// <summary>
    /// Half the width of the gap between the legs: a vertex below the pelvis joint and this close to the middle line stays, since the nearest
    /// skin is an inner thigh and pulling there folds what hangs between the legs (**Observed**, male Crevan: feathers turned inside out).
    /// </summary>
    const float Crotch = 0.6f;

    /// <summary>
    /// The human vertices searched for the nearest skin.
    /// </summary>
    const int Candidates = 6;

    /// <summary>
    /// Passes of pull and smoothing, and the smoothing rounds per pass: smoothing spreads a pass's pull, the next pass takes up what it undid.
    /// </summary>
    const int Passes = 3, SmoothingRounds = 3;

    /// <summary>
    /// The human body's vertices.
    /// </summary>
    readonly Vector3[] _skin;

    /// <summary>
    /// The human body's triangles, three indices each, wound so their normals point out.
    /// </summary>
    readonly int[] _triangles;

    /// <summary>
    /// Each human vertex's triangles (index of the first corner).
    /// </summary>
    readonly List<int>[] _around;

    /// <summary>
    /// The pulled bones' handles.
    /// </summary>
    readonly HashSet<ushort> _pulled;

    /// <summary>
    /// The height of the pelvis joint (<see cref="Crotch"/>).
    /// </summary>
    readonly float _pelvis;

    /// <summary>
    /// Prepares the pull onto <paramref name="human"/> for the bones of <paramref name="kenshi"/>.
    /// </summary>
    public SkinConform(KenshiSkeleton kenshi, HumanBody human)
    {
        _skin = human.Positions;
        _triangles = Outward(human.Positions, human.Indices);
        _around = [.. _skin.Select(_ => new List<int>())];
        for (int t = 0; t < _triangles.Length; t += 3)
            for (int k = 0; k < 3; k++) _around[_triangles[t + k]].Add(t);

        _pulled = Bones.Where(kenshi.Has).Select(kenshi.Handle).ToHashSet();
        _pelvis = kenshi.Position("Bip01 Pelvis").Y;
    }

    /// <summary>
    /// The positions pulled onto the skin over <see cref="Passes"/> passes, scaled by <paramref name="strength"/>; vertices in
    /// <paramref name="kept"/> (a tail's) and those near them stay. Returns how many vertices moved by more than a millimetre.
    /// </summary>
    public (Vector3[] Positions, int Moved) Apply(Vector3[] positions, Dictionary<ushort, float>[] weights, int[] indices, float strength, bool[] kept)
    {
        var share = weights.Select(w => w.Where(p => _pulled.Contains(p.Key)).Sum(p => p.Value)).ToArray();
        var current = positions;
        for (int pass = 0; pass < Passes; pass++)
        {
            var offsets = Pulls(current, share);
            BodyConform.Spare(offsets, current, kept);
            offsets = BodyConform.Smoothed(offsets, current, indices, SmoothingRounds);
            BodyConform.Spare(offsets, current, kept);
            current = [.. current.Select((p, v) => p + strength * offsets[v])];
        }

        int moved = positions.Where((p, v) => Vector3.Distance(p, current[v]) > 0.01f).Count();
        return (current, moved);
    }

    /// <summary>
    /// Each vertex's pull: towards the nearest skin by its distance outside it less the margin, faded by distance and scaled by its weight on the
    /// pulled bones.
    /// </summary>
    Vector3[] Pulls(Vector3[] positions, float[] share)
    {
        var offsets = new Vector3[positions.Length];
        Parallel.For(0, positions.Length, v =>
        {
            if (share[v] <= 0 || positions[v].Y < _pelvis && MathF.Abs(positions[v].X) < Crotch) return;

            var (closest, outside) = Nearest(positions[v]);
            float distance = Vector3.Distance(positions[v], closest);
            if (!outside || distance <= Margin || distance >= Reach) return;

            float fade = Math.Clamp((Reach - distance) / (Reach - Full), 0, 1);
            offsets[v] = (closest - positions[v]) * ((distance - Margin) / distance) * fade * share[v];
        });

        return offsets;
    }

    /// <summary>
    /// The nearest point of the skin to <paramref name="point"/> (on the triangles round its nearest human vertices) and whether the point lies
    /// outside the skin there.
    /// </summary>
    (Vector3 Closest, bool Outside) Nearest(Vector3 point)
    {
        var best = (Point: point, Distance: float.MaxValue, Outside: false);
        foreach (int vertex in NearestVertices(point))
            foreach (int t in _around[vertex])
            {
                var (a, b, c) = (_skin[_triangles[t]], _skin[_triangles[t + 1]], _skin[_triangles[t + 2]]);
                var q = ClosestOnTriangle(point, a, b, c);
                float d = Vector3.DistanceSquared(point, q);
                if (d >= best.Distance) continue;

                best = (q, d, Vector3.Dot(point - q, Vector3.Cross(b - a, c - a)) > 0);
            }

        return (best.Point, best.Outside);
    }

    /// <summary>
    /// The <see cref="Candidates"/> human vertices nearest <paramref name="point"/>.
    /// </summary>
    int[] NearestVertices(Vector3 point)
    {
        var best = new List<(int Index, float Squared)>(Candidates + 1);
        for (int v = 0; v < _skin.Length; v++)
        {
            float d = Vector3.DistanceSquared(point, _skin[v]);
            if (best.Count == Candidates && d >= best[^1].Squared) continue;

            int at = best.FindIndex(b => b.Squared > d);
            best.Insert(at < 0 ? best.Count : at, (v, d));
            if (best.Count > Candidates) best.RemoveAt(Candidates);
        }

        return [.. best.Select(b => b.Index)];
    }

    /// <summary>
    /// The triangles wound so their normals point out of the body: reversed when the mesh's signed volume is negative.
    /// </summary>
    static int[] Outward(Vector3[] positions, int[] indices)
    {
        float volume = 0;
        for (int t = 0; t < indices.Length; t += 3)
            volume += Vector3.Dot(positions[indices[t]], Vector3.Cross(positions[indices[t + 1]], positions[indices[t + 2]]));

        if (volume >= 0) return indices;

        var flipped = (int[])indices.Clone();
        for (int t = 0; t < flipped.Length; t += 3) (flipped[t + 1], flipped[t + 2]) = (flipped[t + 2], flipped[t + 1]);
        return flipped;
    }

    /// <summary>
    /// The point of triangle <paramref name="a"/>, <paramref name="b"/>, <paramref name="c"/> nearest <paramref name="p"/> (Ericson, Real-Time
    /// Collision Detection, 5.1.5).
    /// </summary>
    static Vector3 ClosestOnTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a, ac = c - a, ap = p - a;
        float d1 = Vector3.Dot(ab, ap), d2 = Vector3.Dot(ac, ap);
        if (d1 <= 0 && d2 <= 0) return a;

        Vector3 bp = p - b;
        float d3 = Vector3.Dot(ab, bp), d4 = Vector3.Dot(ac, bp);
        if (d3 >= 0 && d4 <= d3) return b;

        float vc = d1 * d4 - d3 * d2;
        if (vc <= 0 && d1 >= 0 && d3 <= 0) return a + d1 / (d1 - d3) * ab;

        Vector3 cp = p - c;
        float d5 = Vector3.Dot(ab, cp), d6 = Vector3.Dot(ac, cp);
        if (d6 >= 0 && d5 <= d6) return c;

        float vb = d5 * d2 - d1 * d6;
        if (vb <= 0 && d2 >= 0 && d6 <= 0) return a + d2 / (d2 - d6) * ac;

        float va = d3 * d6 - d5 * d4;
        if (va <= 0 && d4 - d3 >= 0 && d5 - d6 >= 0) return b + (d4 - d3) / (d4 - d3 + (d5 - d6)) * (c - b);

        float denominator = 1 / (va + vb + vc);
        return a + ab * (vb * denominator) + ac * (vc * denominator);
    }
}
