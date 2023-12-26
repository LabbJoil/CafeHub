using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CHDesktop.PersonalElements;

public partial class BannerTextboxControl : UserControl
{
    public string BannerText { get; set; } = string.Empty;
    public double NewHeight { get; set; } = 40;
    public double NewFontSize { get; set; } = 18;
    public TextWrapping Wrap { get; set; } = TextWrapping.NoWrap;
    public bool IsReadOnly { get; set; } = false;
    public VerticalAlignment VerticalContent { get; set; } = VerticalAlignment.Center;

    public BannerTextboxControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    private void BannerTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox nowTextBox = (TextBox)sender;
        if (string.IsNullOrEmpty(nowTextBox.Text)) BannerTextBlock.Visibility = Visibility.Visible;
        else BannerTextBlock.Visibility = Visibility.Hidden;
    }


    private void CustomTextBox_MouseEvent(object sender, MouseEventArgs e)
    {
        TextBox nowTextBox = (TextBox)sender;
        if (nowTextBox.IsMouseOver == true) BannerBorder.Background = new SolidColorBrush(Colors.LightGray);
        else BannerBorder.Background = new SolidColorBrush(Colors.White);
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

}
