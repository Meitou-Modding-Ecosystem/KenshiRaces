using System.Numerics;

namespace RaceImport;

/// <summary>
/// Fits a Biped (<c>Bip001 ...</c>) body onto the game's skeleton (docs/race-import.md "How the fit works"): the model is scaled by head height,
/// each limb bone is turned to point where the game's does and stretched to its length, the torso is fitted as one rigid piece, and the feet
/// are squashed so the soles land on the ground. Each vertex follows its own joints, a joint the game lacks following its nearest ancestor.
/// </summary>
public sealed class BodyFit
{
    #region Rig

    /// <summary>
    /// The model's joint names start with this; the rest of the name is the game's bone after <see cref="KenshiPrefix"/>.
    /// </summary>
    const string SourcePrefix = "Bip001 ";

    /// <summary>
    /// The game's Biped bone names start with this.
    /// </summary>
    const string KenshiPrefix = "Bip01 ";

    /// <summary>
    /// Each limb or spine bone's direction: towards this child, named the same in both rigs unless a pair (model child, game child) is given.
    /// </summary>
    static readonly Dictionary<string, (string Source, string Kenshi)> Directions = new()
    {
        ["Spine"] = ("Spine1", "Spine1"),
        ["Spine1"] = ("Spine2", "Spine2"),
        ["Spine2"] = ("Neck", "Neck"),
        ["Neck"] = ("Head", "Head"),
        ["Head"] = ("mixamorig:HeadTop_End", "HeadNub"),
        ["L Clavicle"] = ("L UpperArm", "L UpperArm"),
        ["L UpperArm"] = ("L Forearm", "L Forearm"),
        ["L Forearm"] = ("L Hand", "L Hand"),
        ["R Clavicle"] = ("R UpperArm", "R UpperArm"),
        ["R UpperArm"] = ("R Forearm", "R Forearm"),
        ["R Forearm"] = ("R Hand", "R Hand"),
        ["L Thigh"] = ("L Calf", "L Calf"),
        ["L Calf"] = ("L Foot", "L Foot"),
        ["L Foot"] = ("L Toe0", "L Toe0"),
        ["R Thigh"] = ("R Calf", "R Calf"),
        ["R Calf"] = ("R Foot", "R Foot"),
        ["R Foot"] = ("R Toe0", "R Toe0"),
    };

    /// <summary>
    /// Bones without a usable child in both rigs take their parent's rotation (null: none), unstretched.
    /// </summary>
    static readonly Dictionary<string, string?> Inherited = new()
    {
        ["L Hand"] = "L Forearm",
        ["R Hand"] = "R Forearm",
        ["L Toe0"] = "L Foot",
        ["R Toe0"] = "R Foot",
        ["Pelvis"] = null,
    };

    /// <summary>
    /// Bones fitted as one rigid torso, so the chest keeps its shape (per-bone fits shear it where spine and clavicle weights blend).
    /// </summary>
    static readonly string[] Torso = ["Pelvis", "Spine", "Spine1", "Spine2", "Neck", "L Clavicle", "R Clavicle"];

    /// <summary>
    /// The joints the torso is fitted to: its own, the shoulders and the hips.
    /// </summary>
    static readonly string[] TorsoAnchors = [.. Torso, "L UpperArm", "R UpperArm", "L Thigh", "R Thigh"];

    /// <summary>
    /// The least and most a bone is stretched.
    /// </summary>
    const float MinStretch = 0.6f, MaxStretch = 1.6f;

    /// <summary>
    /// Where the head is carried, as a share of the way from the fitted torso's neck joint to the game's head bone. The torso sits lower than
    /// the game's (the models' torsos are shorter), so a head on the game's head bone stretches the neck to about three times its length.
    /// </summary>
    const float NeckShare = 0.5f;

    #endregion

    #region Fit

    /// <summary>
    /// The model being fitted.
    /// </summary>
    readonly SourceBody _body;

    /// <summary>
    /// The game's skeleton.
    /// </summary>
    readonly KenshiSkeleton _kenshi;

    /// <summary>
    /// Model joint rest positions by short name (<c>L Forearm</c>; non-Biped joints by their full name).
    /// </summary>
    readonly Dictionary<string, Vector3> _source;

    /// <summary>
    /// The model's scale to the game's units: the game's head height over the model's (head joint above its lowest vertex).
    /// </summary>
    public float Scale { get; }

