using Microsoft.AspNetCore.Mvc;
using Vulpes.Liteyear.Api.Attributes;
using Vulpes.Liteyear.Api.Models;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.Domain.Models;
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
        var executionFlowKey = Guid.NewGuid();

        var contentReferences = new List<ContentReference>();
        foreach (var key in request.Keys)
        {
            var input = await contentRepository.GetDocumentAsync(request.DuraluminBucket, key);
            var contentReferenceUri = await contentRepository.StoreDocumentAsync(input, executionFlowKey, "INGEST");

            var contentReference = ContentReference.Default with
            {
                DuraluminKey = contentReferenceUri,
                DataLabels = ["Ingest"],
            };

            contentReferences.Add(contentReference);
        }

        var executionFlow = ExecutionFlow.Default with
        {
            Key = executionFlowKey,
            WorkflowSteps = request.Workflow,
            Results = [WorkflowResult.Default with
            {
                Resource = Resource.Default with { ContentReferences = contentReferences },
                Status = WorkflowResultStatus.Success,
                Initiator = WorkflowStep.Default with { TargetModule = "Ingest Event" },
            }]
        };

        return Ok(executionFlow.Key.ToString());
    }

    record TestMessage(string Value) : LiteyearMessage;
}