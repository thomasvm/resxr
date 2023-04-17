using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.CommandLine;
using Resxr.Commands;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Resxr.Commands.Translation;

namespace Resxr
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddScoped<GeneratorCommand>();
                   services.AddScoped<TranslateMissingKeysCommand>();
                   services.AddHttpClient();
               }).UseConsoleLifetime();

            var host = builder.Build();

            var fileOption = new Option<FileInfo>(
                name: "--file",
                description: "The root .resx file to work with"
            );

            var rootCommand = new RootCommand("Resxr, work with .resx files from the comfort of your terminal.");

            // 1. Generate designer.cs file
            var generateCommand = new Command("generate", "Generate a .Designer.cs file based an input resx")
            {
                fileOption,
            };

            generateCommand.SetHandler(async (file) =>
            {
                var command = host.Services.GetService<GeneratorCommand>();
                await command.Invoke(file);
            }, fileOption);
            rootCommand.Add(generateCommand);

            // 2. Add missing keys to 
            var cultureOption = new Option<string>(
                name: "--culture",
                description: "The culture to translate"
            );
            var providerOption = new Option<string>(
                name: "--provider",
                description: "The translation provider to use"
            );
            var translateCommand = new Command("translate", "Updates language .resx file to include missing keys and to translate them")
            {
                fileOption,
                cultureOption
            };
            translateCommand.SetHandler(async (file, culture, provider) =>
            {
                var command = host.Services.GetService<TranslateMissingKeysCommand>();
                await command.InvokeAsync(file, culture, provider);
            }, fileOption, cultureOption, providerOption);
            rootCommand.Add(translateCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
