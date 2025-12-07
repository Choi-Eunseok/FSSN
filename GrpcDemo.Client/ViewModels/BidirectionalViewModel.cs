using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Services;

namespace GrpcDemo.Client.ViewModels;

public partial class BidirectionalViewModel : ViewModelBase, IAsyncDisposable
{
    private readonly BidirectionalService _service = new();
    
    [ObservableProperty]
    private string message1 = "message #1";
    
    [ObservableProperty]
    private string message2 = "message #2";
    
    [ObservableProperty]
    private string message3 = "message #3";
    
    [ObservableProperty]
    private string message4 = "message #4";
    
    [ObservableProperty]
    private string message5 = "message #5";
    
    private ObservableCollection<string> _logs = [];
    
    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetProperty(ref _logs, value);
    }
    
    [RelayCommand]
    private async Task SendAll()
    {
        await SendAllAsync();
    }

    public BidirectionalViewModel()
    {
        _service.MessageReceived += text =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                Logs.Add($"[server to client] {text}");
            });
        };
        
        _ = _service.StartAsync();
    }

    private async Task SendAllAsync()
    {
        var messages = new[]
        {
            message1,
            message2,
            message3,
            message4,
            message5
        };
        
        foreach (var msg in messages)
        {
            Logs.Add($"[client to server] {msg}");
            await _service.SendAsync(msg);
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        try
        {
            await _service.DisposeAsync();
        }
        catch (Exception ex)
        {
            Logs.Add($"[ERROR] Dispose failed: {ex.Message}");
        }
    }
}
