using System;
using System.IO;
using System.Text;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Core;
using Amazon.ElasticBeanstalk;
using Xunit;

namespace Amazon.ElasticBeanstalk.Tests
{
    public class SqsMessageReaderTests
    {
        protected HttpContext CreateRequest()
        {
            HttpContext context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        public class WhenReceivingMessageFromSqs : SqsMessageReaderTests
        {
            [Fact]
            public void Receive()
            {
                var context = CreateRequest();
                context.Request.Method = "POST";
                context.Request.Headers["X-aws-sqsd-msgid"] = "66FD63B0-5E83-4EE6-AAB9-2D39AD19C13E";
                context.Request.Headers["X-aws-sqsd-queue"] = "Test-Queue";
                context.Request.Headers["X-aws-sqsd-first-received-at"] = "2014-02-18T23:04:50Z";
                context.Request.Headers["X-aws-sqsd-receive-count"] = "2";
                context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes("Hallo Welt"));

                var message = SqsMessageReader.ReadFrom(context.Request);

                Assert.NotNull(message);
                Assert.Equal(message.Id, Guid.Parse("66FD63B0-5E83-4EE6-AAB9-2D39AD19C13E"));
                Assert.Equal(message.QueueName, "Test-Queue");
                Assert.Equal(message.FirstReceivedAt, new DateTime(2014, 2, 18, 23, 04, 50, DateTimeKind.Utc));
            }
        }
    }
}
