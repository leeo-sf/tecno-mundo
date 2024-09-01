using TecnoMundo.MessageBus;

namespace TecnoMundo.CartAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
