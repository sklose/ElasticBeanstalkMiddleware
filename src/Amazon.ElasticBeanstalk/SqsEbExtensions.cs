using Amazon.ElasticBeanstalk;

namespace Microsoft.AspNet.Builder
{
    public static class SqsEbExtensions
    {
        public static void UseElasticBeanstalkWorker(this IApplicationBuilder builder)
        {
            builder.Use(next => new SqsEbMiddleware(next).Invoke);
        }
    }
}
