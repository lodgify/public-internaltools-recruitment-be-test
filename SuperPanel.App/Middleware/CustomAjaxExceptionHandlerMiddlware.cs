using ExternalResource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace SuperPanel.App.Middleware
{
    public class CustomAjaxExceptionHandlerMiddlware
    {
        private readonly RequestDelegate _next;


        public CustomAjaxExceptionHandlerMiddlware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ApiException ex)
            {
                var response = httpContext.Response;
                if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    await HandleExceptionAsync(httpContext, ex);
                else
                    throw;

            }
            catch (Exception ex)
            {
                if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    await HandleExceptionAsync(httpContext, ex);
                else
                    throw;
            }
        }


        public static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var obj = JObject.FromObject(exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = Convert.ToInt32(obj["StatusCode"] ?? 500);
            var Result =
                JsonConvert.SerializeObject(new AjaxException(httpContext.Response.StatusCode, 520, exception.Message));

            return httpContext.Response.WriteAsync(Result);
        }
    }
    public static class AjaxMiddleware
    {
        public static void UseAjaxMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomAjaxExceptionHandlerMiddlware>();
        }
    }
}
