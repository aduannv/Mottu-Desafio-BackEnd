using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Settings;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Mottu.ApiTests;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithName(Guid.NewGuid().ToString())
        .WithImage("postgres:latest")
        .WithDatabase("mottu")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithName(Guid.NewGuid().ToString())
        .WithImage("rabbitmq:3-management")
        .WithUsername("guest")
        .WithPassword("guest")
        .WithHostname("rabbitmq")
        .WithExposedPort("5672")
        .Build();

    public HttpClient _client;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();

        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Remove<DbContextOptions<AppDbContext>>();
                services.Remove<IConnectionFactory>();
                services.Remove<RabbitMQSettings>();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                services.AddSingleton<RabbitMQSettings>(new RabbitMQSettings()
                {
                    HostName = _rabbitMqContainer.Hostname,
                    Port = _rabbitMqContainer.GetMappedPublicPort("5672"),
                    CreateMotorcycleQueue = "create_motorcycle_queue"
                });

                services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
                {
                    Uri = new Uri(_rabbitMqContainer.GetConnectionString()),
                    DispatchConsumersAsync = true
                });
            });
        }).CreateClient();
    }

    public async new Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
    }
}
