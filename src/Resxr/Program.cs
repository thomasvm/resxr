using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.CommandLine;
using Resxr.Commands;
using System.CommandLine.Invocation;

namespace Resxr
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var fileOption = new Option<FileInfo>(
                name: "--file",
                description: "The root .resx file to work with"
            );

            var rootCommand = new RootCommand("Resxr, work with .resx files from the comfort of your terminal.");

            var generateCommand = new Command("generate", "Generate a .Designer.cs file based an input resx")
            {
                fileOption,
            };

            generateCommand.SetHandler(async (file) =>
            {
                var command = new GeneratorCommand();
                await command.Invoke(file);
            }, fileOption);
            rootCommand.Add(generateCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
