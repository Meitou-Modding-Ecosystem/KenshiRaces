using Meitou.Data.Characters;
using Meitou.Data.Ogre;

namespace KenshiRaces.Tests;

/// <summary>
/// Every body's fitted files are committed where its records point (items/races, which Play and the release copy), and each mesh is skinned to
/// the game's skeleton of its gender (a single-gender subrace's one body: the male) with at most the four bones a vertex blends.
/// </summary>
public sealed class BodyFileTests
{
    public static TheoryData<string> Stems => [.. Bodies().Select(b => b.Body.Stem)];

    [Theory]
    [MemberData(nameof(Stems))]
    public void The_body_files_are_committed(string stem)
    {
        var body = Bodies().Single(b => b.Body.Stem == stem).Body;

        Assert.True(File.Exists(Path.Combine(Folder(), stem + ".mesh")));
        Assert.True(File.Exists(Path.Combine(Folder(), stem + "_d.dds")));
        Assert.True(File.Exists(Path.Combine(Folder(), stem + "_parts.png")));
        Assert.Equal(body.NormalMap, File.Exists(Path.Combine(Folder(), stem + "_n.dds")));
    }

    [Theory]
    [MemberData(nameof(Stems))]
    public void The_mesh_is_skinned_to_the_skeleton_of_its_gender(string stem)
    {
        bool female = Bodies().Single(b => b.Body.Stem == stem).Female;
        var mesh = OgreMeshReader.Read(File.ReadAllBytes(Path.Combine(Folder(), stem + ".mesh")));
        var sub = Assert.Single(mesh.SubMeshes);

        Assert.Equal(female ? "female_skeleton.skeleton" : "male_skeleton.skeleton", mesh.SkeletonName);
        Assert.Equal(sub.VertexData!.VertexCount, (uint)sub.BoneAssignments.Select(a => a.VertexIndex).Distinct().Count());
        Assert.True(sub.BoneAssignments.GroupBy(a => a.VertexIndex).All(g => g.Count() <= 4));
    }

    [Theory]
    [MemberData(nameof(Stems))]
    public void The_chest_and_neck_ride_the_bones_the_editor_sliders_scale(string stem)
    {
        bool female = Bodies().Single(b => b.Body.Stem == stem).Female;
        var mesh = OgreMeshReader.Read(File.ReadAllBytes(Path.Combine(Folder(), stem + ".mesh")));
        var weights = Assert.Single(mesh.SubMeshes).BoneAssignments.GroupBy(a => a.BoneIndex).ToDictionary(g => g.Key, g => g.Sum(a => a.Weight));

        // Handles of both rigs: Bip01 Spine2 14 and Neck 20; the female rig's L Boob 28 and R Boob 29 (docs/race-import.md "Human weights").
        Assert.True(weights.GetValueOrDefault((ushort)14) > 50, "Spine2 carries the chest");
        Assert.True(weights.GetValueOrDefault((ushort)20) > 5, "Neck carries the neck");
        if (female) Assert.True(weights.GetValueOrDefault((ushort)28) + weights.GetValueOrDefault((ushort)29) > 20, "the Boob bones carry the bust");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void The_editor_limits_hold_only_bone_sliders_that_leave_the_torso_shape(bool female)
    {
        var limits = AppearanceLimits.Read(Path.Combine(Folder(), "kenshiraces_editor.xml"), female, 0.2f);

        Assert.NotNull(limits);
        Assert.All(limits.Ranges, r => Assert.Null(r.Target));
        Assert.Contains(limits.Ranges, r => r.Name == "Height" && r.Category == AppearanceCategory.Body);
        string[] torso = ["Chest", "Stomach", "Waist", "Hips", "Legs bulk", "Legs shape", "Breast size", "Breast height", "Breast spacing"];
        Assert.DoesNotContain(limits.Ranges, r => torso.Contains(r.Name));
    }

    /// <summary>
    /// Every body of the mod's subraces with whether it is a female one.
    /// </summary>
    static IEnumerable<(RaceBody Body, bool Female)> Bodies() =>
        Races.All.SelectMany(s => new[] { (s.Male, false), (s.Female, true) }).Where(b => b.Item1 is not null).Select(b => (b.Item1!, b.Item2));

    /// <summary>
    /// The repo's KenshiRaces/items/races, found walking up from the test's output folder.
    /// </summary>
    static string Folder()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            string path = Path.Combine(dir.FullName, "KenshiRaces", "items", "races");
            if (Directory.Exists(path)) return path;
        }

        throw new DirectoryNotFoundException("No KenshiRaces/items/races above " + AppContext.BaseDirectory);
    }
}
