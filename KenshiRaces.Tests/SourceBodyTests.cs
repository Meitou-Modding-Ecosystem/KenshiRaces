using RaceImport;

namespace KenshiRaces.Tests;

/// <summary>
/// RaceImport's joint names (docs/race-import.md "What the model needs"): a Mixamo rig's body joints read as the Biped's.
/// </summary>
public sealed class SourceBodyTests
{
    [Theory]
    [InlineData("mixamorig:Hips", "Bip001 Pelvis")]
    [InlineData("mixamorig:LeftForeArm", "Bip001 L Forearm")]
    [InlineData("mixamorig:RightToeBase", "Bip001 R Toe0")]
    [InlineData("mixamorig:HeadTop_End", "mixamorig:HeadTop_End")]
    [InlineData("mixamorig:LeftHandIndex1", "mixamorig:LeftHandIndex1")]
    [InlineData("Bip001 Spine", "Bip001 Spine")]
    public void Mixamo_body_joints_take_their_Biped_names(string name, string biped) => Assert.Equal(biped, SourceBody.Biped(name));
}
