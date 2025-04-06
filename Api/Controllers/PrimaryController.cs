using Microsoft.AspNetCore.Mvc;
using Vulpes.Liteyear.Api.Attributes;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.Api.Controllers;

[ApiController]
[LiteyearRoute("[controller]")]
public class PrimaryController : ControllerBase
{
    private readonly IMessagePublisher messagePublisher;

    public PrimaryController(IMessagePublisher messagePublisher)
    {
        this.messagePublisher = messagePublisher;
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteWorkflow(string workflow)
    {
        await messagePublisher.PublishAsync(LiteyearQueue.Default with { Identifier = "TestConsumer" }, new TestMessage(workflow));
        return Ok("Received workflow: " + workflow);
    }

    record TestMessage(string Value) : LiteyearMessage;
}