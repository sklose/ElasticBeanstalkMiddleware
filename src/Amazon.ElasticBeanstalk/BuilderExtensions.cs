using Amazon.ElasticBeanstalk;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static void RunElasticBeanstalkWorker(this IApplicationBuilder builder, EbOptions options)
        {
            builder.Run(new EbMiddleware(options).Invoke);
        }
    }
}
