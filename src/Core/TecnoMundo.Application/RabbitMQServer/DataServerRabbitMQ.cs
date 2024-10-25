using GeekShopping.MessageBus;

namespace TecnoMundo.Application.RabbitMQServer
{
    public class DataServerRabbitMQ
    {
        public string hostName { get; private set; }
        public string password { get; private set; }
        public string userName { get; private set; }
        public string virtualHost { get; private set; }
        public string queueName { get; private set; }
        public BaseMessage baseMessage { get; private set; }

        public DataServerRabbitMQ(
            string hostName,
            string password,
            string userName,
            string virtualHost,
            string queueName,
            BaseMessage baseMessage)
        {
            this.hostName = hostName;
            this.password = password;
            this.userName = userName;
            this.virtualHost = virtualHost;
            this.queueName = queueName;
            this.baseMessage = baseMessage;
        }
    }
}
