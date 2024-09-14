namespace NodeEditor.ViewModels.Pages;

public partial class ConnectionViewModel : ObservableObject
{
    [ObservableProperty]
    private ConnectorViewModel _source, _target;

    public ConnectionViewModel(ConnectorViewModel source, ConnectorViewModel target)
    {
        Source = source;
        Target = target;

        Source.IsConnected = true;
        Target.IsConnected = true;
    }
}
 