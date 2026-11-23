using System.Numerics;
using SharpGLTF.Schema2;

namespace RaceImport;

/// <summary>
/// The skinned body of a .glb in its rest pose as the file shows it (every vertex skinned by its joints' rest transforms): vertices, the
/// skin's joints with their rest positions, and the material's encoded images.
/// </summary>
public sealed class SourceBody
{
    /// <summary>
    /// Vertex positions in rest pose, model units.
    /// </summary>
    public required Vector3[] Positions { get; init; }

    /// <summary>
    /// Unit vertex normals in rest pose.
    /// </summary>
    public required Vector3[] Normals { get; init; }

    /// <summary>
    /// Texture coordinates, glTF's (v down from the top), which the game's meshes share.
    /// </summary>
    public required Vector2[] Uvs { get; init; }

    /// <summary>
    /// Triangles, three vertex indices each.
    /// </summary>
    public required int[] Indices { get; init; }

    /// <summary>
    /// Per vertex, its joints (indices into <see cref="JointNames"/>) and their weights, summing to 1 (all 0 for an unweighted vertex).
    /// </summary>
    public required (int Joint, float Weight)[][] Influences { get; init; }

    /// <summary>
    /// The skin's joint names, e.g. <c>Bip001 L Forearm</c>.
    /// </summary>
    public required string[] JointNames { get; init; }

    /// <summary>
    /// Each joint's parent (an index into <see cref="JointNames"/>), -1 for a root.
    /// </summary>
    public required int[] JointParents { get; init; }

    /// <summary>
    /// Each joint's rest position.
    /// </summary>
    public required Vector3[] JointPositions { get; init; }

    /// <summary>
    /// The base colour image (PNG or JPEG bytes), null without one.
    /// </summary>
    public byte[]? BaseColourImage { get; init; }

    /// <summary>
    /// The metallic-roughness image (roughness in green), null without one.
    /// </summary>
    public byte[]? MetallicRoughnessImage { get; init; }

    /// <summary>
    /// The tangent-space normal map (glTF's: +Y up), null without one.
    /// </summary>
    public byte[]? NormalImage { get; init; }

    /// <summary>
    /// The first skinned mesh of the file. Throws when it has none.
    /// </summary>
    public static SourceBody Load(string path)
    {
        var model = ModelRoot.Load(path);
        var node = model.LogicalNodes.FirstOrDefault(n => n.Mesh is not null && n.Skin is not null)
            ?? throw new InvalidDataException($"{path}: no skinned mesh");

        var skin = node.Skin;
        var joints = Enumerable.Range(0, skin.JointsCount).Select(skin.GetJoint).ToArray();
        var skinning = joints.Select(j => j.InverseBindMatrix * j.Joint.WorldMatrix).ToArray();
        var geometry = Geometry.Read(node.Mesh, node.WorldMatrix, skinning);
        var material = node.Mesh.Primitives[0].Material;

        return new SourceBody
        {
            Positions = [.. geometry.Positions],
            Normals = [.. geometry.Normals],
            Uvs = [.. geometry.Uvs],
            Indices = [.. geometry.Indices],
            Influences = [.. geometry.Influences],
            JointNames = [.. joints.Select(j => j.Joint.Name ?? "")],
            JointParents = Parents(joints.Select(j => j.Joint).ToArray()),
            JointPositions = [.. joints.Select(j => j.Joint.WorldMatrix.Translation)],
            BaseColourImage = Image(material?.FindChannel("BaseColor")),
            MetallicRoughnessImage = Image(material?.FindChannel("MetallicRoughness")),
            NormalImage = Image(material?.FindChannel("Normal")),
        };
    }

    /// <summary>
    /// Each joint's nearest ancestor among the joints, -1 for none.
    /// </summary>
    static int[] Parents(Node[] joints)
    {
        var index = joints.Select((n, i) => (n, i)).ToDictionary(p => p.n, p => p.i);
        var parents = new int[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            var up = joints[i].VisualParent;
            while (up is not null && !index.ContainsKey(up)) up = up.VisualParent;
            parents[i] = up is null ? -1 : index[up];
        }

        return parents;
    }

    /// <summary>
    /// The encoded image of a channel's texture, null when it has none.
    /// </summary>
    static byte[]? Image(MaterialChannel? channel) =>
        channel?.Texture?.PrimaryImage?.Content is { IsEmpty: false } image ? image.Content.ToArray() : null;

