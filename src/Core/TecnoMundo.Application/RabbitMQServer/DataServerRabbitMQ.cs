using GeekShopping.MessageBus;
using NPOI.SS.Formula.Functions;

namespace TecnoMundo.Application.RabbitMQServer
{
    public class DataServerRabbitMQ<T>
    {
        public string HostName { get; private set; }
        public string Password { get; private set; }
        public string UserName { get; private set; }
        public string VirtualHost { get; private set; }
        public string QueueName { get; private set; }
        public T BaseMessage { get; private set; }

        public DataServerRabbitMQ(
            string hostName,
            string password,
            string userName,
            string virtualHost,
            string queueName,
            T baseMessage
        )
        {
            HostName = hostName;
            Password = password;
            UserName = userName;
            VirtualHost = virtualHost;
            QueueName = queueName;
            BaseMessage = baseMessage;
        }
    }
}
