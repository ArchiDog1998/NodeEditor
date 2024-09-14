using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace NodeEditor.ViewModels.Pages;

public partial class EditorViewModel : ObservableObject
{
    public enum EditState : byte
    {
        Unknown,
        Node,
        Viewport,
    }

    public PendingConnectionViewModel PendingConnection { get; }
    public ObservableCollection<NodeViewModel> Nodes { get; } = [];

    public ObservableCollection<ConnectionViewModel> Connections { get; } = [];

    public ObservableCollection<Visual3D> Geometries { get; } = [];

    public event Action<Visual3D[], Visual3D[]>? GeometryChanged;
    public event Action<EditState>? StateChanged;

    [NotifyPropertyChangedFor(nameof(StateString))]
    [ObservableProperty]
    private EditState _state = EditState.Unknown;

    public string StateString => State switch 
    { 
        EditState.Viewport => "3D Viewer",
        EditState.Node => "Node",
        _ => "Unknown",
    };

    public EditorViewModel()
    {
        PendingConnection = new (this);

        Geometries.CollectionChanged += (s, e) =>
        {
            var removedItems = e.OldItems?.OfType<Visual3D>().ToArray() ?? [];
            var addedItems = e.NewItems?.OfType<Visual3D>().ToArray() ?? [];
            GeometryChanged?.Invoke(removedItems, addedItems);
        };

#if DEBUG
        var welcome = new NodeViewModel
        {
            Title = "Welcome",
            Inputs =
            [
                new ConnectorViewModel
                {
                    Title = "In"
                }
            ],
            Outputs =
            [
                new ConnectorViewModel
                {
                    Title = "Out"
                }
            ]
        };

        var nodify = new NodeViewModel
        {
            Title = "To NodeEditor",
            Inputs =
            [
                new ConnectorViewModel
                {
                    Title = "In"
                }
            ]
        };

        Nodes.Add(welcome);
        Nodes.Add(nodify);
#endif
    }

    partial void OnStateChanged(EditState value)
    {
        StateChanged?.Invoke(value);
    }

    [RelayCommand]
    private void DisconnectConnector(ConnectorViewModel connector)
    {
        var connection = Connections.First(x => x.Source == connector || x.Target == connector);
        connection.Source.IsConnected = false;  // This is not correct if there are multiple connections to the same connector
        connection.Target.IsConnected = false;
        Connections.Remove(connection);
    }

    [RelayCommand]
    public void SwitchOrder()
    {
        State = State switch
        {
            EditState.Node => EditState.Viewport,
            _ => EditState.Node,
        };
    }
}
