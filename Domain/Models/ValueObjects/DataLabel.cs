namespace Vulpes.Liteyear.Domain.Models.ValueObjects;

public record DataLabel(string Value)
{
    public static implicit operator string(DataLabel label) => label.Value;
    public static implicit operator DataLabel(string label) => new(label);
}