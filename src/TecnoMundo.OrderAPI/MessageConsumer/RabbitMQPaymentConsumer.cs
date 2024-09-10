using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQPaymentConsumer(
            OrderRepository repository,
            IConfiguration configuration,
            IRabbitMQMessageSender rabbitMQMessageSender
        )
        {
            _repository = repository;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value,
                UserName = _configuration.GetSection("RabbitMQServer").GetSection("Username").Value,
                Password = _configuration.GetSection("RabbitMQServer").GetSection("Password").Value,
                VirtualHost = _configuration.GetSection("RabbitMQServer").GetSection("VirtualHost").Value
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "orderpaymentresultqueue",
                false,
                false,
                false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResult vo = JsonSerializer.Deserialize<UpdatePaymentResult>(content);
                UpdaatePaymentStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderpaymentresultqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdaatePaymentStatus(UpdatePaymentResult vo)
        {
            try
            {
                await _repository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
