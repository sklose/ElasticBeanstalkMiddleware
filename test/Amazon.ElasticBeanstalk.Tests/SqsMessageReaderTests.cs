using System;
using System.IO;
using System.Text;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Core;
using Amazon.ElasticBeanstalk;
using Xunit;
using Shouldly;

namespace Amazon.ElasticBeanstalk.Tests
{
    public class SqsMessageReaderTests
    {
        [Fact]
        public void ValidHttpRequestReturnsValidSqsMessage()
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

        [Fact]
        public void ValidHttpRequestShouldParseMessageAttributes()
        {
            var context = CreateRequest();
            context.Request.Method = "POST";
            context.Request.Headers["X-aws-sqsd-msgid"] = "66FD63B0-5E83-4EE6-AAB9-2D39AD19C13E";
            context.Request.Headers["X-aws-sqsd-queue"] = "Test-Queue";
            context.Request.Headers["X-aws-sqsd-first-received-at"] = "2014-02-18T23:04:50Z";
            context.Request.Headers["X-aws-sqsd-receive-count"] = "2";
            context.Request.Headers["X-Aws-Sqsd-Attr-MyAttribute1"] = "abc";
            context.Request.Headers["X-Aws-Sqsd-Attr-MyAttribute2"] = "xyz";
            context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes("Hallo Welt"));

            var message = SqsMessageReader.ReadFrom(context.Request);

            Assert.Equal(message.Attributes.Count, 2);
            Assert.Equal(message.Attributes["MyAttribute1"], "abc");
            Assert.Equal(message.Attributes["MyAttribute2"], "xyz");
        }

        [Fact]
        public void MessageAttributesShouldBeCaseInsensitive()
        {
            var context = CreateRequest();
            context.Request.Method = "POST";
            context.Request.Headers["X-aws-sqsd-msgid"] = "66FD63B0-5E83-4EE6-AAB9-2D39AD19C13E";
            context.Request.Headers["X-aws-sqsd-queue"] = "Test-Queue";
            context.Request.Headers["X-aws-sqsd-first-received-at"] = "2014-02-18T23:04:50Z";
            context.Request.Headers["X-aws-sqsd-receive-count"] = "2";
            context.Request.Headers["X-Aws-Sqsd-Attr-MyAttribute1"] = "abc";
            context.Request.Headers["X-Aws-Sqsd-Attr-MyAttribute2"] = "xyz";
            context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes("Hallo Welt"));

            var message = SqsMessageReader.ReadFrom(context.Request);

            Assert.True(message.Attributes.ContainsKey("MyAttribute2"));
            Assert.True(message.Attributes.ContainsKey("myattribute2"));
            Assert.True(message.Attributes.ContainsKey("MYATTRIBUTE2"));
            Assert.True(message.Attributes.ContainsKey("MYATTrIBUTE2"));
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        public void InvalidMethodThrowsException(string method)
        {
            var context = CreateRequest();
            context.Request.Method = method;
            context.Request.Headers["X-aws-sqsd-msgid"] = "66FD63B0-5E83-4EE6-AAB9-2D39AD19C13E";
            context.Request.Headers["X-aws-sqsd-queue"] = "Test-Queue";
            context.Request.Headers["X-aws-sqsd-first-received-at"] = "2014-02-18T23:04:50Z";
            context.Request.Headers["X-aws-sqsd-receive-count"] = "2";

            Should.Throw<Exception>(() => SqsMessageReader.ReadFrom(context.Request));
        }

        private HttpContext CreateRequest()
        {
            HttpContext context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }
    }
}