    /// <summary>
    /// A mesh's triangle primitives, concatenated and brought into rest pose.
    /// </summary>
    sealed class Geometry
    {
        /// <summary>
        /// Rest positions.
        /// </summary>
        public List<Vector3> Positions { get; } = [];

        /// <summary>
        /// Rest normals.
        /// </summary>
        public List<Vector3> Normals { get; } = [];

        /// <summary>
        /// Texture coordinates.
        /// </summary>
        public List<Vector2> Uvs { get; } = [];

        /// <summary>
        /// Triangle indices.
        /// </summary>
        public List<int> Indices { get; } = [];

        /// <summary>
        /// Normalised joint weights per vertex.
        /// </summary>
        public List<(int Joint, float Weight)[]> Influences { get; } = [];

        /// <summary>
        /// Reads every triangle primitive of <paramref name="mesh"/>: weighted vertices skinned by <paramref name="skinning"/> (inverse bind
        /// times joint rest), unweighted ones by the mesh node's own <paramref name="world"/>.
        /// </summary>
        public static Geometry Read(Mesh mesh, Matrix4x4 world, Matrix4x4[] skinning)
        {
            var geometry = new Geometry();
            foreach (var primitive in mesh.Primitives.Where(p => p.DrawPrimitiveType == PrimitiveType.TRIANGLES))
                geometry.Add(primitive, world, skinning);

            if (geometry.Indices.Count == 0) throw new InvalidDataException("the skinned mesh has no triangles");

            return geometry;
        }

        /// <summary>
        /// Appends one primitive.
        /// </summary>
        void Add(MeshPrimitive primitive, Matrix4x4 world, Matrix4x4[] skinning)
        {
            var positions = primitive.GetVertexAccessor("POSITION").AsVector3Array();
            var normals = primitive.GetVertexAccessor("NORMAL")?.AsVector3Array();
            var uvs = primitive.GetVertexAccessor("TEXCOORD_0")?.AsVector2Array();
            var influences = InfluencesOf(primitive, positions.Count);

            int first = Positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                var transform = Blend(influences[i], skinning, world);
                Positions.Add(Vector3.Transform(positions[i], transform));
                Normals.Add(Vector3.Normalize(Vector3.TransformNormal(normals?[i] ?? Vector3.UnitY, transform)));
                Uvs.Add(uvs?[i] ?? Vector2.Zero);
                Influences.Add(influences[i]);
            }

            foreach (var (a, b, c) in primitive.GetTriangleIndices())
            {
                Indices.Add(first + a);
                Indices.Add(first + b);
                Indices.Add(first + c);
            }
        }

        /// <summary>
        /// The weighted sum of the joints' skinning transforms, or <paramref name="world"/> for an unweighted vertex.
        /// </summary>
        static Matrix4x4 Blend((int Joint, float Weight)[] influences, Matrix4x4[] skinning, Matrix4x4 world)
        {
            if (influences.Length == 0) return world;

            var sum = new Matrix4x4();
            foreach (var (joint, weight) in influences) sum += skinning[joint] * weight;

            return sum;
        }

        /// <summary>
        /// Each vertex's joints and weights from every JOINTS_n / WEIGHTS_n pair, zero weights dropped and the rest normalised.
        /// </summary>
        static (int Joint, float Weight)[][] InfluencesOf(MeshPrimitive primitive, int count)
        {
            var lists = Enumerable.Range(0, count).Select(_ => new List<(int, float)>()).ToArray();
            for (int set = 0; primitive.GetVertexAccessor($"JOINTS_{set}") is { } jointAccessor; set++)
            {
                var joints = jointAccessor.AsVector4Array();
                var weights = primitive.GetVertexAccessor($"WEIGHTS_{set}").AsVector4Array();
                for (int i = 0; i < count; i++) AddSet(lists[i], joints[i], weights[i]);
            }

            return [.. lists.Select(Normalised)];
        }

        /// <summary>
        /// Adds a vertex's four joint/weight pairs of one set, skipping zero weights.
        /// </summary>
        static void AddSet(List<(int, float)> into, Vector4 joints, Vector4 weights)
        {
            for (int k = 0; k < 4; k++)
                if (weights[k] > 0) into.Add(((int)joints[k], weights[k]));
        }

        /// <summary>
        /// The pairs with weights scaled to sum to 1.
        /// </summary>
        static (int Joint, float Weight)[] Normalised(List<(int Joint, float Weight)> pairs)
        {
            float total = pairs.Sum(p => p.Weight);
            return [.. pairs.Select(p => (p.Joint, p.Weight / total))];
        }
    }
}
