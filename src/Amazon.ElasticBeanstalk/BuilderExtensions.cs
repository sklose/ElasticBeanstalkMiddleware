using Amazon.ElasticBeanstalk;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static void UseElasticBeanstalkWorker(this IApplicationBuilder builder, EbOptions options)
        {
            builder.Use(next => new EbMiddleware(next, options).Invoke);
        }

        public static void UseElasticBeanstalkWorker(this IApplicationBuilder builder)
        {
            UseElasticBeanstalkWorker(builder, new EbOptions());
        }
    }
}
