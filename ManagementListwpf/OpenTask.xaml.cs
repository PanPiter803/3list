using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ManagementListwpf
{
    /// <summary>
    /// Logika interakcji dla klasy AddTask.xaml
    /// </summary>
    public partial class OpenTask : Window
    {
        readonly BrushConverter brush = new();
        private readonly string Link;

        public OpenTask(string title, string description, string link, int list)
        {
            InitializeComponent();
            TaskStatus_Change(list);
            TitleText.Text = title;
            DescriptionText.Text = description;

            if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
            {
                LinkButton.Content = link;
                Link = link;
            }
            else
            {
                LinkButton.Visibility = Visibility.Hidden;
                LinkButtonBorder.Visibility = Visibility.Hidden;
            }

            if (!((MainWindow)Application.Current.MainWindow).LayoutLight)
            {
                WindowBackground.Background = (Brush)brush.ConvertFrom("#FF212226");

                TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                TitleText.Foreground = Brushes.White;

                DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                DescriptionText.Foreground = Brushes.White;

                TitleTask.Foreground = Brushes.White;
                DescriptionTask.Foreground = Brushes.White;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TaskStatus_Change(int list)
        {
            switch(list)
            {
                case 0:
                    TaskStatus.Content = "AKTUALNE ZADANIE";
                    break;
                case 1:
                    TaskStatus.Content = "WAŻNE ZADANIE";
                    break;
                case 2:
                    TaskStatus.Content = "ZAKOŃCZONE ZADANIE";
                    break;
                default:
                    break;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LinkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            LinkButtonBorder.Background = (Brush)brush.ConvertFrom("#FFAA0000");
        }

        private void LinkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            LinkButtonBorder.Background = (Brush)brush.ConvertFrom("#FFFF0000");
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = Link,
                UseShellExecute = true
            });
        }
    }
}
