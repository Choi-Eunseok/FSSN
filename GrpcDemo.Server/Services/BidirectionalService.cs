using Grpc.Core;
using Bidirectional;
using GrpcDemo.Server.Logging;

namespace GrpcDemo.Server.Services;

public class BidirectionalService : Bidirectional.Bidirectional.BidirectionalBase
{
    public override async Task GetServerResponse(
        IAsyncStreamReader<Message> requestStream,
        IServerStreamWriter<Message> responseStream,
        ServerCallContext context)
    {
        LogManager.Log(source: "Bidirectional", direction: "Info", message: "Bidirectional GetServerResponse 시작");

        await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
        {
            var input = request.Message_;
            LogManager.Log(source: "Bidirectional", direction: "Recv", message: $"수신: \"{input}\"");

            var response = new Message { Message_ = input };
            LogManager.Log(source: "Bidirectional", direction: "Send", message: $"송신: \"{response.Message_}\"");

            await responseStream.WriteAsync(response);
        }

        LogManager.Log(source: "Bidirectional", direction: "Info", message: "Bidirectional GetServerResponse 종료");
    }
}