namespace Vulpes.Liteyear.Domain.Messaging;

public abstract record LiteyearMessage()
{
    public Guid Key { get; init; } = Guid.Empty;
}
