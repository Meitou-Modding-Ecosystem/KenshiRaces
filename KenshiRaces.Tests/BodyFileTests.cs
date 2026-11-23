using Meitou.Data.Ogre;

namespace KenshiRaces.Tests;

/// <summary>
/// Every race's fitted files are committed where its records point (items/races, which Play and the release copy), and each mesh is skinned to
/// the game's human skeleton with at most the four bones a vertex blends.
/// </summary>
public sealed class BodyFileTests
{
    public static TheoryData<string> Stems => [.. Races.All.Select(r => r.Stem)];

    [Theory]
    [MemberData(nameof(Stems))]
    public void The_body_files_are_committed(string stem)
    {
        var body = Races.All.Single(r => r.Stem == stem);

        Assert.True(File.Exists(Path.Combine(Folder(), stem + ".mesh")));
        Assert.True(File.Exists(Path.Combine(Folder(), stem + "_d.dds")));
        Assert.True(File.Exists(Path.Combine(Folder(), stem + "_parts.png")));
        Assert.Equal(body.NormalMap, File.Exists(Path.Combine(Folder(), stem + "_n.dds")));
    }

    [Theory]
    [MemberData(nameof(Stems))]
    public void The_mesh_is_skinned_to_the_human_skeleton(string stem)
    {
        var mesh = OgreMeshReader.Read(File.ReadAllBytes(Path.Combine(Folder(), stem + ".mesh")));
        var sub = Assert.Single(mesh.SubMeshes);

        Assert.Equal("male_skeleton.skeleton", mesh.SkeletonName);
        Assert.Equal(sub.VertexData!.VertexCount, (uint)sub.BoneAssignments.Select(a => a.VertexIndex).Distinct().Count());
        Assert.True(sub.BoneAssignments.GroupBy(a => a.VertexIndex).All(g => g.Count() <= 4));
    }

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
