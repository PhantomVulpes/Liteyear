namespace Vulpes.Liteyear.Api.Models;

public record BeginWorkflowRequest
{
    public string DuraluminBucket { get; init; } = string.Empty;
    public string DuraluminKey { get; init; } = string.Empty;
}