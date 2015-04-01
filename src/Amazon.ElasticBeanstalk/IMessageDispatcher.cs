using System.Threading.Tasks;

namespace Amazon.ElasticBeanstalk
{
    public interface IMessageDispatcher
    {
        Task Dispatch(SqsMessage message);
    }
}