    /// <summary>
    /// The fit of each game bone the model has, by short name.
    /// </summary>
    public IReadOnlyDictionary<string, BoneFit> Fits { get; }

    /// <summary>
    /// Works out the fits of <paramref name="body"/> onto <paramref name="kenshi"/>. Throws when the model lacks a Biped joint the fit needs.
    /// </summary>
    public BodyFit(SourceBody body, KenshiSkeleton kenshi)
    {
        _body = body;
        _kenshi = kenshi;
        _source = body.JointNames.Select((n, i) => (Name: Short(n) ?? n, i)).GroupBy(p => p.Name).ToDictionary(g => g.Key, g => body.JointPositions[g.First().i]);
        Scale = HeadHeight(kenshi) / (Source("Head").Y - body.Positions.Min(p => p.Y));
        Fits = BuildFits();
    }

    /// <summary>
    /// The limb fits, then the torso's and the feet's, which replace those of their bones.
    /// </summary>
    Dictionary<string, BoneFit> BuildFits()
    {
        var fits = LimbFits();
        AddTorso(fits);
        AddHead(fits);
        AddFeet(fits);
        return fits;
    }

    /// <summary>
    /// Each directed or inherited bone the model and the game both have: turned onto the game's direction and stretched to its length.
    /// </summary>
    Dictionary<string, BoneFit> LimbFits()
    {
        var turns = new Dictionary<string, (Matrix4x4 Rotation, Vector3? Along, float Ratio)>();
        foreach (var (bone, (sourceChild, kenshiChild)) in Directions)
        {
            var source = Source(sourceChild) - Source(bone);
            var kenshi = Kenshi(kenshiChild) - Kenshi(bone);
            var rotation = Matrix4x4.CreateFromQuaternion(Between(Vector3.Normalize(source), Vector3.Normalize(kenshi)));
            turns[bone] = (rotation, Vector3.Normalize(source), kenshi.Length() / (Scale * source.Length()));
        }

        foreach (var (bone, parent) in Inherited) turns[bone] = (parent is null ? Matrix4x4.Identity : turns[parent].Rotation, null, 1);

        var fits = new Dictionary<string, BoneFit>();
        foreach (var (bone, (rotation, along, ratio)) in turns)
        {
            if (!_source.ContainsKey(bone) || !_kenshi.Has(KenshiPrefix + bone)) continue;
            fits[bone] = new BoneFit(Source(bone), Kenshi(bone), rotation, along, Math.Clamp(ratio, MinStretch, MaxStretch));
        }

        return fits;
    }

    /// <summary>
    /// One rigid fit (rotation and translation, scale fixed) for the torso bones, onto the game's torso, shoulder and hip joints.
    /// </summary>
    void AddTorso(Dictionary<string, BoneFit> fits)
    {
        var anchors = TorsoAnchors.Where(b => _source.ContainsKey(b) && _kenshi.Has(KenshiPrefix + b)).ToArray();
        var (rotation, translation) = RigidFit.Fit([.. anchors.Select(b => Scale * Source(b))], [.. anchors.Select(Kenshi)]);
        foreach (var bone in Torso.Where(anchors.Contains))
        {
            var target = Vector3.TransformNormal(Scale * Source(bone), rotation) + translation;
            fits[bone] = new BoneFit(Source(bone), target, rotation, null, 1);
        }
    }

    /// <summary>
    /// The head moved down to <see cref="NeckShare"/> of the way from the fitted neck joint to the game's head bone, its turn and stretch kept.
    /// </summary>
    void AddHead(Dictionary<string, BoneFit> fits)
    {
        if (!fits.TryGetValue("Head", out var head) || !fits.TryGetValue("Neck", out var neck)) return;

        fits["Head"] = head with { Target = Vector3.Lerp(neck.Target, head.Target, NeckShare) };
    }

    /// <summary>
    /// The feet kept upright and squashed so the sole lands on the ground (paws and crab feet are taller than the game's foot); the toes move
    /// with the foot.
    /// </summary>
    void AddFeet(Dictionary<string, BoneFit> fits)
    {
        foreach (var side in new[] { "L", "R" })
        {
            string foot = side + " Foot";
            float squash = Kenshi(foot).Y / (Scale * Source(foot).Y);
            var fit = new BoneFit(Source(foot), Kenshi(foot), Matrix4x4.CreateScale(1, squash, 1), null, 1);
            fits[foot] = fit;
            fits[side + " Toe0"] = fit;
        }
    }

    #endregion

    #region Repose

