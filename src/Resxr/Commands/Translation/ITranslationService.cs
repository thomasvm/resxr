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
}
