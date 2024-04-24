using System;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.Api.Bll.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Macroscop.TextAnalyzer.Api.Controllers.V1
{
    [ApiController]
    [Route("v1/text-analyze")]
    public sealed class TextAnalyzeController
    {
        private readonly ITextAnalyzeService _textAnalyzeService;

        public TextAnalyzeController(ITextAnalyzeService textAnalyzeService)
        {
            _textAnalyzeService = textAnalyzeService;
        }
        
        [HttpPost("palindrome")]
        public async Task<bool> Palindrome([FromBody] string text)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            return _textAnalyzeService.CheckPalindrome(text);
        }
    }
}