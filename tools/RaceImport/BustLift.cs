using System.Numerics;

namespace RaceImport;

/// <summary>
/// Raises a body's bust or chest to a human's height (docs/race-import.md "Bust"): the models sit it well below the human's, so the clothing's
/// chest and the part map's bust region ride above it. The bust is the most forward point of the chest between the waist
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
    /// The band that moves by the whole lift: from this far below the bust (its underside) to this far above it.
    /// </summary>
    const float Under = 0.8f, Over = 0.2f;

    /// <summary>
    /// The fades above and below the band are this many times the lift long, and at least <see cref="MinFade"/>: a smoothstep that steep
    /// stretches the chest below by at most 1.75 and squashes the chest above to no less than a quarter, never folding it (a narrow Gaussian
    /// did: Matthew saw a crease above and a cone below the bust, 2026-11-23).
    /// </summary>
    const float FadePerLift = 2, MinFade = 1.5f;

    /// <summary>
    /// The fitted positions with the bust raised to <paramref name="human"/>'s (the rig's own human, or the female's for a woman fitted to the male
    /// rig); returns them and the lift (0 when the bust is already as high).
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
            float around = Band(p.Y - bust.Height, MathF.Max(MinFade, FadePerLift * lift));
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
    /// The share of the lift at <paramref name="dy"/> from the bust: 1 in the band, falling smoothly to 0 over <paramref name="fade"/> on
    /// either side.
    /// </summary>
    static float Band(float dy, float fade)
    {
        if (dy > Over) return 1 - SmoothStep(Over, Over + fade, dy);
        if (dy < -Under) return SmoothStep(-Under - fade, -Under, dy);

        return 1;
    }
}
