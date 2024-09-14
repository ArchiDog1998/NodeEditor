using System.Collections.ObjectModel;

namespace NodeEditor.ViewModels.Pages;
public class NodeViewModel
{
    public string Title { get; set; } = string.Empty;

    public ObservableCollection<ConnectorViewModel> Inputs { get; set; } = [];
    public ObservableCollection<ConnectorViewModel> Outputs { get; set; } = [];
}
