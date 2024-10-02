using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Filters
{
    public class GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger
        ) : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger = logger;

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(Exception))
            {
                var exception = context.Exception;
                _logger.LogError(exception.Message);
            }
        }
    }
}
