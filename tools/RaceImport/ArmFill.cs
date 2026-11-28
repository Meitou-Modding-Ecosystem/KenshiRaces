using System.Numerics;

namespace RaceImport;

/// <summary>
/// Widens arms thinner from side to side than the human's (docs/race-import.md "Arm fill"): around each upper arm and forearm, per sixth of
/// the bone, the fitted arm's side-to-side spread is scaled about its middle up to the human's, at most <see cref="MaxFill"/> times, and its
/// middle moved onto the human's (so the skin conform, which pulls what pokes out of the human's skin back, keeps the width), blended by the
/// vertex's weight on the bone. Arms hang down in the bind pose, so side to side is X.
/// </summary>
public static class ArmFill
{
    /// <summary>
    /// The arm bones widened, each with the joint its segment ends at.
    /// </summary>
    static readonly (string Bone, string End)[] Segments =
    [
        ("L UpperArm", "L Forearm"), ("L Forearm", "L Hand"), ("R UpperArm", "R Forearm"), ("R Forearm", "R Hand"),
    ];

    /// <summary>
    /// The slices along a bone the spread is measured in.
    /// </summary>
    const int Slices = 6;

    /// <summary>
    /// The most an arm is widened.
    /// </summary>
    const float MaxFill = 2;

    /// <summary>
    /// The widening over which a slice's middle moves all the way onto the human's (from none at no widening), so a slice barely widened
    /// barely moves.
    /// </summary>
    const float ShiftRamp = 0.25f;

    /// <summary>
    /// The fitted positions with the arms widened; <paramref name="spared"/> vertices (wing membranes) stay. Returns them and the largest
    /// factor applied (1 when no arm was thinner).
    /// </summary>
    public static (Vector3[] Positions, float Most) Apply(Vector3[] positions, Dictionary<ushort, float>[] weights, KenshiSkeleton kenshi,
        HumanBody human, bool[] spared)
    {
        var filled = (Vector3[])positions.Clone();
        float most = 1;
        foreach (var (bone, end) in Segments)
        {
            if (!kenshi.Has("Bip01 " + bone) || !kenshi.Has("Bip01 " + end)) continue;

            var segment = new Segment(kenshi.Position("Bip01 " + bone), kenshi.Position("Bip01 " + end));
            ushort handle = kenshi.Handle("Bip01 " + bone);
            var own = Spreads(segment, Enumerable.Range(0, positions.Length)
                .Where(v => !spared[v] && Heaviest(weights[v]) == handle)
                .Select(v => positions[v]));
            var theirs = Spreads(segment, Enumerable.Range(0, human.Positions.Length)
                .Where(v => human.Bones[v] == handle)
                .Select(v => human.Positions[v]));
            var factors = Factors(own, theirs);
            most = MathF.Max(most, factors.Max());
            Widen(filled, positions, weights, spared, handle, segment, own, theirs, factors);
        }

        return (filled, most);
    }

    /// <summary>
    /// A bone's segment, from its joint to the next.
    /// </summary>
    readonly record struct Segment(Vector3 From, Vector3 To)
    {
        /// <summary>
        /// Where along the segment a point lies, 0 at its joint and 1 at the next (unclamped).
        /// </summary>
        public float Along(Vector3 p)
        {
            var d = To - From;
            return Vector3.Dot(p - From, d) / d.LengthSquared();
        }

        /// <summary>
        /// A point's side-to-side offset from the segment's axis at its place along it.
        /// </summary>
        public float Across(Vector3 p) => p.X - Vector3.Lerp(From, To, Math.Clamp(Along(p), 0, 1)).X;
    }

    /// <summary>
    /// Each slice's least and greatest side-to-side offset of <paramref name="points"/> (NaN where none lies in it).
    /// </summary>
    static (float Low, float High)[] Spreads(Segment segment, IEnumerable<Vector3> points)
    {
        var spreads = Enumerable.Repeat((Low: float.NaN, High: float.NaN), Slices).ToArray();
        foreach (var p in points)
        {
            float t = segment.Along(p);
            if (t < 0 || t >= 1) continue;

            int s = (int)(t * Slices);
            float x = segment.Across(p);
            var (low, high) = spreads[s];
            spreads[s] = (float.IsNaN(low) ? x : MathF.Min(low, x), float.IsNaN(high) ? x : MathF.Max(high, x));
        }

        return spreads;
    }

    /// <summary>
    /// Each slice's widening: the human's spread over the arm's, 1 to <see cref="MaxFill"/> (1 where either has none).
    /// </summary>
    static float[] Factors((float Low, float High)[] own, (float Low, float High)[] theirs)
    {
        var factors = new float[Slices];
        for (int s = 0; s < Slices; s++)
        {
            float mine = own[s].High - own[s].Low, human = theirs[s].High - theirs[s].Low;
            factors[s] = float.IsNaN(mine) || float.IsNaN(human) || mine <= 1e-4f ? 1 : Math.Clamp(human / mine, 1, MaxFill);
        }

        return factors;
    }

    /// <summary>
    /// Moves each vertex weighted to the bone away from its slice's middle by the slice's widening, about the human's middle, interpolated
    /// between slice centres and scaled by its weight on the bone. A slice not widened stays where it is.
    /// </summary>
    static void Widen(Vector3[] filled, Vector3[] positions, Dictionary<ushort, float>[] weights, bool[] spared, ushort handle, Segment segment,
        (float Low, float High)[] own, (float Low, float High)[] theirs, float[] factors)
    {
        for (int v = 0; v < positions.Length; v++)
        {
            if (spared[v] || !weights[v].TryGetValue(handle, out float weight)) continue;

            var p = positions[v];
            float t = segment.Along(p);
            if (t < 0 || t > 1) continue;

            var (factor, middle, target) = Sample(t, own, theirs, factors);
            float across = segment.Across(p);
            float moved = Lerp(middle, target, Math.Min((factor - 1) / ShiftRamp, 1)) + factor * (across - middle);
            filled[v].X += weight * (moved - across);
        }
    }

    /// <summary>
    /// The widening, the arm's spread's middle and the human's at <paramref name="t"/> along the bone, linear between the slices' centres.
    /// </summary>
    static (float Factor, float Middle, float Target) Sample(float t, (float Low, float High)[] own, (float Low, float High)[] theirs,
        float[] factors)
    {
        float at = Math.Clamp(t * Slices - 0.5f, 0, Slices - 1);
        int a = (int)at, b = Math.Min(a + 1, Slices - 1);
        float f = at - a;
        float middle = Lerp(Middle(own[a]), Middle(own[b]), f);
        float target = Lerp(Target(own[a], theirs[a]), Target(own[b], theirs[b]), f);
        return (Lerp(factors[a], factors[b], f), middle, target);
    }

    /// <summary>
    /// The human's spread's middle, or the arm's own where the human has none.
    /// </summary>
    static float Target((float Low, float High) own, (float Low, float High) theirs) => float.IsNaN(theirs.Low) ? Middle(own) : Middle(theirs);

    /// <summary>
    /// A spread's middle, 0 where it has none.
    /// </summary>
    static float Middle((float Low, float High) spread) => float.IsNaN(spread.Low) ? 0 : (spread.Low + spread.High) / 2;

    /// <summary>
    /// Linear interpolation.
    /// </summary>
    static float Lerp(float a, float b, float f) => a + f * (b - a);

    /// <summary>
    /// The bone that carries most of a vertex's weight (none for an unweighted vertex).
    /// </summary>
    static ushort Heaviest(Dictionary<ushort, float> weights) => weights.Count == 0 ? ushort.MaxValue : weights.MaxBy(w => w.Value).Key;
}
