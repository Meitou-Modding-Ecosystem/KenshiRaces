using System.Globalization;
using RaceImport;

// Fits a Biped .glb body onto one of the game's human skeletons (docs/race-import.md). Arguments: the model, the game folder (its
// skeleton, human body and part map are read), the output folder, the files' stem (e.g. kenshiraces_leomamale), then optionally the rig
// (male or female, default male) and the slimming strength (0 to 1, default 1).
if (args.Length is < 4 or > 6 || !TryOptions(args.Skip(4), out var rig, out float slim))
{
    Console.Error.WriteLine("Usage: RaceImport <model.glb> <Kenshi folder> <output folder> <stem> [male|female] [slim 0-1]");
    return 2;
}

try
{
    Console.WriteLine(BodyImporter.Import(args[0], args[1], args[2], args[3], rig, slim));
    return 0;
}
catch (Exception e) when (e is InvalidDataException or IOException or KeyNotFoundException)
{
    Console.Error.WriteLine($"Race import failed: {e.Message}");
    return 1;
}

// The optional arguments: a rig name and a slimming strength, in any order; false for anything else.
static bool TryOptions(IEnumerable<string> options, out HumanRig rig, out float slim)
{
    rig = HumanRig.Male;
    slim = 1;
    foreach (string option in options)
    {
        if (HumanRig.Named(option) is { } named) rig = named;
        else if (float.TryParse(option, CultureInfo.InvariantCulture, out float value)) slim = Math.Clamp(value, 0, 1);
        else return false;
    }

    return true;
}
