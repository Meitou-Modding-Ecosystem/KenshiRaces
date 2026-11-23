using System.Numerics;
using Meitou.Data.Ogre;

namespace RaceImport;

/// <summary>
/// The game's own human body of a rig (<see cref="HumanRig.Body"/>, bound to the same skeleton) as the reference a fitted body is measured against: each
/// vertex's bind position, uv and heaviest bone. The game's clothing is modelled around it.
/// </summary>
public sealed class HumanBody
{
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
    /// Each vertex's bone weights by handle.
    /// </summary>
    public required Dictionary<ushort, float>[] Weights { get; init; }

    /// <summary>
    /// The body of <paramref name="rig"/> in the game in <paramref name="kenshiFolder"/>. Throws when it is missing or has no skinned geometry.
    /// </summary>
    public static HumanBody Load(string kenshiFolder, HumanRig rig)
    {
        var mesh = OgreMeshReader.Read(File.ReadAllBytes(KenshiSkeleton.GameFile(kenshiFolder, rig.Body)));
        var positions = new List<Vector3>();
        var uvs = new List<Vector2>();
        var weights = new List<Dictionary<ushort, float>>();
        foreach (var sub in mesh.SubMeshes) AddSubMesh(mesh, sub, positions, uvs, weights);

        if (positions.Count == 0) throw new InvalidDataException($"{rig.Body}: no vertices");

        return new HumanBody
        {
            Positions = [.. positions],
            Uvs = [.. uvs],
            Bones = [.. weights.Select(w => w.MaxBy(p => p.Value).Key)],
            Weights = [.. weights],
        };
    }

    /// <summary>
    /// Adds a submesh's skinned vertices with their bone weights (shared geometry is read once, with the first submesh using it).
    /// </summary>
    static void AddSubMesh(OgreMesh mesh, OgreSubMesh sub, List<Vector3> positions, List<Vector2> uvs, List<Dictionary<ushort, float>> weights)
    {
        var data = sub.UseSharedVertices ? mesh.SharedVertexData : sub.VertexData;
        if (data is null || (sub.UseSharedVertices && sub != mesh.SubMeshes.First(s => s.UseSharedVertices))) return;

        var assignments = sub.UseSharedVertices ? mesh.BoneAssignments : sub.BoneAssignments;
        var byVertex = assignments.GroupBy(a => a.VertexIndex).ToDictionary(g => g.Key, g => g.GroupBy(a => a.BoneIndex).ToDictionary(b => b.Key, b => b.Sum(a => a.Weight)));
        var points = data.ReadPositions() ?? [];
        var uvElement = data.Find(OgreVertexSemantic.TextureCoordinates);
        var uvFloats = uvElement is null ? null : data.ReadFloats(uvElement);
        int stride = uvElement?.Type.ComponentCount() ?? 0;
        for (uint v = 0; v < points.Length; v++)
        {
            if (!byVertex.TryGetValue(v, out var own)) continue;

            positions.Add(points[v]);
            uvs.Add(uvFloats is null ? Vector2.Zero : new Vector2(uvFloats[v * stride], uvFloats[v * stride + 1]));
            weights.Add(own);
        }
    }
}
