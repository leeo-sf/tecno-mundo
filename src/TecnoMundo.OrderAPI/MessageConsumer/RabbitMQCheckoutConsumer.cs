using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.RabbitMQServer;
using TecnoMundo.Domain.Entities;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _keyCache;

        public RabbitMQCheckoutConsumer(
            IOrderService repository,
            IConfiguration configuration,
            IRabbitMQMessageSender rabbitMQMessageSender
        )
        {
            _service = repository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value,
                UserName = _configuration.GetSection("RabbitMQServer").GetSection("Username").Value,
                Password = _configuration.GetSection("RabbitMQServer").GetSection("Password").Value,
                VirtualHost = _configuration.GetSection("RabbitMQServer").GetSection("VirtualHost").Value
            };
            _keyCache = _configuration.GetSection("Redis").GetSection("Key_Cache").Value ?? "orders";
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(double.Parse(_configuration.GetSection("Redis").GetSection("Absolute_Expire").Value ?? "3600")),
                SlidingExpiration = TimeSpan.FromSeconds(double.Parse(_configuration.GetSection("Redis").GetSection("Sliding_Expire").Value ?? "600"))
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
            OrderHeader order =
                new()
                {
                    UserId = vo.UserId,
                    FistrName = vo.FistrName,
                    LastName = vo.LastName,
                    OrderDetails = new List<OrderDetail>(),
                    CardNumber = vo.CardNumber,
                    CouponCode = vo.CouponCode,
                    CVV = vo.CVV,
                    DiscountAmount = vo.DiscountAmount,
                    Email = vo.Email,
                    ExpireMonthYear = vo.ExpireMonthYear,
                    OrderTime = DateTime.Now,
                    PurchaseAmount = vo.PurchaseAmount,
                    PaymentStatus = false,
                    Phone = vo.Phone,
                    DateTime = vo.DateTime
                };

            foreach (var details in vo.CartDetails)
            {
                OrderDetail detail =
                    new()
                    {
                        ProductId = details.ProductId,
                        ProductName = details.Product.Name,
                        Price = details.Product.Price,
                        Count = details.Count,
                    };
                order.CartTotalItens += details.Count;
                order.OrderDetails.Add(detail);
            }

            await _service.AddOrder(order, $"{_keyCache}-{order.UserId}", _options);

            PaymentVO payment =
                new()
                {
                    Name = order.FistrName + " " + order.LastName,
                    CartNumber = order.CardNumber,
                    CVV = order.CVV,
                    ExpiryMonthYear = order.ExpireMonthYear,
                    OrderId = order.Id,
                    PurchaseAmount = order.PurchaseAmount,
                    Email = order.Email
                };

            try
            {
                var dataSendToRabbitMQ = new DataServerRabbitMQ(
                    hostName: _configuration.GetSection("RabbitMQServer:HostName").Value ?? "",
                    password: _configuration.GetSection("RabbitMQServer:Password").Value ?? "",
                    userName: _configuration.GetSection("RabbitMQServer:Username").Value ?? "",
                    virtualHost: _configuration.GetSection("RabbitMQServer:VirtualHost").Value ?? "",
                    queueName: "orderpaymentprocessqueue",
                    baseMessage: payment
                );
                _rabbitMQMessageSender.SendMessage<PaymentVO>(dataSendToRabbitMQ);
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
