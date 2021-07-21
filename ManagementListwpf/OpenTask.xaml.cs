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
        private readonly int ID;
        private readonly string Link;

        public OpenTask(int id, string title, string description, string link, int list, int layout)
        {
            InitializeComponent();
            TaskStatus_Change(list);
            ID = id;
            TitleText.Text = title;
            DescriptionText.Text = description;

            switch(layout)
            {
                case 1:
                    LayoutColorBlack();
                    break;
                case 2:
                    LayoutColorYellow();
                    break;
                case 3:
                    LayoutColorBlue();
                    break;
                case 4:
                    LayoutColorBrown();
                    break;
                case 5:
                    LayoutColorPink();
                    break;
                case 6:
                    LayoutColorPurple();
                    break;
                case 7:
                    LayoutColorOrange();
                    break;
                case 8:
                    LayoutColorLightGreen();
                    break;
                case 9:
                    LayoutColorGreen();
                    break;
                default:
                    LayoutColorWhite();
                    break;
            }

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

        private void Layout_Click(object sender, RoutedEventArgs e)
        {
            ColorChange.Visibility = Visibility.Visible;
        }

        private void LayoutColorWhite_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorWhite();
        }
        private void LayoutColorBlack_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorBlack();
        }
        private void LayoutColorYellow_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorYellow();
        }
        private void LayoutColorBlue_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorBlue();
        }
        private void LayoutColorBrown_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorBrown();
        }
        private void LayoutColorPink_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorPink();
        }
        private void LayoutColorPurple_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorPurple();
        }
        private void LayoutColorOrange_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorOrange();
        }
        private void LayoutColorLightGreen_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorLightGreen();
        }
        private void LayoutColorGreen_Click(object sender, RoutedEventArgs e)
        {
            LayoutColorGreen();
        }

        private void LayoutColorWhite()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FFD4D4D4");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FFFFFFFF");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FFFFFFFF");
            BlackText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 0;
        }
        private void LayoutColorBlack()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FF212226");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF3a3b40");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF3a3b40");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 1;
        }
        private void LayoutColorYellow()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FFb4b414");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FFf0f05a");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FFf0f05a");
            BlackText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 2;
        }
        private void LayoutColorBlue()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FF0a0a87");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF1919be");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF1919be");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 3;
        }
        private void LayoutColorBrown()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FF9b5514");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF64320a");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF64320a");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 4;
        }
        private void LayoutColorPink()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FFff00ff");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FFb00db0");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FFb00db0");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 5;
        }
        private void LayoutColorPurple()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FF67087b");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF860c9c");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF860c9c");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 6;
        }
        private void LayoutColorOrange()
        {
            WindowBackground.Background = Brushes.Orange;
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FFea6b48");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FFea6b48");
            BlackText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 7;
        }
        private void LayoutColorLightGreen()
        {
            WindowBackground.Background = Brushes.LightGreen;
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF009f3c");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF009f3c");
            BlackText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 8;
        }
        private void LayoutColorGreen()
        {
            WindowBackground.Background = (Brush)brush.ConvertFrom("#FF13c613");
            TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF099c09");
            DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF099c09");
            WhiteText();
            var item = ((MainWindow)Application.Current.MainWindow).taskslist.Find(item => item.TaskID == ID);
            item.LayoutColor = 9;
        }
        private void WhiteText()
        {
            TitleTask.Foreground = Brushes.White;
            TitleText.Foreground = Brushes.White;
            DescriptionTask.Foreground = Brushes.White;
            DescriptionText.Foreground = Brushes.White;
            ColorChange.Visibility = Visibility.Hidden;
        }
        private void BlackText()
        {
            TitleTask.Foreground = Brushes.Black;
            TitleText.Foreground = Brushes.Black;
            DescriptionTask.Foreground = Brushes.Black;
            DescriptionText.Foreground = Brushes.Black;
            ColorChange.Visibility = Visibility.Hidden;
        }
    }
}
