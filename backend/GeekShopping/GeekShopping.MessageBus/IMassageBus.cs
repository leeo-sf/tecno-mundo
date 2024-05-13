namespace GeekShopping.MessageBus
{
    public interface IMassageBus
    {
        Task PublicMassage(BaseMessage message, string queueName);
    }
}
