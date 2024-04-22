using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddTransient<App>();
                    services.AddTransient<IDataReader, TextReader>();
                });
            
            var app = builder.Build();
            
            var result = await app.Services.GetRequiredService<App>()
                .StartAsync();

            return result;
        }
    }
}