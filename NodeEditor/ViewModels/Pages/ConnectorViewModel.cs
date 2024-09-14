namespace NodeEditor.ViewModels.Pages;
public partial class ConnectorViewModel : ObservableObject
{
    [ObservableProperty]
    private Point _anchor;

    [ObservableProperty]
    private bool _isConnected;

    public string Title { get; set; } = string.Empty;
}