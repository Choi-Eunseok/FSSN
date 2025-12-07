using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Services;

namespace GrpcDemo.Client.ViewModels;

public partial class ClientStreamingViewModel : ViewModelBase
{
    private readonly ClientStreamingService _service = new();

    [ObservableProperty]
    private string countText = "5";
    
    private ObservableCollection<ClientStreamingMessageItemViewModel> _messages = [];
    
    public ObservableCollection<ClientStreamingMessageItemViewModel> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }
    
    private ObservableCollection<string> _logs = [];
    
    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetProperty(ref _logs, value);
    }
    
    [RelayCommand]
    private void GenerateMessages() => GenerateMultipleMessages();
    [RelayCommand]
    private async Task SendAll() => await SendAllAsync();

    private void GenerateMultipleMessages()
    {
        var n = int.Parse(countText);
        Messages.Clear();

        for (var i = 1; i <= n; i++)
        {
            Messages.Add(new ClientStreamingMessageItemViewModel
            {
                Index = i,
                Text = $"message #{i}"
            });
        }
    }

    private async Task SendAllAsync()
    {
        var texts = Messages
            .Select(m => m.Text)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();
        
        foreach (var t in texts)
        {
            Logs.Add($"[client to server] {t}");
        }
        
        try
        {
            var count = await _service.SendMessagesAsync(texts);
            
            Logs.Add($"[server to client] value = {count}");
        }
        catch (Exception ex)
        {
            Logs.Add($"[ERROR] 전송 실패: {ex.Message}");
        }
    }
}
