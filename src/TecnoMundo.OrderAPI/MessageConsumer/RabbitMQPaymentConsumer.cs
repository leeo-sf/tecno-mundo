using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.RabbitMQServer;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQPaymentConsumer(
            IOrderService service,
            IConfiguration configuration,
            IRabbitMQMessageSender rabbitMQMessageSender
        )
        {
            _service = service;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value ?? "",
                UserName = _configuration.GetSection("RabbitMQServer").GetSection("Username").Value ?? "",
                Password = _configuration.GetSection("RabbitMQServer").GetSection("Password").Value ?? "",
                VirtualHost = _configuration.GetSection("RabbitMQServer").GetSection("VirtualHost").Value ?? ""
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
                UpdatePaymentVO vo = JsonSerializer.Deserialize<UpdatePaymentVO>(content) ?? new UpdatePaymentVO();
                UpdaatePaymentStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderpaymentresultqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdaatePaymentStatus(UpdatePaymentVO vo)
        {
            try
            {
                await _service.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
