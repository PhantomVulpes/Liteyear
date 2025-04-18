namespace Vulpes.Liteyear.Domain.Models.ValueObjects;

public record ContentReferenceIndex(string Value)
{
    public static implicit operator string(ContentReferenceIndex index) => index.Value;
    public static implicit operator ContentReferenceIndex(string index) => new(index);
}