using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Dashboard;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcDemo.Dashboard.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string serverStatus = "Server: Connecting...";
    
    private ObservableCollection<string> _logs;
    
    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetProperty(ref _logs, value);
    }
    
    public MainWindowViewModel()
    {
        Logs = [];
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        _ = Task.Run(ListenLoopAsync);
    }
    
    private async Task ListenLoopAsync()
    {
        while (true)
        {
            try
            {
                ServerStatus = "Server: Connecting...";
                Dispatcher.UIThread.Post(() => Logs.Clear());
                
                using var channel = GrpcChannel.ForAddress("http://localhost:50051");
                var client = new LogService.LogServiceClient(channel);
                var call = client.StreamLogs(new Empty());
                
                ServerStatus = "Server: Connected";
                
                await foreach (var entry in call.ResponseStream.ReadAllAsync())
                {
                    var line = $"[{entry.Time}] [{entry.Source}] {entry.Direction} - {entry.Message}";
                    
                    Dispatcher.UIThread.Post(() =>
                    {
                        Logs.Add(line);
                        if (Logs.Count > 500)
                            Logs.RemoveAt(0);
                    });
                }
                
                ServerStatus = "Server: Disconnected (stream ended)";
            }
            catch (Exception ex)
            {
                ServerStatus = "Server: Error – retrying...";
                Dispatcher.UIThread.Post(() => Logs.Add($"[ERROR] {ex.GetType().Name}: {ex.Message}"));

                await Task.Delay(2000);
            }
        }
    }
}