using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Amazon.ElasticBeanstalk
{
    public class SqsEbMiddleware
    {
        private readonly RequestDelegate _next;

        public SqsEbMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return _next(context);
        }
    }
}
