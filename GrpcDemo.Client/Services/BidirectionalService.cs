using System;
using System.Threading;
using System.Threading.Tasks;
using Bidirectional;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcDemo.Client.Services;

public class BidirectionalService : IAsyncDisposable
{
    private readonly Bidirectional.Bidirectional.BidirectionalClient _client;
    private AsyncDuplexStreamingCall<Message, Message>? _call;
    private readonly CancellationTokenSource _cts = new();
    
    public event Action<string>? MessageReceived;

    public BidirectionalService()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:50051");
        _client = new Bidirectional.Bidirectional.BidirectionalClient(channel);
    }
    
    public async Task StartAsync()
    {
        _call = _client.GetServerResponse(cancellationToken: _cts.Token);
        
        _ = Task.Run(async () =>
        {
            try
            {
                await foreach (var resp in _call.ResponseStream.ReadAllAsync(_cts.Token))
                {
                    var text = resp.Message_;
                    MessageReceived?.Invoke(text);
                }
            }
            catch (Exception ex)
            {
                MessageReceived?.Invoke($"[ERROR] {ex.Message}");
            }
        }, _cts.Token);
    }
    
    public async Task SendAsync(string text)
    {
        if (_call == null)
            throw new InvalidOperationException("Streaming call is not started. Call StartAsync() first.");

        await _call.RequestStream.WriteAsync(new Message { Message_ = text });
    }

    private async Task CompleteAsync()
    {
        if (_call != null)
            await _call.RequestStream.CompleteAsync();
        
        await _cts.CancelAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await CompleteAsync();
        _call?.Dispose();
        _cts.Dispose();
    }
}