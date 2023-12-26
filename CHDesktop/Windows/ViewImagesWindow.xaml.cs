using CHDesktop.Models.Domain;
using CHDesktop.Services.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CHDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для ViewImagesWindow.xaml
    /// </summary>
    public partial class ViewImagesWindow : Window
    {
        private readonly List<Attachment> Attachments;
        private int NumAtach = 0;

        public ViewImagesWindow(List<Attachment> attachments)
        {
            Attachments = attachments;
            InitializeComponent();
            ImageI.Source = WindowHelper.ConvertByteToImage(Attachments[NumAtach].ImageBytes);
        }

        private void PreviousImageRC_Click(object sender, RoutedEventArgs e)
        {
            
            if (NumAtach - 1 == -1)
                return;
            NumAtach--;
            ImageI.Source = WindowHelper.ConvertByteToImage(Attachments[NumAtach].ImageBytes);
        }

        private void NextImageRC_Click(object sender, RoutedEventArgs e)
        {
            if (NumAtach+1 == Attachments.Count)
                return;
            NumAtach++;
            ImageI.Source = WindowHelper.ConvertByteToImage(Attachments[NumAtach].ImageBytes);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
