using System.Collections.ObjectModel;

namespace NodeEditor.ViewModels.Pages;

public partial class NodeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;
    
    [ObservableProperty]
    private Point _location = new (100, 100);

    public ObservableCollection<ConnectorViewModel> Inputs { get; } = [];
    public ObservableCollection<ConnectorViewModel> Outputs { get; } = [];

    public NodeViewModel()
    {
        Inputs.CollectionChanged += (s, e) =>
        {
            if (e.NewItems is null) return;
            foreach (ConnectorViewModel item in e.NewItems)
            {
                item.IsIn = true;
                item.Node = this;
            }
        };
        Outputs.CollectionChanged += (s, e) =>
        {
            if (e.NewItems is null) return;
            foreach (ConnectorViewModel item in e.NewItems)
            {
                item.IsIn = false;
                item.Node = this;
            }
        };
    }
}
