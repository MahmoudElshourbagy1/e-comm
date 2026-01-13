using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly  IMemoryCache _memoryCache;
        private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(1);

        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = 429;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(
                        new ApiException(429, "Too many requests, try again later")
                    );
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                    throw;

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment()
                    ? new ApiException(500, ex.Message, ex.StackTrace)
                    : new ResponseAPI(500, "Internal Server Error");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cachKey = $"Rate:{ip}";
            var dateNow = DateTime.UtcNow;
            var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey,entry =>{ entry.AbsoluteExpirationRelativeToNow = _timeSpan;
                return (timesTamp: dateNow, count: 0);
            });
            if(dateNow - timesTamp < _timeSpan)
            {
                if (count >= 10)
                {
                    return false;
                }
                _memoryCache.Set(cachKey, (timesTamp, count + 1),_timeSpan); 
            }else
            {
                _memoryCache.Set(cachKey, (timesTamp, count), _timeSpan);
            }
            return true;
        }
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Context-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
