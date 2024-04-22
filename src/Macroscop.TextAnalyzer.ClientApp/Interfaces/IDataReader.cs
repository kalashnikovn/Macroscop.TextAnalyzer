using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Models;

namespace Macroscop.TextAnalyzer.ClientApp.Interfaces
{
    public interface IDataReader
    {
        Task ReadData(string folderName, Channel<PalindromeRequestModel> outputChannel);
    }
}