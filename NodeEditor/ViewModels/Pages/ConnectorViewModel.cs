namespace NodeEditor.ViewModels.Pages;
public partial class ConnectorViewModel : ObservableObject
{
    public NodeViewModel? Node { get; set; }
    public bool IsIn { get; set; }
    
    [ObservableProperty]
    private Point _anchor;

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private string _title = string.Empty;
}