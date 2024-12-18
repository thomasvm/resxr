using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Resxr.Commands;

public class ReportMissingKeysCommand
{
    public async Task InvokeAsync(FileInfo file, string culture)
    {
        var source = await File.ReadAllTextAsync(file.FullName);

        var input = new ResxWriter(source);

        var cultureFilename = $"{Path.GetFileNameWithoutExtension(file.Name)}.{culture}.resx";

        var outputFilename = Path.Combine(file.DirectoryName, cultureFilename);

        if (!File.Exists(outputFilename))
            throw new NotSupportedException(
                $"The requested target culture resx {cultureFilename} does not exist"
            );

        var target = await File.ReadAllTextAsync(outputFilename);
        var output = new ResxWriter(target);

        var inputKeys = input.GetValues().Select((value) => value.Item1).ToArray();

        var outputKeys = output.GetValues().Select((value) => value.Item1).ToArray();

        var missingKeysInTargetFile = inputKeys.Except(outputKeys).ToArray();

        Log(
            $"The following keys exist in {file.Name} but not in {cultureFilename}:",
            missingKeysInTargetFile
        );

        var missingKeysInSourceFile = outputKeys.Except(inputKeys).ToArray();

        Log(
            $"The following keys exist in {cultureFilename} but not in {file.Name}:",
            missingKeysInSourceFile
        );
    }

    private void Log(string msg, string[] keys)
    {
        if (!keys.Any())
            return;

        Console.WriteLine(msg);
        foreach (var key in keys)
            Console.WriteLine($"  {key}");
    }
}
