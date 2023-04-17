using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resxr.Commands.Translation
{
    public class DeeplTranslationService : ITranslationService
    {
        private readonly string _apiKey;

        private readonly DeepL.Translator _translator;

        public DeeplTranslationService()
        {
            const string ENV_NAME = "RESXR_DEEPL_KEY";

            _apiKey = Environment.GetEnvironmentVariable(ENV_NAME);

            if (string.IsNullOrEmpty(_apiKey))
                throw new InvalidOperationException($"You must set {ENV_NAME} to be able to use Deepl provider");

            _translator = new DeepL.Translator(_apiKey);
        }

        public DeeplTranslationService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<TranslationOutput> TranslateAsync(TranslationInput input)
        {
            var result = await _translator.TranslateTextAsync(input.Text, "en", input.TargetLanguage);

            return new TranslationOutput
            {
                Output = result.Text,
                DetectedLanguage = result.DetectedSourceLanguageCode,
            };
        }
    }
}
