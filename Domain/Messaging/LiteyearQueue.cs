namespace Vulpes.Liteyear.Domain.Messaging;

public sealed record LiteyearQueue
{
    public static LiteyearQueue Empty { get; } = new();
    public static LiteyearQueue Default => Empty;

    public const string QueuePrefix = "Q_LTYR_";

    public const string DeadLetterKey = "x-dead-letter-exchange";
    public string DeadLetterName => $"{Name}.DLQ";

    public string Identifier { get; init; } = string.Empty;
    public string Name => $"{QueuePrefix}{Identifier.Replace(QueuePrefix, string.Empty)}";
    public string RoutingKey => $"*.*.{Identifier}".ToLower();
    public string BuildRoute<TMessage>(TMessage message) where TMessage : LiteyearMessage => $"{message.Key}.{typeof(TMessage).Name}.{Identifier}".ToLower();
}
