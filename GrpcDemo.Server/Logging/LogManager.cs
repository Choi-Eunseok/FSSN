using System.Collections.Concurrent;
using System.Threading.Channels;
using Dashboard;

namespace GrpcDemo.Server.Logging;

public static class LogManager
{
    private static readonly ConcurrentDictionary<Guid, ChannelWriter<LogEntry>> Subscribers = new();
    
    public static void Log(string source, string direction, string message)
    {
        var entry = new LogEntry
        {
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Source = source,
            Direction = direction,
            Message = message
        };
        
        foreach (var subscriber in Subscribers)
        {
            subscriber.Value.TryWrite(entry);
        }
    }
    
    public static ChannelReader<LogEntry> Subscribe(Guid connectionId)
    {
        var channel = Channel.CreateUnbounded<LogEntry>();
        
        Subscribers.TryAdd(connectionId, channel.Writer);
        
        return channel.Reader;
    }
    
    public static void Unsubscribe(Guid connectionId)
    {
        if (Subscribers.TryRemove(connectionId, out var writer))
            writer.TryComplete();
    }
}