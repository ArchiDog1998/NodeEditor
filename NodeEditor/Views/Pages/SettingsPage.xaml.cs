using NodeEditor.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NodeEditor.Views.Pages;
public partial class SettingsPage : INavigableView<SettingsViewModel>
{
    public SettingsViewModel ViewModel => (SettingsViewModel)DataContext!;

    public SettingsPage()
    {
        InitializeComponent();
    }
}
