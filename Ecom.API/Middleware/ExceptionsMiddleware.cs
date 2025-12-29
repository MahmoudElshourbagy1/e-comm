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
        private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(30);

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
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many request . please try agin later");
                    
                    await context.Response.WriteAsJsonAsync(response);
                   
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment()?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message,ex.StackTrace)
                    : new ResponseAPI((int)HttpStatusCode.InternalServerError, ex.Message);
                var Json = JsonSerializer.Serialize(response);
               await context.Response.WriteAsync(Json);
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
                if (count >= 8)
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
