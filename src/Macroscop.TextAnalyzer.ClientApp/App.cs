using System.IO;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class App
    {
        private readonly IDataReader _dataReader;

        public App(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }
        
        public async Task<int> StartAsync()
        {
            var files = Directory.GetFiles(@"C:\Users\user\Desktop\Macroscop.TextAnalyzer\src\Macroscop.TextAnalyzer.ClientApp\requests");
            
            return 0;
        }
    }
}