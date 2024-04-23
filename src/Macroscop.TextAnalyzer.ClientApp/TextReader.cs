using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Macroscop.TextAnalyzer.ClientApp.Models;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class TextReader : IDataReader
    {
        public async Task ReadData(string folderPath, Channel<PalindromeRequestModel> outputChannel)
        {
            var files = Directory.GetFiles(folderPath);
            
            foreach (var filePath in files)
            {
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var streamReader = new StreamReader(fileStream);
                
                var fileContent = await streamReader.ReadToEndAsync();
                
                await outputChannel.Writer.WriteAsync(new PalindromeRequestModel(fileContent, filePath));
            }
            
            outputChannel.Writer.Complete();
        }
    }
}