using NodeEditor.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NodeEditor.Views.Pages;
public partial class DashboardPage : INavigableView<DashboardViewModel>
{
    public DashboardViewModel ViewModel => (DashboardViewModel)DataContext!;

    public DashboardPage()
    {
        InitializeComponent();
    }
}
