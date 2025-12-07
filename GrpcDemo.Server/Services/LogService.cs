using Grpc.Core;
using GrpcDemo.Server.Logging;
using Dashboard;

namespace GrpcDemo.Server.Services;

public class LogServiceImpl : LogService.LogServiceBase
{
    public override async Task StreamLogs(
        Google.Protobuf.WellKnownTypes.Empty request,
        IServerStreamWriter<LogEntry> responseStream,
        ServerCallContext context)
    {
        var connectionId = Guid.NewGuid();
        var reader = LogManager.Subscribe(connectionId);
        
        try
        {
            await foreach (var entry in reader.ReadAllAsync(context.CancellationToken))
            {
                await responseStream.WriteAsync(entry);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            LogManager.Unsubscribe(connectionId);
        }
    }
}