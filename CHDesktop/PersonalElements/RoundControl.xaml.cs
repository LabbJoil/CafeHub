using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CHDesktop.PersonalElements;

public partial class RoundControl : UserControl
{
    public ImageSource? ImageLeaveButton { get; set; }
    public ImageSource? ImageEnterButton { get; set; }
    public ImageSource? ImageStaticButton { get; set; }
    public event RoutedEventHandler? Click;

    public string? Text { get; set; }
    public new double Height { get; set; }
    public new double Width { get; set; }
    public float StaticImageOpacity { get; set; } = 1.0f;
    public float LEImageOpacity { get; set; } = 1.0f;
    public new double FontSize { get; set; } = 11;

    public RoundControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void ChangeFullSize(double newSize, double newSizeFont)
    {
        Height = newSize; Width = newSize;
        RoundButton.Height = newSize;
        RoundButton.Width = newSize;
        RoundButton.FontSize = newSizeFont;
        Height = newSize;
        Width = newSize;
    }

    private void RoundButton_MouseLeftButtonDown(object sender, RoutedEventArgs e) 
        => Click?.Invoke(this, e);

    public void UpdateImageStatic(ImageSource imageSource)
    {
        Ellipse staticEllipse = (Ellipse)RoundButton.Template.FindName("StaticEllipse", RoundButton);
        if (staticEllipse != null)
            if (staticEllipse.Fill is ImageBrush staticImageBrush)
            {
                staticImageBrush.ImageSource = imageSource;
                ImageStaticButton = imageSource;
            }
    }
}

