using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcDemo.Client.Services;

namespace GrpcDemo.Client.ViewModels;

public partial class HelloGrpcViewModel : ViewModelBase
{
    private readonly HelloGrpcService _helloGrpc = new();
    
    [ObservableProperty]
    private int inputValue;

    [ObservableProperty]
    private int resultValue;

    [RelayCommand]
    private async Task MyFunction()
    {
        resultValue = await _helloGrpc.MyFunction(inputValue);
    }
}