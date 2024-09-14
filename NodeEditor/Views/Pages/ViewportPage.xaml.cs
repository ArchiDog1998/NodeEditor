using NodeEditor.ViewModels.Pages;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NodeEditor.Views.Pages;
/// <summary>
/// Interaction logic for ViewportPage.xaml
/// </summary>
public partial class ViewportPage : INavigableView<ViewportViewModel>
{
    public ViewportViewModel ViewModel => (ViewportViewModel)DataContext!;

    public ViewportPage()
    {
        InitializeComponent();
    }
}
