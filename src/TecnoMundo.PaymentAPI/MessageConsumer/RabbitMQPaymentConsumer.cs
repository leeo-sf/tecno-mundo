using System.Text;
using System.Text.Json;
using GeekShopping.PaymentAPI.RabbitMQSender;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TecnoMundo.Application.DTOs;

namespace GeekShopping.PaymentAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;
        private readonly IProcessPayment _processPayment;

        public RabbitMQPaymentConsumer(
            IConfiguration configuration,
            IRabbitMQMessageSender rabbitMQMessageSender,
            IProcessPayment processPayment
        )
        {
            _processPayment = processPayment;
            _rabbitMQMessageSender = rabbitMQMessageSender;
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
                queue: "orderpaymentprocessqueue",
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
                PaymentVO vo = JsonSerializer.Deserialize<PaymentVO>(content) ?? new PaymentVO();
                ProcessPayment(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderpaymentprocessqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessPayment(PaymentVO vo)
        {
            var result = _processPayment.PaymentProcessor();

            UpdatePaymentVO paymentResult =
                new()
                {
                    Status = result,
                    OrderId = vo.OrderId,
                    Email = vo.Email
                };

            try
            {
                _rabbitMQMessageSender.SendMessage(paymentResult, "orderpaymentresultqueue");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
