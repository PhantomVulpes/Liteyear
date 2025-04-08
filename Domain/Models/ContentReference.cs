using Vulpes.Electrum.Validation.Models;
using Vulpes.Liteyear.Domain.Models.ValueObjects;

namespace Vulpes.Liteyear.Domain.Models;

public record ContentReference : AggregateRoot
{
    public static ContentReference Empty { get; } = new();
    public static ContentReference Default => Empty;

    public string DuraluminKey { get; init; } = string.Empty;
    public IEnumerable<DataLabel> DataLabels { get; init; } = [];
    public ContentReferenceIndex Index { get; init; } = new(string.Empty);
}