using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using NPOI.SS.Formula.Functions;
using RabbitMQ.Client;

namespace TecnoMundo.Application.RabbitMQServer
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private IConnection? _connection;

        public void SendMessage<T>(DataServerRabbitMQ<T> data)
        {
            if (ConnectionExists(data))
            {
                using var channel = _connection?.CreateModel();
                channel?.QueueDeclare(queue: data.QueueName, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray<T>(data.BaseMessage);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: data.QueueName,
                    basicProperties: null,
                    body: body
                );
            }
        }

        private byte[] GetMessageAsByteArray<T>(T message)
        {
            var options = new JsonSerializerOptions
            {
                //para considerar as classes filhas
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection<T>(DataServerRabbitMQ<T> data)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = data.HostName,
                    UserName = data.UserName,
                    Password = data.Password,
                    VirtualHost = data.VirtualHost
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists<T>(DataServerRabbitMQ<T> data)
        {
            if (_connection != null)
                return true;
            CreateConnection(data);
            return _connection != null;
        }
    }
}
