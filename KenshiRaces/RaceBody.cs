namespace KenshiRaces;

/// <summary>
/// A race whose body was fitted by the race import (docs/race-import.md): its name, the ids its records take, and where its files sit. The
/// files are <c>&lt;Stem&gt;.mesh</c>, <c>&lt;Stem&gt;_d.dds</c>, <c>&lt;Stem&gt;_n.dds</c> (with <see cref="NormalMap"/>) and
/// <c>&lt;Stem&gt;_parts.png</c> in the mod folder <see cref="Folder"/>'s <c>items/races</c>.
/// </summary>
/// <param name="Name">The race's name, also its CHARACTER's (e.g. "leonidmale").</param>
/// <param name="RaceId">The RACE record's string id, e.g. <c>10-KenshiRaces.mod</c>.</param>
/// <param name="CharacterId">The id of the CHARACTER of the race (null for none): what spawning by name and squads use.</param>
/// <param name="Folder">The folder of the mod holding the files, under the game's <c>mods</c> folder (or the Workshop's).</param>
/// <param name="Stem">The files' common name, e.g. <c>kenshiraces_leonidmale</c>; unique across mods, since the game finds files by bare name.</param>
/// <param name="NormalMap">Whether the body has a normal map.</param>
public sealed record RaceBody(string Name, string RaceId, string? CharacterId, string Folder, string Stem, bool NormalMap)
{
    /// <summary>
    /// A record's path to one of the body's files: <c>.\mods\&lt;Folder&gt;\items\races\&lt;Stem&gt;&lt;suffix&gt;</c>, which the game resolves into
    /// the mod's own folder wherever it is installed.
    /// </summary>
    public string File(string suffix) => $@".\mods\{Folder}\items\races\{Stem}{suffix}";

    /// <summary>
    /// The mesh's path.
    /// </summary>
    public string Mesh => File(".mesh");

    /// <summary>
    /// The colour texture's path.
    /// </summary>
    public string Texture => File("_d.dds");

    /// <summary>
    /// The normal map's path, empty without one.
    /// </summary>
    public string Normals => NormalMap ? File("_n.dds") : "";

    /// <summary>
    /// The part map's path (the body parts worn items hide).
    /// </summary>
    public string PartMap => File("_parts.png");
}
