using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Amazon.ElasticBeanstalk
{
    public class EbMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EbOptions _options;

        public EbMiddleware(RequestDelegate next, EbOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var message = SqsMessageReader.ReadFrom(context.Request);
                await _options.Dispatcher.Dispatch(message);
                context.Response.StatusCode = 200; // OK
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync(ex.Message);
            }
            
            await _next(context);
        }
    }
}
