using System.CommandLine;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resxr.Commands;

namespace Resxr
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        services.AddScoped<GeneratorCommand>();
                        services.AddScoped<TranslateMissingKeysCommand>();
                        services.AddHttpClient();
                    }
                )
                .UseConsoleLifetime();

            var host = builder.Build();

            var fileArgument = new Argument<FileInfo>(
                name: "file",
                description: "The root .resx file to work with"
            );

            var rootCommand = new RootCommand(
                "Resxr, work with .resx files from the comfort of your terminal."
            );

            // 1. Generate designer.cs file
            var generateCommand = new Command(
                "generate",
                "Generate a .Designer.cs file based an input resx"
            )
            {
                fileArgument,
            };

            generateCommand.SetHandler(
                async (file) =>
                {
                    var command = host.Services.GetService<GeneratorCommand>();
                    await command.Invoke(file);
                },
                fileArgument
            );
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
            var translateCommand = new Command(
                "translate",
                "Updates language .resx file to include missing keys and to translate them"
            )
            {
                cultureOption,
                providerOption,
                fileArgument,
            };
            translateCommand.SetHandler(
                async (file, culture, provider) =>
                {
                    var command = host.Services.GetService<TranslateMissingKeysCommand>();
                    await command.InvokeAsync(file, culture, provider);
                },
                fileArgument,
                cultureOption,
                providerOption
            );
            rootCommand.Add(translateCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
