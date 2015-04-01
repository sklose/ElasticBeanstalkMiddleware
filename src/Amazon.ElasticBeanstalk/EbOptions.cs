namespace Amazon.ElasticBeanstalk
{
    public class EbOptions
    {
        public EbOptions()
        {
            Dispatcher = new LogMessageDispatcher();
        }
        
        public IMessageDispatcher Dispatcher { get; set; }
    }
}
