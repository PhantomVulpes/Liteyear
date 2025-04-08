using Vulpes.Electrum.Validation.Models;

namespace Vulpes.Liteyear.Domain.Models;

public record ExecutionFlow : AggregateRoot
{
    public static ExecutionFlow Empty { get; } = new();
    public static ExecutionFlow Default => Empty;

    public IEnumerable<WorkflowStep> WorkflowSteps { get; init; } = [];
    public IEnumerable<WorkflowResult> Results { get; init; } = [];
}