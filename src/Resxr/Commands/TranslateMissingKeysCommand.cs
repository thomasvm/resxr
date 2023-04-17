using Resxr.    Commands.Translation;
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
