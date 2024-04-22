using System.Threading;
using System.Threading.Tasks;
using Macroscop.TextAnalyzer.Api.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Macroscop.TextAnalyzer.Api.Middlewares
{
    public class ConcurrencyLimiterMiddleware
    {
        private readonly IOptionsMonitor<ConcurrencyLimitOptions> _optionsMonitor;
        private readonly RequestDelegate _next;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        
        public ConcurrencyLimiterMiddleware(RequestDelegate next, IOptionsMonitor<ConcurrencyLimitOptions> optionsMonitor)
        {
            _next = next;
            _optionsMonitor = optionsMonitor;

            _semaphore = new SemaphoreSlim(_optionsMonitor.CurrentValue.MaxConcurrentRequests);
            _optionsMonitor.OnChange(options =>
            {
                _semaphore = new SemaphoreSlim(options.MaxConcurrentRequests);
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_semaphore.CurrentCount == 0)
            {
                context.Response.StatusCode = 429;
                return;
            }

            await _semaphore.WaitAsync();

            try
            {
                await _next(context);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}