    /// <summary>
    /// The model's vertices and normals moved onto the game's skeleton, and each vertex's weights by game bone handle.
    /// </summary>
    public (Vector3[] Positions, Vector3[] Normals, Dictionary<ushort, float>[] Weights) Repose()
    {
        var follow = FollowedFits();
        int count = _body.Positions.Length;
        var positions = new Vector3[count];
        var normals = new Vector3[count];
        var weights = new Dictionary<ushort, float>[count];
        for (int v = 0; v < count; v++) (positions[v], normals[v], weights[v]) = ReposeVertex(v, follow);

        return (positions, normals, weights);
    }

    /// <summary>
    /// One vertex: the weighted sum of its joints' fits, and its weights gathered by the game bones those joints follow.
    /// </summary>
    (Vector3, Vector3, Dictionary<ushort, float>) ReposeVertex(int v, (string Bone, BoneFit Fit)[] follow)
    {
        var position = Vector3.Zero;
        var normal = Vector3.Zero;
        var weights = new Dictionary<ushort, float>();
        foreach (var (joint, weight) in _body.Influences[v])
        {
            var (bone, fit) = follow[joint];
            position += weight * fit.Apply(_body.Positions[v], Scale);
            normal += weight * fit.Apply(_body.Normals[v], Scale, isNormal: true);
            ushort handle = _kenshi.Handle(KenshiPrefix + bone);
            weights[handle] = weights.GetValueOrDefault(handle) + weight;
        }

        return (position, normal == Vector3.Zero ? Vector3.UnitY : Vector3.Normalize(normal), weights);
    }

    /// <summary>
    /// For each model joint, the game bone it follows and that bone's fit: itself, or its nearest ancestor that has a fit (fingers, tails and
    /// helper joints follow their limb). Throws when a joint has none.
    /// </summary>
    (string Bone, BoneFit Fit)[] FollowedFits()
    {
        var follow = new (string, BoneFit)[_body.JointNames.Length];
        for (int i = 0; i < follow.Length; i++)
        {
            int j = i;
            while (j >= 0 && !(Short(_body.JointNames[j]) is { } bone && Fits.ContainsKey(bone))) j = _body.JointParents[j];
            if (j < 0) throw new InvalidDataException($"joint {_body.JointNames[i]} has no fitted ancestor");

            string name = Short(_body.JointNames[j])!;
            follow[i] = (name, Fits[name]);
        }

        return follow;
    }

    #endregion

    #region Helpers

    /// <summary>
    /// A model joint's rest position by short name. Throws when the model lacks it.
    /// </summary>
    Vector3 Source(string name) =>
        _source.TryGetValue(name, out var p) ? p : throw new InvalidDataException($"the model has no joint {SourcePrefix}{name} (or {name})");

    /// <summary>
    /// A game bone's rest position by short name.
    /// </summary>
    Vector3 Kenshi(string name) => _kenshi.Position(KenshiPrefix + name);

    /// <summary>
    /// The game's head height: its head bone's height above the ground.
    /// </summary>
    static float HeadHeight(KenshiSkeleton kenshi) => kenshi.Position(KenshiPrefix + "Head").Y;

    /// <summary>
    /// A model joint's name without <see cref="SourcePrefix"/>, null for a joint that is not a Biped one.
    /// </summary>
    static string? Short(string name) => name.StartsWith(SourcePrefix, StringComparison.Ordinal) ? name[SourcePrefix.Length..] : null;

    /// <summary>
    /// The shortest rotation taking unit direction <paramref name="a"/> to unit direction <paramref name="b"/>.
    /// </summary>
    static Quaternion Between(Vector3 a, Vector3 b)
    {
        float d = Math.Clamp(Vector3.Dot(a, b), -1, 1);
        if (d > 1 - 1e-7f) return Quaternion.Identity;
        if (d < -1 + 1e-7f) return Quaternion.CreateFromAxisAngle(Perpendicular(a), MathF.PI);

        var axis = Vector3.Cross(a, b);
        return Quaternion.Normalize(new Quaternion(axis, 1 + d));
    }

    /// <summary>
    /// A unit direction perpendicular to <paramref name="a"/>.
    /// </summary>
    static Vector3 Perpendicular(Vector3 a)
    {
        var axis = Vector3.Cross(a, Vector3.UnitX);
        if (axis.Length() < 1e-6f) axis = Vector3.Cross(a, Vector3.UnitY);
        return Vector3.Normalize(axis);
    }

    #endregion
}
