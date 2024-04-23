using System.Linq;
using Macroscop.TextAnalyzer.Api.Bll.Services.Interfaces;

namespace Macroscop.TextAnalyzer.Api.Bll.Services
{
    public sealed class TextAnalyzeService : ITextAnalyzeService
    {
        public bool CheckPalindrome(string src)
        {
            var cleanString = new string(
                    src.Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                    .ToArray())
                .ToLower();

            
            return cleanString.SequenceEqual(cleanString.Reverse());
        }
    }
}