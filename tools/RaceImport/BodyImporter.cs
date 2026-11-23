using System.Numerics;
using StructureImport;

namespace RaceImport;

/// <summary>
/// One body import: load the model, fit it onto the game's skeleton, write the mesh and textures (docs/race-import.md).
/// </summary>
public static class BodyImporter
{
    /// <summary>
    /// Side of the written textures in pixels.
    /// </summary>
    public const int TextureSide = 2048;

    /// <summary>
    /// Imports <paramref name="model"/> as <c>&lt;stem&gt;.mesh</c>, <c>&lt;stem&gt;_d.dds</c>, (with a normal map) <c>&lt;stem&gt;_n.dds</c> and the
    /// part map <c>&lt;stem&gt;_parts.png</c> in <paramref name="outFolder"/>, slimmed by <paramref name="slim"/> (0 none, 1 full); returns a line
    /// for the log.
    /// </summary>
    public static string Import(string model, string kenshiFolder, string outFolder, string stem, float slim)
    {
        var body = SourceBody.Load(model);
        var kenshi = KenshiSkeleton.Load(kenshiFolder);
        var human = HumanBody.Load(kenshiFolder);
        var fit = new BodyFit(body, kenshi);
        var (fitted, normals, weights) = fit.Repose();
        var positions = new BodyConform(kenshi, human).Apply(fitted, weights, slim);
        var mesh = new SkinnedMesh(positions, normals, body.Uvs, Tangents(positions, normals, body.Uvs, body.Indices), body.Indices, weights);

        Directory.CreateDirectory(outFolder);
        SkinnedMeshWriter.Write(Path.Combine(outFolder, stem + ".mesh"), mesh, stem, KenshiSkeleton.FileName);
        bool normalMap = BodyTextures.Write(body, outFolder, stem, TextureSide);
        int parts = WritePartMap(kenshiFolder, Path.Combine(outFolder, stem + "_parts.png"), mesh, human);

        var min = positions.Aggregate(Vector3.Min);
        var max = positions.Aggregate(Vector3.Max);
        return $"Race body {stem}: {positions.Length} vertices, {body.Indices.Length / 3} triangles, scale {fit.Scale:0.00}, " +
            $"bounds {min:0.0} to {max:0.0}, normal map {(normalMap ? "yes" : "no")}, {parts} vertices in hideable parts";
    }

    /// <summary>
    /// The part map of the fitted mesh from the game's human one; returns how many vertices got a part.
    /// </summary>
    static int WritePartMap(string kenshiFolder, string path, SkinnedMesh mesh, HumanBody human)
    {
        var humanMap = Rgba.Decode(File.ReadAllBytes(KenshiSkeleton.GameFile(kenshiFolder, PartMap.HumanFile)));
        var heaviest = mesh.Weights.Select(w => w.MaxBy(p => p.Value).Key).ToArray();
        return PartMap.Write(path, mesh.Positions, mesh.Uvs, heaviest, human, humanMap);
    }

    /// <summary>
    /// Per-vertex tangents from the uv layout, made perpendicular to the normal, w the bitangent's handedness.
    /// </summary>
    public static Vector4[] Tangents(Vector3[] positions, Vector3[] normals, Vector2[] uvs, int[] indices)
    {
        var tangents = new Vector3[positions.Length];
        var bitangents = new Vector3[positions.Length];
        for (int t = 0; t < indices.Length; t += 3) AddTriangle(positions, uvs, indices.AsSpan(t, 3), tangents, bitangents);

        var result = new Vector4[positions.Length];
        for (int v = 0; v < positions.Length; v++)
        {
            var n = normals[v];
            var tangent = tangents[v] - n * Vector3.Dot(tangents[v], n);
            tangent = tangent.LengthSquared() > 0 ? Vector3.Normalize(tangent) : tangent;
            float handedness = Vector3.Dot(Vector3.Cross(n, tangent), bitangents[v]) < 0 ? -1 : 1;
            result[v] = new Vector4(tangent, handedness);
        }

        return result;
    }

    /// <summary>
    /// Adds one triangle's uv-space tangent and bitangent to its three vertices.
    /// </summary>
    static void AddTriangle(Vector3[] positions, Vector2[] uvs, ReadOnlySpan<int> tri, Vector3[] tangents, Vector3[] bitangents)
    {
        var e1 = positions[tri[1]] - positions[tri[0]];
        var e2 = positions[tri[2]] - positions[tri[0]];
        var d1 = uvs[tri[1]] - uvs[tri[0]];
        var d2 = uvs[tri[2]] - uvs[tri[0]];
        float det = d1.X * d2.Y - d2.X * d1.Y;
        float f = MathF.Abs(det) > 1e-12f ? 1 / det : 0;

        var tangent = f * (e1 * d2.Y - e2 * d1.Y);
        var bitangent = f * (e2 * d1.X - e1 * d2.X);
        foreach (int v in tri)
        {
            tangents[v] += tangent;
            bitangents[v] += bitangent;
        }
    }
}
