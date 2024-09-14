using HelixToolkit.Wpf;
using NodeEditor.ViewModels.Pages;
using Nodify;
using System.Windows.Media;
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

        ViewModel.GeometryChanged += (oldItems, newItems) =>
        {
            foreach (var item in oldItems)
            {
                View.Children.Remove(item);
            }
            foreach (var item in newItems)
            {
                View.Children.Add(item);
            }
        };

        ViewModel.StateChanged += (state) =>
        {
            var disabledOpacity = 0.5;

            var children = MainGrid.Children;
            var editor = children.OfType<NodifyEditor>().FirstOrDefault();
            var viewer = children.OfType<HelixViewport3D>().FirstOrDefault();
            if (editor is null || viewer is null) return;
            switch (state)
            {
                case EditorViewModel.EditState.Viewport:
                    viewer.Opacity = 1;
                    editor.Opacity = disabledOpacity;

                    children.Remove(viewer);
                    children.Insert(1, viewer);
                    break;

                case EditorViewModel.EditState.Node:
                    editor.Opacity = 1;
                    viewer.Opacity = disabledOpacity;

                    children.Remove(viewer);
                    children.Insert(0, viewer);
                    break;
            }
        };

        ViewModel.SwitchOrder();

#if DEBUG
        ViewModel.Geometries.Add(new HelixToolkit.Wpf.TorusVisual3D() { Material = new System.Windows.Media.Media3D.DiffuseMaterial(new SolidColorBrush(Color.FromRgb(100, 100, 100))) });
#endif
    }
}
