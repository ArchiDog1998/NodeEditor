namespace NodeEditor.ViewModels.Pages;

public partial class PendingConnectionViewModel(EditorViewModel editor) : ObservableObject
{
    private readonly EditorViewModel _editor = editor;
    private ConnectorViewModel? _source;

    [RelayCommand]
    private void Start(ConnectorViewModel? source)
    {
        _source = source;
    }

    [RelayCommand]
    private void Finish(ConnectorViewModel? target)
    {
        if (target is null || _source is null) return;
        _editor.Connections.Add(new(_source, target));
    }
}
