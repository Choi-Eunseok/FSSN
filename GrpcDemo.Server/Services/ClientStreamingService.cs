using Grpc.Core;
using Clientstreaming;
using GrpcDemo.Server.Logging;

namespace GrpcDemo.Server.Services;

public class ClientStreamingService : ClientStreaming.ClientStreamingBase
{
    public override async Task<Number> GetServerResponse(
        IAsyncStreamReader<Message> requestStream,
        ServerCallContext context)
    {
        LogManager.Log(source: "ClientStreaming", direction: "Info", message: "Server processing gRPC client-streaming.");

        var count = 0;

        await foreach (var msg in requestStream.ReadAllAsync(context.CancellationToken))
        {
            count++;

            LogManager.Log(source: "ClientStreaming", direction: "Recv", message: $"#{count} 받은 메시지: \"{msg.Message_}\"");
        }

        var response = new Number { Value = count };

        LogManager.Log(source: "ClientStreaming", direction: "Send", message: $"총 메시지 개수 = {count}");

        return response;
    }
}