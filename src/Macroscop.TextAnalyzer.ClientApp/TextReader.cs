using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Macroscop.TextAnalyzer.ClientApp.Models;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class TextReader : IDataReader
    {
        public Task ReadData(string folderName, Channel<PalindromeRequestModel> outputChannel)
        {
            
            throw new System.NotImplementedException();
        }
    }
}