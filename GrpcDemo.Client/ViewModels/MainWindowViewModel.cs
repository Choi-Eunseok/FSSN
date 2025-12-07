using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Services;

namespace GrpcDemo.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly HelloGrpcService _helloGrpc = new();
    
    [ObservableProperty]
    private int inputValue;

    [ObservableProperty]
    private int resultValue;

    [RelayCommand]
    private async Task MyFunction()
    {
        ResultValue = await _helloGrpc.MyFunction(InputValue);
    }
    
}