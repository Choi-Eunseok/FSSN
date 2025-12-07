using GrpcDemo.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
    options.ListenLocalhost(50051, o =>  o.Protocols = HttpProtocols.Http2));

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<HelloGrpcService>();
app.MapGrpcService<BidirectionalService>();
app.MapGrpcService<ClientStreamingService>();
app.MapGrpcService<ServerStreamingService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();