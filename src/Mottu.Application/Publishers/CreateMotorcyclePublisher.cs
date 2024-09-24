using Mottu.Application.Settings;
using Mottu.Domain.Events;
using Mottu.Domain.Interfaces;
using Mottu.Infrastructure.DbContext.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mottu.Application.Publishers
{
    public class CreateMotorcyclePublisher : ICreateMotorcyclePublisher
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        public CreateMotorcyclePublisher(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public async Task Publish(Motorcycle motorcycle)
        {
            var connectionString = $"amqp://guest:guest@{_rabbitMQSettings.HostName}:{_rabbitMQSettings.Port}/";

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            };

            var str = _rabbitMQSettings.HostName;

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var createMotorcycleEvent = new CreateMotorcycleEvent
            {
                Identificador = motorcycle.Identificador,
                Modelo = motorcycle.Modelo,
                Ano = motorcycle.Ano,
                Placa = motorcycle.Placa
            };

            channel.QueueDeclare(queue: _rabbitMQSettings.CreateMotorcycleQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(createMotorcycleEvent));
            channel.BasicPublish(exchange: "", routingKey: _rabbitMQSettings.CreateMotorcycleQueue, basicProperties: null, body: body);
        }
    }
}
