namespace Vulpes.Liteyear.Domain.Models;

public record WorkflowResult
{
    public static WorkflowResult Empty { get; } = new();
    public static WorkflowResult Default => Empty;

    public WorkflowResultStatus Status { get; init; } = WorkflowResultStatus.None;
    public string Message { get; init; } = string.Empty;

    public WorkflowStep Initiator { get; init; } = WorkflowStep.Empty;
    public Resource Resource { get; init; } = Resource.Empty;
}

public enum WorkflowResultStatus
{
    None,
    Success,
    Failure,
}