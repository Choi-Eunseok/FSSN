
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GrpcDemo.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase currentView;
    
    public HelloGrpcViewModel HelloGrpc { get; }
    public BidirectionalViewModel Bidirectional { get; }
    
    public IRelayCommand ShowHelloCommand { get; }
    public IRelayCommand ShowBidirectionalCommand { get; }
    
    public MainWindowViewModel()
    {
        HelloGrpc = new HelloGrpcViewModel();
        Bidirectional = new BidirectionalViewModel();
        
        CurrentView = HelloGrpc;
        
        ShowHelloCommand = new RelayCommand(() => CurrentView = HelloGrpc);
        ShowBidirectionalCommand = new RelayCommand(() => CurrentView = Bidirectional);
    }
}