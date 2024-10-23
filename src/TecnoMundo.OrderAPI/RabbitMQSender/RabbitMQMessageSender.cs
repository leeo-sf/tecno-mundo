using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using TecnoMundo.Application.DTOs;

namespace GeekShopping.OrderAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private readonly string _virtualHost;
        private IConnection _connection;

        public RabbitMQMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _hostName = _configuration.GetSection("RabbitMQServer").GetSection("HostName").Value ?? "";
            _password = _configuration.GetSection("RabbitMQServer").GetSection("Password").Value ?? "";
            _userName = _configuration.GetSection("RabbitMQServer").GetSection("Username").Value ?? "";
            _virtualHost = _configuration.GetSection("RabbitMQServer").GetSection("VirtualHost").Value ?? "";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body
                );
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                //para considerar as classes filhas
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize((PaymentVO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password,
                    VirtualHost = _virtualHost
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
                return true;
            CreateConnection();
            return _connection != null;
        }
    }
}
