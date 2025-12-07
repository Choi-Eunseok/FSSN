using Grpc.Core;
using GrpcDemo.Server.Logging;

namespace GrpcDemo.Server.Services;

public class HelloGrpcService : MyService.MyServiceBase
{
    public override Task<MyNumber> MyFunction(MyNumber request, ServerCallContext context)
    {
        var input = request.Value;
        LogManager.Log(source: "MyService", direction: "Recv", message: $"MyFunction({input}) 호출");
        
        var result = input * input;
        LogManager.Log(source: "MyService", direction: "Send", message: $"MyFunction 결과 = {result}");
        
        return Task.FromResult(new MyNumber { Value = result });
    }
}