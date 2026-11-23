using System.Numerics;

namespace RaceImport;

/// <summary>
/// Raises a female body's bust to the game's female human's height (docs/race-import.md "Bust"): the models sit it well below the human's, so
/// the clothing's chest and the part map's bust region ride above it. The bust is the most forward point of the chest between the waist
/// (<c>Spine1</c>) and the shoulders; the front of the torso moves up by the difference, fading out above, below and towards the back.
/// </summary>
public static class BustLift
{
    /// <summary>
    /// Bones whose vertices move with the bust (the torso's); arm and leg vertices stay.
    /// </summary>
    static readonly string[] TorsoBones = ["Pelvis", "Spine", "Spine1", "Spine2", "Neck", "L Clavicle", "R Clavicle"];

    /// <summary>
    /// How far the chest's sides reach from the middle, in game units (0.1 m): the bust is looked for between these.
    /// </summary>
    const float Inner = 0.3f, Outer = 1.8f;

    /// <summary>
    /// The height of the slices the chest's forward profile is taken in.
    /// </summary>
    const float Slice = 0.25f;

    /// <summary>
    /// How far above and below the bust the lift reaches (the Gaussian's width): wide enough that no vertex overtakes the one above it.
    /// </summary>
    const float Reach = 1.5f;

    /// <summary>
    /// The fitted positions with the bust raised to <paramref name="human"/>'s; returns them and the lift (0 when the bust is already as high).
    /// </summary>
    public static (Vector3[] Positions, float Lift) Apply(Vector3[] positions, Dictionary<ushort, float>[] weights, KenshiSkeleton kenshi,
        HumanBody human)
    {
        float low = kenshi.Position("Bip01 Spine1").Y, high = kenshi.Position("Bip01 L UpperArm").Y;
        var bust = Apex(positions, low, high);
        float lift = Apex(human.Positions, low, high).Height - bust.Height;
        if (lift <= 0) return (positions, 0);

        float back = kenshi.Position("Bip01 Spine2").Z;
        var torso = TorsoBones.Select(b => kenshi.Handle("Bip01 " + b)).ToHashSet();
        var lifted = new Vector3[positions.Length];
        for (int v = 0; v < positions.Length; v++)
        {
            var p = positions[v];
            float share = weights[v].Where(w => torso.Contains(w.Key)).Sum(w => w.Value);
            float around = MathF.Exp(-Square((p.Y - bust.Height) / Reach));
            float front = SmoothStep(back, bust.Depth, p.Z);
            lifted[v] = p with { Y = p.Y + lift * share * around * front };
        }

        return (lifted, lift);
    }

    /// <summary>
    /// The chest's most forward point between heights <paramref name="low"/> and <paramref name="high"/>: the middle of the slice whose
    /// frontmost vertex (beside the middle line, inside the chest's sides) is furthest forward, at that vertex's depth.
    /// </summary>
    static (float Height, float Depth) Apex(IReadOnlyList<Vector3> points, float low, float high)
    {
        int slices = (int)MathF.Ceiling((high - low) / Slice);
        var front = Enumerable.Repeat(float.MinValue, slices).ToArray();
        foreach (var p in points)
        {
            float side = MathF.Abs(p.X);
            if (side < Inner || side > Outer || p.Y < low || p.Y >= high) continue;

            int s = (int)((p.Y - low) / Slice);
            front[s] = MathF.Max(front[s], p.Z);
        }

        int best = Array.IndexOf(front, front.Max());
        return (low + (best + 0.5f) * Slice, front[best]);
    }

    /// <summary>
    /// 0 below <paramref name="from"/>, 1 above <paramref name="to"/>, smooth between.
    /// </summary>
    static float SmoothStep(float from, float to, float x)
    {
        float t = Math.Clamp((x - from) / (to - from), 0, 1);
        return t * t * (3 - 2 * t);
    }

    /// <summary>
    /// The square.
    /// </summary>
    static float Square(float x) => x * x;
}
