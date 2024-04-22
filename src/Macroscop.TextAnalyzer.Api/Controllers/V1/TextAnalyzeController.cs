using System;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.Api.Requests.V1;
using Macroscop.TextAnalyzer.Api.Responses.V1;
using Microsoft.AspNetCore.Mvc;

namespace Macroscop.TextAnalyzer.Api.Controllers.V1
{
    [ApiController]
    [Route("v1/text-analyze")]
    public sealed class TextAnalyzeController
    {
        public TextAnalyzeController()
        {
            
        }
        
        [HttpPost("palindrome")]
        public async Task<PalindromeResponse> Palindrome(PalindromeRequest request)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return new PalindromeResponse(request.Text);
        }
    }
}