namespace Vulpes.Liteyear.Domain.Logging;

public static class LogTags
{
    public static string ApplicationStart => $"[{nameof(ApplicationStart)}]";
    public static string ApplicationShutdown => $"[{nameof(ApplicationShutdown)}]";
    public static string Heartbeat => $"[{nameof(Heartbeat)}]";

    public static string EventingStartup => $"[{nameof(EventingStartup)}]";
    public static string EventingShutdown => $"[{nameof(EventingShutdown)}]";
    public static string PublisherStart => $"[{nameof(PublisherStart)}]";
    public static string ConsumerFatal => $"[{nameof(ConsumerFatal)}]";
    public static string ConsumerStart => $"[{nameof(ConsumerStart)}]";

    public static string MessageSuccess => $"[{nameof(MessageSuccess)}]";
    public static string MessageFailure => $"[{nameof(MessageFailure)}]";
}
