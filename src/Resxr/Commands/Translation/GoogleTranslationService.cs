﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Resxr.Commands.Translation
{
    public class GoogleTranslationService : ITranslationService
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _apiKey;

        public GoogleTranslationService(IHttpClientFactory factory)
        {
            const string ENV_NAME = "RESXR_GOOGLE_TRANSLATE_KEY";

            _factory = factory;

            _apiKey = Environment.GetEnvironmentVariable(ENV_NAME);

            if (string.IsNullOrEmpty(_apiKey))
                throw new InvalidOperationException($"You must set {ENV_NAME} to be able to use Google Translate provider");
        }

        public GoogleTranslationService(IHttpClientFactory factory, string apiKey)
        {
            _factory = factory;
            _apiKey = apiKey;
        }

        public async Task<TranslationOutput> TranslateAsync(TranslationInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return new TranslationOutput
                {
                    Input = input.Text,
                    DetectedLanguage = null,
                    Output = input.Text,
                };
            }

            ValidateCulture(input);

            using var client = _factory.CreateClient();

            var uri = $"https://translation.googleapis.com/language/translate/v2?q={WebUtility.UrlEncode(input.Text)}&key={_apiKey}&target={input.TargetLanguage}";

            try
            {
                var result = await client.GetStringAsync(uri);

                var response = JsonConvert.DeserializeObject<GoogleTranslationResponse>(result);

                var translation = response?.data.translations?[0];

                if (translation == null)
                    return null;

                return new TranslationOutput
                {
                    Input = input.Text,
                    DetectedLanguage = translation.detectedSourceLanguage,
                    Output = translation.translatedText,
                };
            }
            catch
            {
                return null;
            }
        }

        
        private void ValidateCulture(TranslationInput input)
        {
            if (string.IsNullOrEmpty(input.TargetLanguage))
                throw new NotSupportedException();

            // make sure we don't try anything strange
            if (input.TargetLanguage.Length > 2)
                throw new NotSupportedException();
        }

        public class GoogleTranslationResponse
        {
            public GoogleTranslationData data { get; set; }
        }

        public class GoogleTranslationData
        {
            public GoogleTranslation[] translations;
        }

        public class GoogleTranslation
        {
            public string detectedSourceLanguage { get; set; }

            public string translatedText { get; set; }
        }
    }
}
