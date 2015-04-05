using System;
using System.Collections.Generic;
using System.IO;

namespace Amazon.ElasticBeanstalk
{
    public class SqsMessage
    {
        public SqsMessage()
        {
            Attributes = new Dictionary<string, string>();
        }

        public Guid Id { get; set; }
        public string QueueName { get; set; }
        public string SenderId { get; set; }
        public string ContentType { get; set; }
        public int ReceiveCount { get; set; }
        public DateTime FirstReceivedAt { get; set; }
        public string TaskNane { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public Stream Payload { get; set; }
        public Dictionary<string, string> Attributes { get; }
    }
}
