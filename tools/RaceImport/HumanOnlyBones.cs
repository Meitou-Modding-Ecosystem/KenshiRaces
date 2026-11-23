using System.Numerics;

namespace RaceImport;

/// <summary>
/// Gives a fitted body the weights of the bones only the game's rig has (docs/race-import.md "Human-only bones"): the female skeleton's
/// <c>L Boob</c> and <c>R Boob</c>, which the bust sliders and the game's breathing move. The models' Biped rigs have no such bones, so a vertex
/// carried by the spine takes the share the nearest human vertex gives them, less with distance (feathers and hair hanging over the chest from
/// the neck and head keep their own).
/// </summary>
public static class HumanOnlyBones
{
    /// <summary>
    /// The prefix of the bones both rigs share; the rig's other bones are human-only.
    /// </summary>
    const string SharedPrefix = "Bip01";

    /// <summary>
    /// How far a fitted vertex looks for a human vertex to copy, in game units (0.1 m).
    /// </summary>
    const float Reach = 1;

    /// <summary>
    /// The bones whose vertices may take a share: the chest's.
    /// </summary>
    static readonly string[] Takers = ["Bip01 Spine1", "Bip01 Spine2"];

    /// <summary>
    /// The weights with each vertex's human-only share copied from the nearest human vertex (the vertex's own weights scaled to make room);
    /// returns how many vertices got some.
    /// </summary>
    public static int Apply(Vector3[] positions, Dictionary<ushort, float>[] weights, KenshiSkeleton kenshi, HumanBody human)
    {
        var donors = Enumerable.Range(0, human.Positions.Length).Where(v => HumanOnlyShare(human.Weights[v], kenshi) > 0).ToArray();
        if (donors.Length == 0) return 0;

        var takers = Takers.Select(kenshi.Handle).ToHashSet();
        int given = 0;
        for (int v = 0; v < positions.Length; v++)
        {
            if (!takers.Contains(weights[v].MaxBy(p => p.Value).Key)) continue;

            int nearest = donors.MinBy(d => Vector3.DistanceSquared(positions[v], human.Positions[d]));
            float distance = Vector3.Distance(positions[v], human.Positions[nearest]);
            if (distance > Reach) continue;

            Give(weights[v], human.Weights[nearest], 1 - distance / Reach, kenshi);
            given++;
        }

        return given;
    }

    /// <summary>
    /// Scales <paramref name="weights"/> down by the donor's human-only share times <paramref name="strength"/> and adds the donor's human-only
    /// weights times it.
    /// </summary>
    static void Give(Dictionary<ushort, float> weights, Dictionary<ushort, float> donor, float strength, KenshiSkeleton kenshi)
    {
        float share = strength * HumanOnlyShare(donor, kenshi);
        foreach (ushort bone in weights.Keys.ToArray()) weights[bone] *= 1 - share;
        foreach (var (bone, weight) in donor.Where(p => IsHumanOnly(p.Key, kenshi)))
            weights[bone] = weights.GetValueOrDefault(bone) + strength * weight;
    }

    /// <summary>
    /// The summed weight of a vertex's human-only bones.
    /// </summary>
    static float HumanOnlyShare(Dictionary<ushort, float> weights, KenshiSkeleton kenshi) =>
        weights.Where(p => IsHumanOnly(p.Key, kenshi)).Sum(p => p.Value);

    /// <summary>
    /// Whether the bone of that handle is one only the game's rig has.
    /// </summary>
    static bool IsHumanOnly(ushort handle, KenshiSkeleton kenshi) =>
        kenshi.Name(handle) is { } name && !name.StartsWith(SharedPrefix, StringComparison.Ordinal);
}
