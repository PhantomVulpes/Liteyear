using Vulpes.Electrum.Validation.Models;

namespace Vulpes.Liteyear.Domain.Models;

public record Resource : AggregateRoot
{
    public static Resource Empty { get; } = new();
    public static Resource Default => Empty;

    public IEnumerable<ContentReference> ContentReferences { get; init; } = [];
}