using Grpc.Core;
using Serverstreaming;
using GrpcDemo.Server.Logging;

namespace GrpcDemo.Server.Services;

public class ServerStreamingService : ServerStreaming.ServerStreamingBase
{
    public override async Task GetServerResponse(
        Number request,
        IServerStreamWriter<Message> responseStream,
        ServerCallContext context)
    {
        var n = request.Value;
        LogManager.Log(source: "ServerStreaming", direction: "Info", message: $"Server processing gRPC server-streaming {{value = {n}}}.");
        
        var messages = new[]
        {
            "message #1",
            "message #2",
            "message #3",
            "message #4",
            "message #5"
        };
        
        foreach (var text in messages)
        {
            LogManager.Log(source: "ServerStreaming", direction: "Send", message: $"송신: \"{text}\"");
            var msg = new Message { Message_ = text };
            await responseStream.WriteAsync(msg);
        }
    }
}