namespace KenshiRaces;

/// <summary>
/// One body fitted by the race import (docs/race-import.md): where its files sit. The files are <c>&lt;Stem&gt;.mesh</c>, <c>&lt;Stem&gt;_d.dds</c>,
/// <c>&lt;Stem&gt;_n.dds</c> (with <see cref="NormalMap"/>) and <c>&lt;Stem&gt;_parts.png</c> in the mod folder <see cref="Folder"/>'s
/// <c>items/races</c>. A male body is fitted to the game's male skeleton, a female one to its female skeleton.
/// </summary>
/// <param name="Folder">The folder of the mod holding the files, under the game's <c>mods</c> folder (or the Workshop's).</param>
/// <param name="Stem">The files' common name, e.g. <c>kenshiraces_leomamale</c>; unique across mods, since the game finds files by bare name.</param>
/// <param name="NormalMap">Whether the body has a normal map.</param>
/// <param name="HumanHead">Whether the body wears the human head (imported <c>headless</c>, docs/race-import.md "Headless"): the race then keeps the
/// Greenlander's heads, hair, face sliders and random faces.</param>
/// <param name="SkinTone">Whether the body's skin takes the character editor's skin tone with the head (imported <c>skintone</c>,
/// docs/race-import.md "Skin tone"): the race then has the body mask <c>&lt;Stem&gt;_mask.dds</c> and the tinted editor limits.</param>
public sealed record RaceBody(string Folder, string Stem, bool NormalMap, bool HumanHead = false, bool SkinTone = false)
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

    /// <summary>
    /// The skin-tone body mask's path (white where the skin takes the editor's skin tone), empty without <see cref="SkinTone"/>.
    /// </summary>
    public string Mask => SkinTone ? File("_mask.dds") : "";
}

/// <summary>
/// A CHARACTER of a subrace: what the console's <c>spawn</c> and squads use.
/// </summary>
/// <param name="Name">The name, one lowercase word (e.g. <c>leomamale</c>) so <c>spawn</c> takes it without quotes.</param>
/// <param name="Id">The CHARACTER record's string id.</param>
/// <param name="Female">Whether the character is female (it then wears the subrace's female body).</param>
public sealed record RaceCharacter(string Name, string Id, bool Female);

/// <summary>
/// A subrace: one RACE record (the editor's subrace) with a fitted body per gender it has, and the CHARACTERs that spawn as it. With both bodies
/// the editor's gender arrow switches between them; with one the race is single gender.
/// </summary>
/// <param name="Name">The RACE's name, one lowercase word (e.g. <c>leoma</c>).</param>
/// <param name="RaceId">The RACE record's string id, e.g. <c>10-KenshiRaces.mod</c>.</param>
/// <param name="Male">The male body, null for none.</param>
/// <param name="Female">The female body, null for none.</param>
/// <param name="Characters">The subrace's CHARACTERs.</param>
/// <param name="Traits">What sets it apart from a Greenlander (aptitudes, health, gear limits); null for none.</param>
public sealed record Subrace(string Name, string RaceId, RaceBody? Male, RaceBody? Female, IReadOnlyList<RaceCharacter> Characters,
    RaceTraits? Traits = null)
{
    /// <summary>
    /// Whether the race has one gender: the editor shows "-" for gender and every character wears the one body.
    /// </summary>
    public bool SingleGender => Male is null || Female is null;

    /// <summary>
    /// Whether the subrace's bodies wear the human head (<see cref="RaceBody.HumanHead"/>).
    /// </summary>
    public bool HumanHead => (Male ?? Female)?.HumanHead == true;

    /// <summary>
    /// The body a gender wears: its own, else the other's.
    /// </summary>
    public RaceBody Body(bool female) =>
        (female ? Female ?? Male : Male ?? Female) ?? throw new InvalidOperationException($"Subrace {Name} has no body.");
}

/// <summary>
/// A race as the character editor lists it (a RACE_GROUP): its name and its subraces. Other mods add subraces to it with
/// <see cref="RaceRecords.JoinGroup"/>.
/// </summary>
/// <param name="Name">The name the editor shows, e.g. <c>Leoma</c>.</param>
/// <param name="Id">The RACE_GROUP record's string id.</param>
/// <param name="Subraces">The subraces, in the editor's order.</param>
public sealed record RaceGroup(string Name, string Id, IReadOnlyList<Subrace> Subraces);
