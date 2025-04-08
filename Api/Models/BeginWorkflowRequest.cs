using Vulpes.Liteyear.Domain.Models;

namespace Vulpes.Liteyear.Api.Models;

public record BeginWorkflowRequest
{
    public string DuraluminBucket { get; init; } = string.Empty;
    public IEnumerable<string> Keys { get; init; } = [];
    public IEnumerable<WorkflowStep> Workflow { get; init; } = [];
}