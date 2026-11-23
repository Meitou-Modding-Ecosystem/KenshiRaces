using System.Numerics;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The race import's torso fit finds the rotation and translation that carry one point set onto another.
/// </summary>
public sealed class RigidFitTests
{
    [Fact]
    public void It_recovers_a_known_rotation_and_translation()
    {
        Vector3[] from = [new(0, 0, 0), new(1, 0, 0), new(0, 2, 0), new(0, 0, 3), new(1, 1, 1)];
        var rotation = Matrix4x4.CreateFromYawPitchRoll(0.7f, -0.3f, 1.1f);
        var shift = new Vector3(4, -2, 9);
        var to = from.Select(p => Vector3.TransformNormal(p, rotation) + shift).ToArray();

        var (fitted, translation) = RigidFit.Fit(from, to);

        for (int i = 0; i < from.Length; i++)
            Assert.True(Vector3.Distance(Vector3.TransformNormal(from[i], fitted) + translation, to[i]) < 1e-4f);
    }
}
