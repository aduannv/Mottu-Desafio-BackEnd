using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mottu.Application.Settings;
using Mottu.Domain.Events;
using Mottu.Infrastructure.DbContext.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mottu.Application.Consumers
{
    public class CreateMotorcycleConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<CreateMotorcycleConsumer> _logger;
        private readonly IMapper _mapper;
        private IConnection? _connection;
        private IModel? _channel;

        public CreateMotorcycleConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<CreateMotorcycleConsumer> logger, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> CreateMotorcycleConsumer -> StartAsync");

            using var scope = _serviceScopeFactory.CreateScope();
            var rabbitMQSettings = scope.ServiceProvider.GetRequiredService<RabbitMQSettings>();
            var connectionString = $"amqp://guest:guest@{rabbitMQSettings.HostName}:{rabbitMQSettings.Port}/";

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: rabbitMQSettings.CreateMotorcycleQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> CreateMotorcycleConsumer -> ExecuteAsync");
            using var scope = _serviceScopeFactory.CreateScope();
            var rabbitMQSettings = scope.ServiceProvider.GetRequiredService<RabbitMQSettings>();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var createMotorcycleEvent = JsonConvert.DeserializeObject<CreateMotorcycleEvent>(message) ?? throw new Exception("Error in deserialize CreateMotorcycleEvent");

                    if (createMotorcycleEvent.Ano == 2024)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var motorcycle = _mapper.Map<BrandNewMotorcycle>(createMotorcycleEvent);
                        dbContext.BrandNewMotorcycles.Add(motorcycle);
                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation($"Motorcycle {createMotorcycleEvent.Identificador} saved successfully.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                }
            };

            _channel.BasicConsume(
                queue: rabbitMQSettings.CreateMotorcycleQueue,
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> CreateMotorcycleConsumer -> StopAsync");
            _channel?.Close();

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
