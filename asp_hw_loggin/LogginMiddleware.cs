using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace asp_hw_loggin
{
    public class LogginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogginMiddleware> _logger;

        public LogginMiddleware(RequestDelegate next, ILogger<LogginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                int a = 2;
                int b = 0;
                Console.WriteLine(a / b);
                await _next(context);
            }
            catch (Exception ex)
            {
                LogErrorToFile(ex);
                throw; 
            }
        }

        private void LogErrorToFile(Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            var logFilePath = "error.log";
            File.AppendAllText(logFilePath, $"{DateTime.UtcNow} - An unhandled exception occurred: {ex}\n");
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogginMiddleware>();
        }
    }
}
