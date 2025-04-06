using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using System.Security.Authentication;
using Vulpes.Liteyear.Domain.Logging;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.External.Extensions;
using System.Threading.Tasks;

namespace Vulpes.Liteyear.External.Rabbit;

public class RabbitMqConnectionManager : IRabbitMqConnectionManager
{
    private readonly Lazy<Task<IConnectionFactory>> connectionFactoryLazy;
    private readonly Lazy<Task<IConnection>> subscribeConnectionLazy;
    private readonly Lazy<Task<IConnection>> publishConnectionLazy;
    private readonly ILogger<RabbitMqConnectionManager> logger;
    private readonly IHostApplicationLifetime lifetime;

    public Task<IConnection> PublishConnection => publishConnectionLazy.Value;
    public Task<IConnection> SubscribeConnection => subscribeConnectionLazy.Value;

    public RabbitMqConnectionManager(ILogger<RabbitMqConnectionManager> logger, IHostApplicationLifetime lifetime)
    {
        this.logger = logger;
        this.lifetime = lifetime;

        connectionFactoryLazy = new Lazy<Task<IConnectionFactory>>(CreateConnectionFactory);
        publishConnectionLazy = new Lazy<Task<IConnection>>(CreatePublisherConnection);
        subscribeConnectionLazy = new Lazy<Task<IConnection>>(CreateSubscriberConnection);
    }

    private async Task<IConnectionFactory> CreateConnectionFactory()
    {
        // Everything is running on a local container, this is probably all fine but idk we'll find out.
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "password",
            Port = 62023,
            VirtualHost = "liteyear",
            Ssl = new SslOption
            {
                Enabled = false,
                Version = SslProtocols.None,
                ServerName = "localhost"
            },
            ConsumerDispatchConcurrency = 1, // Ensures that only one message is processed at a time per consumer
        };

        await InitializeAsync(factory);

        logger.LogInformation($"{LogTags.EventingStartup} RabbitMQ connection factory created successfully.");

        return factory;
    }

    private async Task InitializeAsync(ConnectionFactory connectionFactory)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync("liteyear-exchange.DLX", ExchangeType.Topic, true, false);
        await channel.ExchangeDeclareAsync("liteyear-exchange", ExchangeType.Topic, true, false);

        logger.LogInformation($"{LogTags.EventingStartup} RabbitMQ exchanges initialized successfully.");
    }

    private async Task<IConnection> CreatePublisherConnection()
    {
        var factory = await connectionFactoryLazy.Value;
        var connection = await factory.CreateConnectionAsync();

        logger.LogInformation($"{LogTags.EventingStartup} RabbitMQ publisher connection created successfully.");

        return connection;
    }

    private async Task<IConnection> CreateSubscriberConnection()
    {
        var factory = await connectionFactoryLazy.Value;
        var connection = await factory.CreateConnectionAsync();

        connection.ConnectionBlockedAsync += (sender, args) =>
        {
            logger.LogError($"{LogTags.EventingStartup} RabbitMQ connection blocked. A shutdown has been triggered: {args.Reason}");
            lifetime.StopApplication();

            return Task.CompletedTask;
        };

        connection.ConnectionShutdownAsync += (sender, args) =>
        {
            logger.LogError($"{LogTags.EventingStartup} RabbitMQ connection shutdown: {args.ReplyText}");
            lifetime.StopApplication();

            return Task.CompletedTask;
        };

        logger.LogInformation($"{LogTags.EventingStartup} RabbitMQ subscriber connection created successfully.");
        return connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (subscribeConnectionLazy.IsValueCreated)
        {
            (await subscribeConnectionLazy.Value)?.Dispose();
        }

        if (publishConnectionLazy.IsValueCreated)
        {
            (await publishConnectionLazy.Value)?.Dispose();
        }
    }

    public async Task EnsureQueue(IChannel channel, LiteyearQueue queue)
    {
        await queue.DeclareAsync(channel);

        logger.LogInformation($"{LogTags.EventingStartup} Ensured queue '{queue.Name}' exists on RabbitMQ.");
    }
}
