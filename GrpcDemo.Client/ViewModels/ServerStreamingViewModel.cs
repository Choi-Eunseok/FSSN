using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Services;

namespace GrpcDemo.Client.ViewModels;

public partial class ServerStreamingViewModel : ViewModelBase
{
    private readonly ServerStreamingService _service;
    
    [ObservableProperty]
    private int inputNumber = 5;

    private ObservableCollection<string> _logs = [];
    
    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetProperty(ref _logs, value);
    }

    public ServerStreamingViewModel()
    {
        _service = new ServerStreamingService();
        _service.MessageReceived += (msg) =>
            Avalonia.Threading.Dispatcher.UIThread.Post(() => Logs.Add(msg));
    }
    
    [RelayCommand]
    private async Task Start()
    {
        await _service.StartStreamingAsync(inputNumber);
    }
}