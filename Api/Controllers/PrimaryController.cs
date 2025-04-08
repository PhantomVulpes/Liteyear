using Microsoft.AspNetCore.Mvc;
using Vulpes.Liteyear.Api.Attributes;
using Vulpes.Liteyear.Api.Models;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.Domain.Storage;

namespace Vulpes.Liteyear.Api.Controllers;

[ApiController]
[LiteyearRoute("[controller]")]
public class PrimaryController : ControllerBase
{
    private readonly IContentRepository contentRepository;

    public PrimaryController(IContentRepository contentRepository)
    {
        this.contentRepository = contentRepository;
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteWorkflow(BeginWorkflowRequest request)
    {
        // Get the Duralumin bucket and key from the request.
        // Move to Liteyear's bucket.
        var input = await contentRepository.GetExternalDocumentAsync(request.DuraluminBucket, request.DuraluminKey);
        await contentRepository.StoreDocumentAsync(input, "ingest");

        return Ok("Received request to execute workflow.");
    }

    record TestMessage(string Value) : LiteyearMessage;
}