using System;
using System.IO;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Macroscop.TextAnalyzer.ClientApp.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public class Program
    {
        public static async Task<int> Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services
                        .Configure<TextAnalyzerAppOptions>(configuration.GetSection("TextAnalyzerAppOptions"))
                        .AddTransient<App>()
                        .AddTransient<IDirContext, DirContext>()
                        .AddTransient<IDataReader, TextReader>()
                        .AddTransient<IDataProcessor, DataProcessor>()
                        .AddHttpClient("TextAnalyzerApi", client =>
                        {
                            client.BaseAddress = new Uri("http://localhost:5000/v1/text-analyze/");
                        });
                });
            
            var app = builder.Build();
            
            var result = await app.Services.GetRequiredService<App>()
                .StartAsync();

            return result;
        }
    }
}