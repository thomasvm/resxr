using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Resxr.Commands.Translation.GoogleTranslationService;

namespace Resxr.Commands.Translation
{
    public interface ITranslationService
    {
        Task<TranslationOutput> TranslateAsync(TranslationInput input);
    }

    public class TranslationInput
        {
            public string TargetLanguage { get; set; }

            public string Text { get; set; }
        }

        public class TranslationOutput
        {
            public string Input { get; set; }

            public string DetectedLanguage { get; set; }

            public string Output { get; set; }
        }

}
