using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNet.Http;

namespace Amazon.ElasticBeanstalk
{
    public static class SqsMessageReader
    {
        private const string MessageIdHeader = "X-Aws-Sqsd-Msgid";
        private const string QueueNameHeader = "X-Aws-Sqsd-Queue";
        private const string FirstReceivedAtHeader = "X-Aws-Sqsd-First-Received-At";
        private const string ReceiveCountHeader = "X-Aws-Sqsd-Receive-Count";
        private const string TaskNameHeader = "X-Aws-Sqsd-Taskname";
        private const string TaskScheduledAtHeader = "X-Aws-Sqsd-Scheduled-At";
        private const string SenderIdHeader = "X-Aws-Sqsd-Sender-Id";
        private const string AttributePrefixHeader = "X-Aws-Sqsd-Attr-";

        private static HashSet<string> RequiredHeaders = new HashSet<string>(
            new[]
            {
                MessageIdHeader, QueueNameHeader, FirstReceivedAtHeader, ReceiveCountHeader
            });

        public static SqsMessage ReadFrom(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(string.Format("Unexpected method '{0}'", request.Method));
            }

            VerifyHeaders(request);

            Guid messageId;
            if (!Guid.TryParse(request.Headers[MessageIdHeader], out messageId))
            {
                throw new Exception(string.Format(
                    "Invalid value '{0}' for header '{1}'", request.Headers[MessageIdHeader], MessageIdHeader));
            }            

            DateTime firstReceivedAt;
            if (!DateTime.TryParse(request.Headers[FirstReceivedAtHeader],
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out firstReceivedAt))
            {
                throw new Exception(string.Format(
                    "Invalid value '{0}' for header '{1}'", request.Headers[FirstReceivedAtHeader], FirstReceivedAtHeader));
            }

            int receiveCount;
            if (!int.TryParse(request.Headers[ReceiveCountHeader], NumberStyles.Integer, CultureInfo.InvariantCulture, out receiveCount))
            {
                throw new Exception(string.Format(
                    "Invalid value '{0}' for header '{1}'", request.Headers[ReceiveCountHeader], ReceiveCountHeader));
            }

            string senderId = request.Headers[SenderIdHeader];
            string queueName = request.Headers[QueueNameHeader];
            string taskName = request.Headers[TaskNameHeader];
            DateTime? taskScheduledAt = null;

            if (request.Headers.ContainsKey(TaskScheduledAtHeader))
            {
                DateTime tmp;
                if (!DateTime.TryParse(request.Headers[TaskScheduledAtHeader],
                        CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out tmp))
                {
                    throw new Exception(string.Format(
                        "Invalid value '{0}' for header '{1}'", request.Headers[TaskScheduledAtHeader], TaskScheduledAtHeader));
                }

                taskScheduledAt = tmp;
            }

            return new SqsMessage
            {
                Id = messageId,
                ContentType = request.ContentType,
                FirstReceivedAt = firstReceivedAt,
                QueueName = queueName,
                ReceiveCount = receiveCount,
                SenderId = senderId,
                TaskNane = taskName,
                TaskScheduledAt = taskScheduledAt,
                Payload = request.Body
            };
        }

        private static void VerifyHeaders(HttpRequest request)
        {
            foreach (var key in RequiredHeaders)
            {
                if (string.IsNullOrEmpty(request.Headers[key]))
                {
                    throw new Exception(string.Format("Expected header '{0}' missing", key));
                }
            }
        }
    }
}