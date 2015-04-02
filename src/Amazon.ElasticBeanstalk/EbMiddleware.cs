using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Amazon.ElasticBeanstalk
{
    public class EbMiddleware
    {
        private readonly EbOptions _options;

        public EbMiddleware(EbOptions options)
        {
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
        }
    }
}
