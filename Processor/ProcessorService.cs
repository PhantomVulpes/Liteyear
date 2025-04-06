using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vulpes.Liteyear.Domain.Logging;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.Processor;

public class ProcessorService : BackgroundService
{
    private readonly IMessageSubscriber messageSubscriber;
    private readonly ILogger<ProcessorService> logger;
    private readonly IEnumerable<IMessageConsumer> consumers;

    public ProcessorService(IMessageSubscriber messageSubscriber, ILogger<ProcessorService> logger, IEnumerable<IMessageConsumer> consumers)
    {
        this.messageSubscriber = messageSubscriber; ;
        this.logger = logger;
        this.consumers = consumers;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation($"{LogTags.ApplicationStart} {nameof(ProcessorService)} is attempting to start.");

            foreach (var consumer in consumers)
            {
                await messageSubscriber.SubscribeAsync(consumer.Queue, consumer.ConsumeMessageAsync);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                // Keep the service running until cancellation is requested
                logger.LogDebug($"{LogTags.Heartbeat} {nameof(ProcessorService)} is running: UTC-{DateTime.UtcNow}");
                await Task.Delay(1000, stoppingToken); // Sleep for a second to avoid busy waiting
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation($"{LogTags.ApplicationShutdown} {nameof(ProcessorService)} is shutting down.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{LogTags.ConsumerFatal} {nameof(ProcessorService)} encountered a fatal error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}