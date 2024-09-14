using NodeEditor.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NodeEditor.Views.Pages;

/// <summary>
/// Interaction logic for Editor.xaml
/// </summary>
public partial class EditorPage : INavigableView<EditorViewModel>
{
    public EditorViewModel ViewModel => (EditorViewModel)DataContext!;

    public EditorPage()
    {
        InitializeComponent();
    }
}
