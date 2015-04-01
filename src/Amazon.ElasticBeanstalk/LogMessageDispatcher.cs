using System;
using System.Threading.Tasks;

namespace Amazon.ElasticBeanstalk
{
    public class LogMessageDispatcher : IMessageDispatcher
    {
        public Task Dispatch(SqsMessage message)
        {
            System.Diagnostics.Trace.Write(message);
            return Task.FromResult(0);
        }
    }
}
