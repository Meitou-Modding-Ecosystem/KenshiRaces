using BCnEncoder.Shared;
using StructureImport;

namespace RaceImport;

/// <summary>
/// A body's textures in the game's character layout (docs/race-import.md "Textures"): <c>_d</c> = base colour with gloss in alpha, <c>_n</c> =
/// the normal map with X in alpha and Y (flipped) in green; both BC3 with mips.
/// </summary>
public static class BodyTextures
{
    /// <summary>
    /// Roughness taken for a model without a metallic-roughness image (0 to 255).
    /// </summary>
    const byte DefaultRoughness = 200;

    /// <summary>
    /// Writes <c>&lt;stem&gt;_d.dds</c> and, when the model has a normal map, <c>&lt;stem&gt;_n.dds</c> at <paramref name="side"/> pixels square;
    /// returns whether the normal map was written. Throws when the model has no base colour image.
    /// </summary>
    public static bool Write(SourceBody body, string folder, string stem, int side)
    {
        var colour = Rgba.Decode(body.BaseColourImage ?? throw new InvalidDataException("the model has no base colour texture")).Resized(side, side);
        var metalRough = body.MetallicRoughnessImage is { } mr ? Rgba.Decode(mr).Resized(side, side) : null;
        DdsWriter.Write(Diffuse(colour, metalRough), CompressionFormat.Bc3, Path.Combine(folder, stem + "_d.dds"));
        if (body.NormalImage is null) return false;

        DdsWriter.Write(Normal(Rgba.Decode(body.NormalImage).Resized(side, side)), CompressionFormat.Bc3, Path.Combine(folder, stem + "_n.dds"));
        return true;
    }

    /// <summary>
    /// Base colour with gloss in alpha: half of (255 - roughness), roughness from the green channel.
    /// </summary>
    static Rgba Diffuse(Rgba colour, Rgba? metalRough)
    {
        var result = new Rgba(colour.Width, colour.Height);
        for (int i = 0; i < colour.Pixels.Length; i += 4)
        {
            int roughness = metalRough?.Pixels[i + 1] ?? DefaultRoughness;
            result.Pixels[i] = colour.Pixels[i];
            result.Pixels[i + 1] = colour.Pixels[i + 1];
            result.Pixels[i + 2] = colour.Pixels[i + 2];
            result.Pixels[i + 3] = (byte)((255 - roughness) / 2);
        }

        return result;
    }

    /// <summary>
    /// The game's character normal layout: X in alpha, Y in green, flipped from glTF's +Y up; red and blue zero.
    /// </summary>
    static Rgba Normal(Rgba normal)
    {
        var result = new Rgba(normal.Width, normal.Height);
        for (int i = 0; i < normal.Pixels.Length; i += 4)
        {
            result.Pixels[i + 1] = (byte)(255 - normal.Pixels[i + 1]);
            result.Pixels[i + 3] = normal.Pixels[i];
        }

        return result;
    }
}
