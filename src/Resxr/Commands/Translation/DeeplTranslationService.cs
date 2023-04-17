using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resxr.Commands.Translation
{
    public class DeeplTranslationService : ITranslationService
    {
        public Task<GoogleTranslationService.TranslationOutput> TranslateAsync(GoogleTranslationService.TranslationInput input)
        {
            throw new NotImplementedException();
        }
    }
}
