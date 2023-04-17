using Resxr.Commands.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Resxr.Commands
{
    public class TranslateMissingKeysCommand
    {
        private IHttpClientFactory _httpFactory;

        public TranslateMissingKeysCommand(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task InvokeAsync(FileInfo file, string culture, string provider)
        {
            var translator = GetTranslationService(provider);

            var source = File.ReadAllText(file.FullName);

            var input = new ResxWriter(source);

            var cultureFilename = $"{Path.GetFileNameWithoutExtension(file.Name)}.{culture}.resx";

            var outputFilename = Path.Combine(file.DirectoryName, cultureFilename);

            string currentOutput = null;

            if (File.Exists(outputFilename))
                currentOutput = await File.ReadAllTextAsync(outputFilename);

            var output = new ResxWriter(currentOutput);

            foreach ((var key, var value) in input.GetValues())
            {
                if (output.Contains(key))
                    continue;

                var translated = await translator.TranslateAsync(new TranslationInput
                {
                    TargetLanguage = culture,
                    Text = value,
                });

                output.Set(key, translated.Output);
            }

            var export = output.Export();

            File.WriteAllText(outputFilename, export);
        }

        private ITranslationService GetTranslationService(string provider)
        {
            if (provider == "google")
                return new GoogleTranslationService(_httpFactory);

            if (provider == "deepl" || string.IsNullOrEmpty(provider))
                return new DeeplTranslationService();

            throw new NotSupportedException($"Provider {provider} is not supported");
        }
    }
}
