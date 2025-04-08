using Vulpes.Electrum.Validation.Models;

namespace Vulpes.Liteyear.Domain.Models;

public record WorkflowStep : AggregateRoot
{
    public static WorkflowStep Empty { get; } = new();
    public static WorkflowStep Default => Empty;

    public string TargetModule { get; init; } = string.Empty;
}