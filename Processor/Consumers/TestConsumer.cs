using Microsoft.Extensions.Logging;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.Processor.Consumers;

public class TestConsumer : IMessageConsumer
{
    private readonly ILogger<TestConsumer> logger;

    public TestConsumer(ILogger<TestConsumer> logger)
    {
        this.logger = logger;
    }

    public LiteyearQueue Queue => LiteyearQueue.Default with
    {
        Identifier = "TestConsumer",
    };

    public Task ConsumeMessageAsync(string message)
    {
        logger.LogInformation($"TestConsumer received message: {message}");
        // Simulate some processing
        return Task.CompletedTask;
    }
}
