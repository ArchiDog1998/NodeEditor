namespace NodeEditor.ViewModels.Pages;

public partial class PendingConnectionViewModel(EditorViewModel editor) : ObservableObject
{
  
    [ObservableProperty]
    private ConnectorViewModel? _source, _previewTarget;

    [ObservableProperty]
    private string? _content;
    
    partial void OnPreviewTargetChanged(ConnectorViewModel? value)
    {
        CanTarget(value, out var why);
        Content = why;
    }

    private bool CanTarget(ConnectorViewModel? target, out string? why)
    {
        if (Source is null)
        {
            why = "Source is null";
            return false;
        }

        if (target is null)
        {
            why = "Find a target!";
            return false;
        }

        if (Source == target)
        {
            why = "Target is the same as the source";
            return false;
        }

        if (Source.IsIn == target.IsIn)
        {
            why = target.IsIn 
                ? "Please find an output target"
                : "Please find an input target";
            return false;
        }

        if (target.Node == Source.Node)
        {
            why = "Please don't connect to the same node";
            return false;
        }

        why = $"Connect to {target.Title}";
        return true;
    }
    
    [RelayCommand]
    private void Finish(ConnectorViewModel? target)
    {
        if(Source is null || target is null)return;
        if (!CanTarget(target, out _)) return;
        var source = !Source.IsIn ? Source : target;
        var tar = Source.IsIn ? Source : target;
        editor.Connections.Add(new(source, tar));
    }
}
