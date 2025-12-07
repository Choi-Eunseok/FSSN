using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GrpcDemo.Client.Services;

public class HelloGrpcService
{
    private readonly MyService.MyServiceClient _client;

    public HelloGrpcService()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:50051");
        _client = new MyService.MyServiceClient(channel);
    }

    public async Task<int> MyFunction(int value)
    {
        var request = new MyNumber { Value = value };
        var reply = await _client.MyFunctionAsync(request);
        return reply.Value;
    }
}