using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Macroscop.TextAnalyzer.ClientApp.Models;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class DataProcessor : IDataProcessor
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DataProcessor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task ProcessData(Channel<PalindromeRequestModel> inputChannel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var client = _httpClientFactory.CreateClient("TextAnalyzerApi");

            await foreach(var requestModel in inputChannel.Reader.ReadAllAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var response = await client.PostAsJsonAsync("palindrome", requestModel, cancellationToken);
                
                
                if (!response.IsSuccessStatusCode)
                {
                    PrintResponse(requestModel.FileName,
                        requestModel.Text,
                        "Bad status code!!!",
                        response.StatusCode);
                    continue;
                }
                
                var result = await response.Content.ReadFromJsonAsync<PalindromeResponseModel>();
                var responseInfo = result.Result ? "Строка является палиндромом" : "Строка не является палиндромом";
                
                PrintResponse(requestModel.FileName,
                    requestModel.Text,
                    responseInfo,
                    response.StatusCode);
                
            }
        }

        private void PrintResponse(string fileName, string fileText, string responseInfo, HttpStatusCode statusCode)
        {
            Console.WriteLine($"\n\nПуть до файла: {fileName} \n" +
                              $"Текст файла: {fileText} \n" +
                              $"Результат: {responseInfo} \n" +
                              $"Статус код: {statusCode}");
        }
    }
}