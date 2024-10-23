using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.RabbitMQSender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TecnoMundo.Application.Interfaces;
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

            await _service.AddOrder(order);

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
                _rabbitMQMessageSender.SendMessage(payment, "orderpaymentprocessqueue");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
