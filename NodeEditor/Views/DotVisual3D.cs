using System.Windows.Documents;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace NodeEditor.Views;

public class DotVisual3D : ScreenSpaceVisual3D
{
    private class DotGeometryBuilder(Visual3D visual) : ScreenGeometryBuilder(visual)
    {
        public static PointCollection CreateTextCoordinates(int n)
        {
            var coordinates = new PointCollection(n * 4);

            for (var i = 0; i < n; i++)
            {
                coordinates.Add(new Point(0, 0));
                coordinates.Add(new Point(0, 1));
                coordinates.Add(new Point(1, 0));
                coordinates.Add(new Point(1, 1));
            }

            coordinates.Freeze();
            return coordinates;
        }

        public static Int32Collection CreateIndices(int n)
        {
            var indices = new Int32Collection(n * 6);

            for (var i = 0; i < n; i++)
            {
                indices.Add(i * 4 + 0);
                indices.Add(i * 4 + 1);
                indices.Add(i * 4 + 3);

                indices.Add(i * 4 + 0);
                indices.Add(i * 4 + 3);
                indices.Add(i * 4 + 2);
            }

            indices.Freeze();
            return indices;
        }

        public Point3DCollection CreatePositions(IList<Point3D> points, double width, double height, double depthOffset)
        {
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            var outline = new[]
            {
                new Vector(-halfWidth, -halfHeight),
                new Vector(-halfWidth, halfHeight),
                new Vector(halfWidth, -halfHeight),
                new Vector(halfWidth, halfHeight),
            };

            var positions = new Point3DCollection(4 * points.Count);

            foreach (var point in points)
            {
                var screenPoint = (Point4D)point * this.visualToScreen;

                var spx = screenPoint.X;
                var spy = screenPoint.Y;
                var spz = screenPoint.Z;
                var spw = screenPoint.W;

                if (!depthOffset.Equals(0))
                {
                    spz -= depthOffset * spw;
                }

                var p0 = new Point4D(spx, spy, spz, spw) * this.screenToVisual;
                var pwInverse = 1 / p0.W;

                foreach (var v in outline)
                {
                    var p = new Point4D(spx + v.X * spw, spy + v.Y * spw, spz, spw) * this.screenToVisual;
                    positions.Add(new Point3D(p.X * pwInverse, p.Y * pwInverse, p.Z * pwInverse));
                }
            }

            return positions;
        }
    }

    public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
        nameof(FontFamily), typeof(FontFamily), typeof(DotVisual3D),
        new PropertyMetadata(default(FontFamily), MaterialChanged));

    public FontFamily? FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly DependencyProperty TextHeightProperty = DependencyProperty.Register(
        nameof(TextHeight), typeof(double), typeof(DotVisual3D), new PropertyMetadata(30d, MaterialChanged));

    public double TextHeight
    {
        get => (double)GetValue(TextHeightProperty);
        set => SetValue(TextHeightProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(DotVisual3D),
        new PropertyMetadata(new CornerRadius(0), MaterialChanged));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
        nameof(BorderThickness), typeof(Thickness), typeof(DotVisual3D),
        new PropertyMetadata(new Thickness(0), MaterialChanged));

    public Thickness BorderThickness
    {
        get => (Thickness)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register(
        nameof(BorderColor), typeof(Color), typeof(DotVisual3D), new PropertyMetadata(Colors.Black, MaterialChanged));

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
        nameof(FontSize), typeof(double), typeof(DotVisual3D), new PropertyMetadata(0d, MaterialChanged));

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(DotVisual3D), new PropertyMetadata("Dot", MaterialChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
        nameof(BackgroundColor), typeof(Color), typeof(DotVisual3D),
        new PropertyMetadata(Colors.Transparent, MaterialChanged));

    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    private static void MaterialChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ((DotVisual3D)sender).MaterialChanged();
    }

    private void MaterialChanged()
    {
        var textBlock = new System.Windows.Controls.TextBlock(new Run(Text))
        {
            Foreground = new SolidColorBrush(Color),
            Background = Brushes.Transparent,
        };
        if (FontSize > 0)
        {
            textBlock.FontSize = this.FontSize;
        }

        if (FontFamily != null)
        {
            textBlock.FontFamily = FontFamily;
        }

        var element = new System.Windows.Controls.Border
        {
            BorderBrush = new SolidColorBrush(BorderColor),
            Background = new SolidColorBrush(BackgroundColor),
            BorderThickness = BorderThickness,
            CornerRadius = CornerRadius,
            Child = textBlock,
        };

        element.Measure(new Size(1000, 1000));
        element.Arrange(new Rect(element.DesiredSize));
        element.RenderSize = element.DesiredSize;

        var rtb = new RenderTargetBitmap(
            (int)element.ActualWidth + 1, (int)element.ActualHeight + 1, 96, 96, PixelFormats.Pbgra32);
        rtb.Render(element);
        rtb.Freeze();
        _material = new EmissiveMaterial(new ImageBrush(rtb));
        _ratio = element.ActualWidth / element.ActualHeight;
    }

    private double _ratio = 1;
    private Material _material = new DiffuseMaterial(Brushes.Transparent);
    private readonly DotGeometryBuilder? _builder;

    public DotVisual3D()
    {
        _builder = new DotGeometryBuilder(this);
        MaterialChanged();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == ColorProperty)
        {
            MaterialChanged();
        }
    }

    protected override void UpdateGeometry()
    {
        if (this.Points == null || this._builder == null)
        {
            this.Mesh.Positions = null;
            return;
        }

        var height = TextHeight;
        var width = _ratio * height;

        var num = this.Points.Count;

        this.Mesh.Positions = this._builder.CreatePositions(this.Points, width, height, this.DepthOffset);
        this.Mesh.TriangleIndices = DotGeometryBuilder.CreateIndices(num);
        this.Mesh.TextureCoordinates = DotGeometryBuilder.CreateTextCoordinates(num);
        this.Model.Material = _material;
    }

    protected override bool UpdateTransforms()
    {
        return this._builder?.UpdateTransforms() ?? false;
    }
}