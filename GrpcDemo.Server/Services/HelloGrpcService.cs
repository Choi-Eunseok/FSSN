namespace GrpcDemo.Server.Services;
using Grpc.Core;

public class HelloGrpcService : MyService.MyServiceBase
{
    public override Task<MyNumber> MyFunction(MyNumber request, ServerCallContext context)
    {
        var input = request.Value;
        return Task.FromResult(new MyNumber
        {
            Value = input * input
        });
    }
}