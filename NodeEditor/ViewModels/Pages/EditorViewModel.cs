using System.Collections.ObjectModel;

namespace NodeEditor.ViewModels.Pages;

public partial class EditorViewModel : ObservableObject
{
    public PendingConnectionViewModel PendingConnection { get; }
    public ObservableCollection<NodeViewModel> Nodes { get; } = [];

    public ObservableCollection<ConnectionViewModel> Connections { get; } = [];
    public EditorViewModel()
    {
        PendingConnection = new (this);
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

        //Connections.Add(new ConnectionViewModel
        //(welcome.Outputs[0], nodify.Inputs[0]));
    }

    [RelayCommand]
    private void DisconnectConnector(ConnectorViewModel connector)
    {
        var connection = Connections.First(x => x.Source == connector || x.Target == connector);
        connection.Source.IsConnected = false;  // This is not correct if there are multiple connections to the same connector
        connection.Target.IsConnected = false;
        Connections.Remove(connection);
    }
}
