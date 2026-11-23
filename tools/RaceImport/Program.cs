using System.Globalization;
using RaceImport;

// Fits a Biped .glb body onto the game's human skeleton (docs/race-import.md). Arguments: the model, the game folder (its
// skeleton, human body and part map are read), the output folder, the files' stem (e.g. kenshiraces_leonid) and optionally the slimming
// strength (0 to 1, default 1).
if (args.Length is not (4 or 5) || (args.Length == 5 && !float.TryParse(args[4], CultureInfo.InvariantCulture, out _)))
{
    Console.Error.WriteLine("Usage: RaceImport <model.glb> <Kenshi folder> <output folder> <stem> [slim 0-1]");
    return 2;
}

float slim = args.Length == 5 ? Math.Clamp(float.Parse(args[4], CultureInfo.InvariantCulture), 0, 1) : 1;

try
{
    Console.WriteLine(BodyImporter.Import(args[0], args[1], args[2], args[3], slim));
    return 0;
}
catch (Exception e) when (e is InvalidDataException or IOException or KeyNotFoundException)
{
    Console.Error.WriteLine($"Race import failed: {e.Message}");
    return 1;
}
