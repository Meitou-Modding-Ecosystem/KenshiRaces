namespace KenshiRaces;

/// <summary>
/// One clip of the animation library (<see cref="RaceAnimations"/>).
/// </summary>
/// <param name="Name">The clip's name in the animation files, e.g. <c>kr_ball_throw</c>: what the COMBAT_TECHNIQUE's <c>anim name</c> holds.</param>
/// <param name="Source">The source model's clip name (Meshy's animation library).</param>
/// <param name="Length">The clip's length in seconds.</param>
/// <param name="TechniqueId">The id of its disabled COMBAT_TECHNIQUE, the record a mod combat move plays.</param>
public sealed record RaceAnimation(string Name, string Source, float Length, string TechniqueId);
