using Meitou.Data;
using Meitou.Data.Fcs;
using Meitou.Data.Gameplay.Combat;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// The animation library's records (docs/animations.md), added in the data phase: an ANIMATION_FILE naming the clip files (one per skeleton) in
/// items/animations, linked into the <c>animation files</c> of every race on the game's human skeletons, and a disabled COMBAT_TECHNIQUE per clip
/// (disabled, so no fight ever picks it) for mods to play as a mod combat move.
/// </summary>
public static class AnimationRecords
{
    /// <summary>
    /// The ANIMATION_FILE's id.
    /// </summary>
    public const string File = "40-" + Races.FileName;

    /// <summary>
    /// The clip files for male and female bodies (male_skeleton and female_skeleton bone handles), resolved into the mod's own folder.
    /// </summary>
    public const string MaleFile = $@".\mods\{Races.Folder}\items\animations\kenshiraces_anims_male.skeleton",
        FemaleFile = $@".\mods\{Races.Folder}\items\animations\kenshiraces_anims_female.skeleton";

    /// <summary>
    /// The folder of the game's human body meshes (humans, shek, hivers, skeletons, fishmen): every mesh in it uses male_skeleton or
    /// female_skeleton, except the Hive Queen's.
    /// </summary>
    const string HumanoidMeshes = "data/character/meshes/";

    /// <summary>
    /// The one mesh in <see cref="HumanoidMeshes"/> with a skeleton of its own.
    /// </summary>
    const string QueenMesh = "hive_queen.mesh";

    /// <summary>
    /// The folder fitted bodies sit in, in any mod (<see cref="RaceBody.File"/>).
    /// </summary>
    const string FittedBodies = "/items/races/";

    /// <summary>
    /// Adds the ANIMATION_FILE and every clip's COMBAT_TECHNIQUE, and links the file to every loaded race on the human skeletons. Races added in
    /// the same data phase get it from <see cref="RaceRecords.FillRace"/>.
    /// </summary>
    public static void Add(ModDataContext c)
    {
        var file = c.Add(File, FcsRecordType.AnimationFile, "Kenshi Races animations");
        file.Filenames["male animation"] = MaleFile;
        file.Filenames["female animation"] = FemaleFile;
        foreach (var clip in RaceAnimations.All) AddTechnique(c, clip);

        foreach (var race in c.Data.OfType(FcsRecordType.Race).Where(OnHumanSkeleton)) Link(c.Edit(race.StringId));
    }

    /// <summary>
    /// Adds the library's file to a race's <c>animation files</c> (a new race's, or an edit's: an edit adds the entry to the race's list).
    /// </summary>
    public static void Link(FcsRecord race) =>
        race.References["animation files"] = [.. race.References.GetValueOrDefault("animation files") ?? [], new FcsReference(File, 0, 0, 0)];

    /// <summary>
    /// The clip's technique as the combat and animation code takes it, from the loaded data (<c>ModContext.Data</c>); null when Kenshi Races'
    /// records are not loaded. Answer it from an <c>AddCombatMove</c> source as a <c>CombatMove</c> with the clip's progress.
    /// </summary>
    public static CombatTechnique? Technique(GameDatabase data, RaceAnimation clip) =>
        data.Find(clip.TechniqueId) is { } record ? CombatTechnique.From(record) : null;

    /// <summary>
    /// A clip's disabled COMBAT_TECHNIQUE.
    /// </summary>
    static void AddTechnique(ModDataContext c, RaceAnimation clip)
    {
        var technique = c.Add(clip.TechniqueId, FcsRecordType.CombatTechnique, "Kenshi Races " + clip.Name);
        technique.Strings["anim name"] = clip.Name;
        technique.Bools["disabled"] = true;
    }

    /// <summary>
    /// Whether a race's body is on the game's human skeletons: one of the game's human meshes, or a fitted body.
    /// </summary>
    static bool OnHumanSkeleton(GameRecord race)
    {
        string mesh = race.GetPath("male mesh").Replace('\\', '/').TrimStart('.', '/').ToLowerInvariant();
        bool human = mesh.StartsWith(HumanoidMeshes, StringComparison.Ordinal) && !mesh.EndsWith(QueenMesh, StringComparison.Ordinal);
        return human || mesh.Contains(FittedBodies, StringComparison.Ordinal);
    }
}
