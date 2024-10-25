using GeekShopping.MessageBus;

namespace TecnoMundo.Application.RabbitMQServer
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage<T>(DataServerRabbitMQ data);
    }
}
