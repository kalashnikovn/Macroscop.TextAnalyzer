using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Macroscop.TextAnalyzer.Api.Formatters
{
    public sealed class PlainTextFormatter : TextInputFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeNames.Text.Plain));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.ASCII);
        }
        
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var reader = new StreamReader(context.HttpContext.Request.Body);
            var plainText = await reader.ReadToEndAsync();
        
            return await InputFormatterResult.SuccessAsync(plainText);
        }
        
        protected override bool CanReadType(Type type) => type == typeof(string);
    }
}