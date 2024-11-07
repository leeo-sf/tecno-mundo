using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.RabbitMQServer;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _keyCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQPaymentConsumer(
            IConfiguration configuration,
            IRabbitMQMessageSender rabbitMQMessageSender,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            var factory = new ConnectionFactory
            {
                HostName =
                    _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value ?? "",
                UserName =
                    _configuration.GetSection("RabbitMQServer").GetSection("Username").Value ?? "",
                Password =
                    _configuration.GetSection("RabbitMQServer").GetSection("Password").Value ?? "",
                VirtualHost =
                    _configuration.GetSection("RabbitMQServer").GetSection("VirtualHost").Value
                    ?? ""
            };
            _keyCache =
                _configuration.GetSection("Redis").GetSection("Key_Cache").Value ?? "orders";
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Absolute_Expire").Value
                            ?? "3600"
                    )
                ),
                SlidingExpiration = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Sliding_Expire").Value
                            ?? "600"
                    )
                )
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
                UpdatePaymentVO vo =
                    JsonSerializer.Deserialize<UpdatePaymentVO>(content) ?? new UpdatePaymentVO();
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
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    await orderService.UpdateOrderPaymentStatus(
                        vo.OrderId,
                        vo.Status,
                        _keyCache,
                        _options
                    );
                }
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
