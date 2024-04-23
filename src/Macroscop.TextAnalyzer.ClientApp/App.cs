using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;
using Macroscop.TextAnalyzer.ClientApp.Models;
using Macroscop.TextAnalyzer.ClientApp.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class App
    {
        private readonly IDataReader _dataReader;
        private readonly IDataProcessor _dataProcessor;
        private readonly IDirContext _dirContext;
        private readonly ILogger<App> _logger;
        private readonly TextAnalyzerAppOptions _options;
        private readonly Channel<PalindromeRequestModel> _requestsChannel;
        private readonly Queue<Task> _tasks = new();
        private readonly Dictionary<Task, CancellationTokenSource> _cancellationTokenSources = new();
        private bool _isProcessorsFinishRead;
        
        public App(
            IDataReader dataReader,
            IDataProcessor dataProcessor,
            IDirContext dirContext,
            IOptionsMonitor<TextAnalyzerAppOptions> optionsMonitor,
            ILogger<App> logger)
        {
            _dataReader = dataReader;
            _dataProcessor = dataProcessor;
            _dirContext = dirContext;
            _logger = logger;
            _options = optionsMonitor.CurrentValue;
            
            optionsMonitor.OnChange(options => ChangeNumberOfProcessors(options.ParallelismDegree));

            _requestsChannel = Channel.CreateBounded<PalindromeRequestModel>(
                new BoundedChannelOptions(_options.ReaderChannelBound)
                {
                    SingleReader = false,
                    SingleWriter = true
                });
        }
        
        public async Task<int> StartAsync()
        {
            var dirPath = GetPath();
            
            var readerTask = _dataReader.ReadData(dirPath, _requestsChannel);
            RunProcessors(_options.ParallelismDegree, _requestsChannel);
            

            await readerTask;
            await _requestsChannel.Reader.Completion;
            
            lock (_tasks)
            {
                _isProcessorsFinishRead = true;
            }
            
            await Task.WhenAll(_tasks);
            
            return 0;
        }
        
        private void RunProcessors(int count, Channel<PalindromeRequestModel> inputChannel)
        {
            for (int i = 0; i < count; i++)
            {
                var tokenSource = new CancellationTokenSource();
                var cancellationToken = tokenSource.Token;

                var task = _dataProcessor.ProcessData(inputChannel, cancellationToken);

                _tasks.Enqueue(task);
                _cancellationTokenSources[task] = tokenSource;
                _logger.Log(LogLevel.Debug, "Task{number} created and started", _tasks.Count);
            }
        }
        
        private void ChangeNumberOfProcessors(int newCount)
        {
            lock (_tasks)
            {
                if (_isProcessorsFinishRead)
                {
                    return;
                }
    
                if (newCount > _tasks.Count)
                {
                    var delta = newCount - _tasks.Count;
                    RunProcessors(delta, _requestsChannel);
                }
                else if (newCount < _tasks.Count)
                {
                    var delta = _tasks.Count - newCount;
                    for (var i = 0; i < delta; ++i)
                    {
                        var task = _tasks.Dequeue();
                        _cancellationTokenSources[task].Cancel();
                        _logger.Log(LogLevel.Debug, "Task{number} canceled", _tasks.Count + 1);
                    }
                }
            }
        }
        
        
        private string GetPath()
        {
            const string inputMessage = "Укажите путь до папки, которая содержтит файлы для запросов \n" +
                                        "(относительно текущей рабочей директории)";
            string path;
            
            do
            {
                Console.WriteLine($"{inputMessage}\nТекущая рабочая директория: {_dirContext.GetProjectDirectory()}");
                Console.Write(">> ");

                var relativePath = Console.ReadLine();
                path = Path.Combine(_dirContext.GetProjectDirectory(), relativePath ?? string.Empty);
                
            } while (!Directory.Exists(path) || !IsDirectory(path));


            return path;
        }
        
        private bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Directory);
        }
    }
}