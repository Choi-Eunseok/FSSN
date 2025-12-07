
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Views;

namespace GrpcDemo.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private UserControl currentView;
    
    public MainWindowViewModel()
    {
        CurrentView = new HelloGrpcView();
    }
    
    [RelayCommand]
    private async Task ShowHello()
    {
        if (CurrentView.DataContext is IAsyncDisposable disposable)
            await disposable.DisposeAsync();

        CurrentView = new HelloGrpcView();
    }
    
    [RelayCommand]
    private async Task ShowBidirectional()
    {
        if (CurrentView.DataContext is IAsyncDisposable disposable)
            await disposable.DisposeAsync();

        CurrentView = new BidirectionalView();
    }
    
    [RelayCommand]
    private async Task ShowClientStreaming()
    {
        if (CurrentView.DataContext is IAsyncDisposable disposable)
            await disposable.DisposeAsync();

        CurrentView = new ClientStreamingView();
    }
    
    [RelayCommand]
    private async Task ShowServerStreaming()
    {
        if (CurrentView.DataContext is IAsyncDisposable disposable)
            await disposable.DisposeAsync();

        CurrentView = new ServerStreamingView();
    }
}