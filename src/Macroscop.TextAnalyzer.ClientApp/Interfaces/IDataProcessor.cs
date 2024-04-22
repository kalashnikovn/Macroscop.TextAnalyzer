using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Models;

namespace Macroscop.TextAnalyzer.ClientApp.Interfaces
{
    public interface IDataProcessor
    {
        Task ProcessData(Channel<PalindromeRequestModel> inputChannel,
            CancellationToken cancellationToken);
    }
}