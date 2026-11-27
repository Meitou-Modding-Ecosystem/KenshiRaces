using System.Numerics;
using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// The fit turns a model's head upright only forward and back: a head top the rig put far off the midline (an asymmetric head) loses its
/// sideways lean, a small lean stays.
/// </summary>
public sealed class HeadLeanTests
{
    [Fact]
    public void A_head_leaning_far_sideways_is_turned_upright_keeping_its_forward_lean()
    {
        var kittyCatlon = new Vector3(-0.106f, 0.294f, -0.111f);

        Assert.Equal(new Vector3(0, 0.294f, -0.111f), BodyFit.Upright(kittyCatlon));
    }

    [Fact]
    public void A_small_sideways_lean_stays()
    {
        var leonid = new Vector3(0.01f, 0.29f, 0.07f);

        Assert.Equal(leonid, BodyFit.Upright(leonid));
    }
}
