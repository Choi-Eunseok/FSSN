using CommunityToolkit.Mvvm.ComponentModel;

namespace GrpcDemo.Client.ViewModels;

public partial class ClientStreamingMessageItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private string text = string.Empty;
}