using System;
using Microsoft.AspNetCore.Mvc;

namespace Macroscop.TextAnalyzer.Api.Controllers.V1
{
    [ApiController]
    [Route("v1/text-analyze")]
    public sealed class TextAnalyzeController
    {
        [HttpPost("palindrome")]
        public string Palindrome([FromBody] string text)
        {
            //throw new NotImplementedException();
        }
    }
}