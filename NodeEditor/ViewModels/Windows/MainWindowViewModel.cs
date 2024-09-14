using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace NodeEditor.ViewModels.Windows;
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "Node Editor";
}
