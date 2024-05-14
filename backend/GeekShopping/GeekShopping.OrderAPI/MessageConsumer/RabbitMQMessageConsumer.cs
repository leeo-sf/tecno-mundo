
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQMessageConsumer(OrderRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value,
                UserName = _configuration.GetSection("RabbitMQServer").GetSection("Username").Value,
                Password = _configuration.GetSection("RabbitMQServer").GetSection("Password").Value
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
                ProccessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProccessOrder(CheckoutHeaderVO vo)
        {
            throw new NotImplementedException();
        }
    }
}
