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

namespace ManagementListwpf
{
    /// <summary>
    /// Logika interakcji dla klasy AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        private readonly ListBoxItem Item;
        private readonly int TaskID;
        private readonly bool editing;

        readonly BrushConverter brush = new();

        public AddTask()
        {
            InitializeComponent();
            SetButtonsCursor();
            editing = false;
        }

        public AddTask(ListBoxItem item, int ID, string title, string description, string link)
        {
            InitializeComponent();
            SetButtonsCursor();
            CheckBox.IsEnabled = false;
            CheckBox.Visibility = Visibility.Hidden;

            Item = item;
            TitleText.Text = title;
            DescriptionText.Text = description;
            LinkText.Text = link;
            TaskID = ID;
            Create.Content = "Edytuj";
            editing = true;
        }

        private void SetButtonsCursor()
        {
            Create.Cursor = Cursors.Hand;
            Cancel.Cursor = Cursors.Hand;
            CheckBox.Cursor = Cursors.Hand;

            if (!((MainWindow)Application.Current.MainWindow).LayoutLight)
            {
                WindowBackground.Background = (Brush)brush.ConvertFrom("#FF212226");
                TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                TitleText.Foreground = Brushes.White;
                DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                DescriptionText.Foreground = Brushes.White;
                LinkTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                LinkText.Foreground = Brushes.White;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!editing) ((MainWindow)Application.Current.MainWindow).CreateTask(Titlee(), Description(), Link(), Important());
            else ((MainWindow)Application.Current.MainWindow).Edit(Item, TaskID, Titlee(), Description(), Link());
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string Titlee()
        {
            return TitleText.Text;
        }

        private string Description()
        {
            return DescriptionText.Text;
        }

        private string Link()
        {
            return LinkText.Text;
        }

        private int Important()
        {
            if (CheckBox.IsChecked == true) return 1;
            else return 0;
        }

        private void Create_MouseEnter(object sender, MouseEventArgs e)
        {
            CreateBorder.Background = (Brush)brush.ConvertFrom("#FFAA0000");
        }

        private void Create_MouseLeave(object sender, MouseEventArgs e)
        {
            CreateBorder.Background = (Brush)brush.ConvertFrom("#FFFF0000");
        }

        private void Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            CancelBorder.Background = (Brush)brush.ConvertFrom("#FFAA0000");
        }

        private void Cancel_MouseLeave(object sender, MouseEventArgs e)
        {
            CancelBorder.Background = (Brush)brush.ConvertFrom("#FFFF0000");
        }
    }
}
