using StructureImport;

namespace RaceImport;

/// <summary>
/// Softens a body's muscle definition (docs/race-import.md "Relief"): the normal map's slopes are scaled towards flat, and the skin's painted
/// shading keeps its broad light and dark but only a share of its finer detail (each texel's brightness against the mean of the skin round it).
/// </summary>
public static class Relief
{
    /// <summary>
    /// Half the side of the square whose skin mean a texel's brightness is measured against, in pixels at <see cref="BodyImporter.TextureSide"/>:
    /// about a muscle's width on the game's bodies, so abs and pecs fade while the torso's broad shading stays.
    /// </summary>
    public const int Radius = 48;

    /// <summary>
    /// <paramref name="normalImage"/> (a glTF tangent-space normal map) with its X and Y scaled towards 128 by <paramref name="relief"/> (1 keeps
    /// it, 0 flat); null stays null.
    /// </summary>
    public static byte[]? FlattenedNormal(byte[]? normalImage, float relief)
    {
        if (normalImage is null || relief >= 1) return normalImage;

        var image = Rgba.Decode(normalImage);
        for (int i = 0; i < image.Pixels.Length; i += 4)
        {
            image.Pixels[i] = Toward128(image.Pixels[i], relief);
            image.Pixels[i + 1] = Toward128(image.Pixels[i + 1], relief);
        }

        return PartMap.Png(image);
    }

    /// <summary>
    /// <paramref name="values"/> where <paramref name="mask"/> is set replaced by the masked mean of the <see cref="Radius"/> square round each plus
    /// <paramref name="relief"/> times its difference from that mean (<paramref name="side"/>-square images); other values unchanged.
    /// </summary>
    public static float[] Softened(float[] values, bool[] mask, int side, float relief)
    {
        if (relief >= 1) return values;

        var sums = Integral(side, i => mask[i] ? values[i] : 0);
        var counts = Integral(side, i => mask[i] ? 1 : 0);
        var result = (float[])values.Clone();
        for (int y = 0; y < side; y++)
            for (int x = 0; x < side; x++)
            {
                int i = y * side + x;
                if (!mask[i]) continue;

                double mean = Box(sums, side, x, y) / Box(counts, side, x, y);
                result[i] = (float)(mean + relief * (values[i] - mean));
            }

        return result;
    }

    /// <summary>
    /// A byte moved towards 128 by <paramref name="relief"/>.
    /// </summary>
    static byte Toward128(byte value, float relief) => (byte)Math.Clamp(MathF.Round(128 + relief * (value - 128)), 0, 255);

    /// <summary>
    /// The summed-area table of a <paramref name="side"/>-square image, one wider and higher (row and column 0 zero).
    /// </summary>
    static double[] Integral(int side, Func<int, double> value)
    {
        int stride = side + 1;
        var table = new double[stride * stride];
        for (int y = 0; y < side; y++)
        {
            double row = 0;
            for (int x = 0; x < side; x++)
            {
                row += value(y * side + x);
                table[(y + 1) * stride + x + 1] = table[y * stride + x + 1] + row;
            }
        }

        return table;
    }

    /// <summary>
    /// The sum over the <see cref="Radius"/> square round (<paramref name="x"/>, <paramref name="y"/>), clipped to the image.
    /// </summary>
    static double Box(double[] table, int side, int x, int y)
    {
        int stride = side + 1;
        int x0 = Math.Max(x - Radius, 0), x1 = Math.Min(x + Radius + 1, side);
        int y0 = Math.Max(y - Radius, 0), y1 = Math.Min(y + Radius + 1, side);
        return table[y1 * stride + x1] - table[y0 * stride + x1] - table[y1 * stride + x0] + table[y0 * stride + x0];
    }
}
