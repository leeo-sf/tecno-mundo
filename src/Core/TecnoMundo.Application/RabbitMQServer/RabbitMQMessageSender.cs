using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace TecnoMundo.Application.RabbitMQServer
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private IConnection? _connection;

        public void SendMessage<T>(DataServerRabbitMQ data)
        {
            if (ConnectionExists(data))
            {
                using var channel = _connection?.CreateModel();
                channel?.QueueDeclare(queue: data.queueName, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray<T>(data.baseMessage);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: data.queueName,
                    basicProperties: null,
                    body: body
                );
            }
        }

        private byte[] GetMessageAsByteArray<T>(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                //para considerar as classes filhas
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(
                message,
                options
            );
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection(DataServerRabbitMQ data)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = data.hostName,
                    UserName = data.userName,
                    Password = data.password,
                    VirtualHost = data.virtualHost
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists(DataServerRabbitMQ data)
        {
            if (_connection != null)
                return true;
            CreateConnection(data);
            return _connection != null;
        }
    }
}
