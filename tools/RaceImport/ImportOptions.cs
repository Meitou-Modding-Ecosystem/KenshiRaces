namespace RaceImport;

/// <summary>
/// How a body is imported (docs/race-import.md "Run it"): the rig it is fitted to, the slimming strength (0 none, 1 full), whose human's bust
/// height the bust rises to (null: the rig's own) and the neck's length as a share of the fitted one.
/// </summary>
public sealed record ImportOptions(HumanRig Rig, float Slim = 1, HumanRig? Bust = null, float Neck = 1)
{
    /// <summary>
    /// The options from the command line's optional arguments, in any order: <c>male</c> / <c>female</c>, a slimming strength,
    /// <c>bust=male|female</c> and <c>neck=&lt;share&gt;</c>; null for anything else.
    /// </summary>
    public static ImportOptions? Parse(IEnumerable<string> arguments)
    {
        var options = new ImportOptions(HumanRig.Male);
        foreach (string argument in arguments)
        {
            var next = Apply(options, argument);
            if (next is null) return null;

            options = next;
        }

        return options;
    }

    /// <summary>
    /// <paramref name="options"/> with one argument applied; null when it is not one.
    /// </summary>
    static ImportOptions? Apply(ImportOptions options, string argument)
    {
        if (HumanRig.Named(argument) is { } rig) return options with { Rig = rig };
        if (argument.StartsWith("bust=", StringComparison.Ordinal))
            return HumanRig.Named(argument[5..]) is { } bust ? options with { Bust = bust } : null;
        if (argument.StartsWith("neck=", StringComparison.Ordinal))
            return Number(argument[5..]) is { } neck ? options with { Neck = Math.Clamp(neck, 0.2f, 1.5f) } : null;

        return Number(argument) is { } slim ? options with { Slim = Math.Clamp(slim, 0, 1) } : null;
    }

    /// <summary>
    /// The invariant-culture number, or null.
    /// </summary>
    static float? Number(string text) =>
        float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value)
            ? value
            : null;
}
