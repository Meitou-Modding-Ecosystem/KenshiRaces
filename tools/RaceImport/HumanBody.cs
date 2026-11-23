using System.Numerics;
using Meitou.Data.Ogre;

namespace RaceImport;

/// <summary>
/// The game's own male body (<c>human_male.mesh</c>, bound to the same skeleton) as the reference a fitted body is measured against: each
/// vertex's bind position, uv and heaviest bone. The game's clothing is modelled around it.
/// </summary>
public sealed class HumanBody
{
    /// <summary>
    /// The mesh read.
    /// </summary>
    public const string FileName = "human_male.mesh";

    /// <summary>
    /// Bind positions.
    /// </summary>
    public required Vector3[] Positions { get; init; }

    /// <summary>
    /// Texture coordinates.
    /// </summary>
    public required Vector2[] Uvs { get; init; }

    /// <summary>
    /// Each vertex's heaviest bone handle.
    /// </summary>
    public required ushort[] Bones { get; init; }

    /// <summary>
    /// The body of the game in <paramref name="kenshiFolder"/>. Throws when it is missing or has no skinned geometry.
    /// </summary>
    public static HumanBody Load(string kenshiFolder)
    {
        var mesh = OgreMeshReader.Read(File.ReadAllBytes(KenshiSkeleton.GameFile(kenshiFolder, FileName)));
        var positions = new List<Vector3>();
        var uvs = new List<Vector2>();
        var bones = new List<ushort>();
        foreach (var sub in mesh.SubMeshes) AddSubMesh(mesh, sub, positions, uvs, bones);

        if (positions.Count == 0) throw new InvalidDataException($"{FileName}: no vertices");

        return new HumanBody { Positions = [.. positions], Uvs = [.. uvs], Bones = [.. bones] };
    }

    /// <summary>
    /// Adds a submesh's vertices with their heaviest bone (shared geometry is read once, with the first submesh using it).
    /// </summary>
    static void AddSubMesh(OgreMesh mesh, OgreSubMesh sub, List<Vector3> positions, List<Vector2> uvs, List<ushort> bones)
    {
        var data = sub.UseSharedVertices ? mesh.SharedVertexData : sub.VertexData;
        if (data is null || (sub.UseSharedVertices && sub != mesh.SubMeshes.First(s => s.UseSharedVertices))) return;

        var assignments = sub.UseSharedVertices ? mesh.BoneAssignments : sub.BoneAssignments;
        var heaviest = assignments.GroupBy(a => a.VertexIndex).ToDictionary(g => g.Key, g => g.MaxBy(a => a.Weight).BoneIndex);
        var points = data.ReadPositions() ?? [];
        var uvElement = data.Find(OgreVertexSemantic.TextureCoordinates);
        var uvFloats = uvElement is null ? null : data.ReadFloats(uvElement);
        int stride = uvElement?.Type.ComponentCount() ?? 0;
        for (uint v = 0; v < points.Length; v++)
        {
            if (!heaviest.TryGetValue(v, out ushort bone)) continue;

            positions.Add(points[v]);
            uvs.Add(uvFloats is null ? Vector2.Zero : new Vector2(uvFloats[v * stride], uvFloats[v * stride + 1]));
            bones.Add(bone);
        }
    }
}
