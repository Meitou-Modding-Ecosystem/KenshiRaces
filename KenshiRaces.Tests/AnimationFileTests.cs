using Meitou.Data.Ogre;

namespace KenshiRaces.Tests;

/// <summary>
/// The animation library's files are committed where its ANIMATION_FILE points, each holding every clip of <see cref="RaceAnimations"/> at its
/// length on its skeleton's bones, and the clips' names and technique ids are unique.
/// </summary>
public sealed class AnimationFileTests
{
    [Theory]
    [InlineData("kenshiraces_anims_male.skeleton", 30)]
    [InlineData("kenshiraces_anims_female.skeleton", 30)]
    public void A_file_holds_every_clip_at_its_length(string file, int bones)
    {
        var skeleton = OgreSkeletonReader.Read(File.ReadAllBytes(Path.Combine(Folder(), file)));

        Assert.Equal(bones, skeleton.Bones.Count);
        foreach (var clip in RaceAnimations.All)
        {
            var animation = Assert.Single(skeleton.Animations, a => a.Name == clip.Name);
            Assert.Equal(clip.Length, animation.Length, 0.01f);
        }
    }

    [Fact]
    public void The_record_paths_name_the_committed_files()
    {
        Assert.EndsWith(@"\items\animations\kenshiraces_anims_male.skeleton", AnimationRecords.MaleFile);
        Assert.EndsWith(@"\items\animations\kenshiraces_anims_female.skeleton", AnimationRecords.FemaleFile);
    }

    [Fact]
    public void Clip_names_and_technique_ids_are_unique()
    {
        Assert.Equal(RaceAnimations.All.Count, RaceAnimations.All.Select(a => a.Name).Distinct().Count());
        Assert.Equal(RaceAnimations.All.Count, RaceAnimations.All.Select(a => a.TechniqueId).Distinct().Count());
        Assert.All(RaceAnimations.All, a => Assert.StartsWith("kr_", a.Name));
    }

    /// <summary>
    /// The repo's KenshiRaces/items/animations, found walking up from the test's output folder.
    /// </summary>
    static string Folder()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            string path = Path.Combine(dir.FullName, "KenshiRaces", "items", "animations");
            if (Directory.Exists(path)) return path;
        }

        throw new DirectoryNotFoundException("No KenshiRaces/items/animations above " + AppContext.BaseDirectory);
    }
}
