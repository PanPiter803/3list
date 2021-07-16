using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ManagementListwpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Tasks> taskslist = new();
        private ListBoxItem Item = null;

        private readonly List<OpenTask> openedTasks = new();

        public bool LayoutLight = true;
        readonly BrushConverter brush = new();

        public MainWindow()
        {
            InitializeComponent();
            ButtonStateCheck();
        }

        private void ButtonStateCheck()
        {
            if (CurrentList.SelectedIndex <= -1 && ImportantList.SelectedIndex <= -1 && FinishedList.SelectedIndex <= -1)
            {
                EditTask.IsEnabled = false;
                RemoveTask.IsEnabled = false;
            }
            else if (FinishedList.SelectedIndex > -1)
            {
                EditTask.IsEnabled = false;
                RemoveTask.IsEnabled = true;
            }
            else
            {
                EditTask.IsEnabled = true;
                RemoveTask.IsEnabled = true;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeProgram_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) this.WindowState = WindowState.Normal;
            else this.WindowState = WindowState.Maximized;
        }

        private void MinimizeProgram_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTask task = new();
            task.ShowDialog();

            CurrentList_ClearSelection();
            ImportantList_ClearSelection();
            FinishedList_ClearSelection();
            ButtonStateCheck();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var item = taskslist.Find(item => item.TitleOfTask == Item.Content.ToString());
            AddTask open = new(Item, item.TaskID, item.TitleOfTask, item.DescriptionOfTask, item.LinkOfTask);
            Item = null;

            CurrentList_ClearSelection();
            ImportantList_ClearSelection();
            FinishedList_ClearSelection();
            ButtonStateCheck();

            open.ShowDialog();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            var item = taskslist.Find(item => item.TitleOfTask == Item.Content.ToString());
            switch (item.ListNameOfTask)
            {
                case 0:
                    CurrentList.Items.Remove(Item);
                    break;
                case 1:
                    ImportantList.Items.Remove(Item);
                    break;
                case 2:
                    FinishedList.Items.Remove(Item);
                    break;
                default:
                    break;
            }

            Item = null;

            CurrentList_ClearSelection();
            ImportantList_ClearSelection();
            FinishedList_ClearSelection();
            ButtonStateCheck();
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialog = MessageBox.Show("Opcja 'Zapisz zadania' zastąpi poprzedno zapisane zadania obecnymi. Kontynuować?", "Ostrzeżenie", MessageBoxButton.OKCancel);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Tasks.txt";

            if (File.Exists(path))
            {
                if (dialog == MessageBoxResult.OK) Save(path);
            }
            else Save(path);
        }

        private void Save(string path)
        {
            try
            {
                using StreamWriter save = new(path);

                foreach (Tasks tasks in taskslist)
                    save.WriteLine(tasks.TitleOfTask + ";" + tasks.DescriptionOfTask + ";" + tasks.LinkOfTask + ";" + tasks.ListNameOfTask);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Wystąpił błąd: " + exp.Message, "Błąd");
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialog = MessageBox.Show("Opcja 'Wczytaj zadania' wczyta wszystkie zapisane zadania i zastąpi obecne. Kontynuować?", "Ostrzeżenie", MessageBoxButton.OKCancel);
            if (dialog == MessageBoxResult.OK)
            {
                CurrentList.Items.Clear();
                ImportantList.Items.Clear();
                FinishedList.Items.Clear();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Tasks.txt";

                try
                {
                    using StreamReader reader = new(path);
                    string line;
                    List<String> linesplit = new();

                    while ((line = reader.ReadLine()) != null)
                    {
                        linesplit = line.Split(';').ToList();
                        CreateTask(linesplit[0], linesplit[1], linesplit[2], Int32.Parse(linesplit[3]));
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Wystąpił błąd: " + exp.Message);
                }
            }
        }

        public void Edit(ListBoxItem editingtask, int ID, string title, string description, string link)
        {
            var item = taskslist.Find(item => item.TaskID == ID);
            editingtask.Content = title;

            taskslist[ID].TitleOfTask = title;
            taskslist[ID].DescriptionOfTask = description;
            taskslist[ID].LinkOfTask = link;
        }

        public void CreateTask(string title, string description, string link, int list)
        {
            ListBoxItem creatingtask = new();
            creatingtask.Content = title;

            taskslist.Add(new Tasks());
            taskslist[^1].TaskID = taskslist.Count() - 1;
            taskslist[^1].TitleOfTask = title;
            taskslist[^1].DescriptionOfTask = description;
            taskslist[^1].LinkOfTask = link;
            switch (list)
            {
                case 0:
                    taskslist[^1].ListNameOfTask = 0;
                    CurrentList.Items.Add(creatingtask);
                    break;
                case 1:
                    taskslist[^1].ListNameOfTask = 1;
                    ImportantList.Items.Add(creatingtask);
                    break;
                case 2:
                    taskslist[^1].ListNameOfTask = 2;
                    FinishedList.Items.Add(creatingtask);
                    break;
                default:
                    break;
            }
        }

        private void OpenTask(ListBoxItem selectedItem)
        {
            try
            {
                var item = taskslist.Find(item => item.TitleOfTask == selectedItem.Content.ToString());
                openedTasks.Add(new OpenTask(item.TitleOfTask, item.DescriptionOfTask, item.LinkOfTask, item.ListNameOfTask));
                openedTasks[^1].Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

        }

        //ListBoxes logic

        Point CurrentListStartMousePos;
        Point ImportantListStartMousePos;
        Point FinishedListStartMousePos;

        private void CurrentList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurrentListStartMousePos = e.GetPosition(null);
        }

        private void ImportantList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImportantListStartMousePos = e.GetPosition(null);
        }

        private void FinishedList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FinishedListStartMousePos = e.GetPosition(null);
        }

        private void CurrentList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listItem)
            {
                CurrentList.Items.Add(listItem);
                taskslist.Where(x => x.TitleOfTask == listItem.Content.ToString()).ToList().ForEach(s => s.ListNameOfTask = 0);
            }
        }

        private void ImportantList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listItem)
            {
                ImportantList.Items.Add(listItem);
                taskslist.Where(x => x.TitleOfTask == listItem.Content.ToString()).ToList().ForEach(s => s.ListNameOfTask = 1);
            }
        }

        private void FinishedList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listItem)
            {
                FinishedList.Items.Add(listItem);
                taskslist.Where(x => x.TitleOfTask == listItem.Content.ToString()).ToList().ForEach(s => s.ListNameOfTask = 2);
            }
        }

        private void CurrentList_MouseMove(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem selectedItem = (ListBoxItem)CurrentList.SelectedItem;
                    CurrentList.Items.Remove(selectedItem);

                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, selectedItem), DragDropEffects.Copy);

                    if (selectedItem.Parent == null)
                    {
                        CurrentList.Items.Add(selectedItem);
                    }
                }
                catch { }
            }
        }

        private void ImportantList_MouseMove(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem selectedItem = (ListBoxItem)ImportantList.SelectedItem;
                    ImportantList.Items.Remove(selectedItem);

                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, selectedItem), DragDropEffects.Copy);

                    if (selectedItem.Parent == null)
                    {
                        ImportantList.Items.Add(selectedItem);
                    }
                }
                catch { }
            }
        }

        private void FinishedList_MouseMove(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem selectedItem = (ListBoxItem)FinishedList.SelectedItem;
                    FinishedList.Items.Remove(selectedItem);

                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, selectedItem), DragDropEffects.Copy);

                    if (selectedItem.Parent == null)
                    {
                        FinishedList.Items.Add(selectedItem);
                    }
                }
                catch { }
            }
        }

        private void CurrentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)CurrentList.SelectedItem;
            OpenTask(selectedItem);
        }

        private void ImportantList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)ImportantList.SelectedItem;
            OpenTask(selectedItem);
        }

        private void FinishedList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)FinishedList.SelectedItem;
            OpenTask(selectedItem);
        }

        private void CurrentList_ClearSelection()
        {
            CurrentList.SelectedIndex = -1;
        }

        private void ImportantList_ClearSelection()
        {
            ImportantList.SelectedIndex = -1;
        }

        private void FinishedList_ClearSelection()
        {
            FinishedList.SelectedIndex = -1;
        }

        private void CurrentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentList.SelectedIndex > -1)
            {
                ImportantList_ClearSelection();
                FinishedList_ClearSelection();
                ButtonStateCheck();
                Item = (ListBoxItem)CurrentList.SelectedItem;
            }
        }

        private void ImportantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImportantList.SelectedIndex > -1)
            {
                CurrentList_ClearSelection();
                FinishedList_ClearSelection();
                ButtonStateCheck();
                Item = (ListBoxItem)ImportantList.SelectedItem;
            }
        }

        private void FinishedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FinishedList.SelectedIndex > -1)
            {
                ImportantList_ClearSelection();
                CurrentList_ClearSelection();
                ButtonStateCheck();
                Item = (ListBoxItem)FinishedList.SelectedItem;
            }
        }

        //Application Design
        private void ChangeLayout(object sender, RoutedEventArgs e)
        {
            if (LayoutLight)
            {
                ChangeLayoutBorder.Background = Brushes.Black;
                WindowBorder.Background = (Brush)brush.ConvertFrom("#FF212226");
                ListsBorder.Background = (Brush)brush.ConvertFrom("#FF303136");

                CurrentTaskTitle.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                ImportantTaskTitle.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                FinishedTaskTitle.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");

                CurrentListBorder.Background = (Brush)brush.ConvertFrom("#FF454651");
                ImportantListBorder.Background = (Brush)brush.ConvertFrom("#FF454651");
                FinishedListBorder.Background = (Brush)brush.ConvertFrom("#FF454651");

                AddTask.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                AddTaskText.FontWeight = FontWeights.Normal;
                EditTask.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                EditTaskText.FontWeight = FontWeights.Normal;
                RemoveTask.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                RemoveTaskText.FontWeight = FontWeights.Normal;
                SaveFile.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                SaveFileText.FontWeight = FontWeights.Normal;
                LoadFile.Foreground = (Brush)brush.ConvertFrom("#FFf2f3f5");
                LoadFileText.FontWeight = FontWeights.Normal;

                foreach (OpenTask o in openedTasks)
                {
                    o.WindowBackground.Background = (Brush)brush.ConvertFrom("#FF212226");

                    o.TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                    o.TitleText.Foreground = Brushes.White;

                    o.DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
                    o.DescriptionText.Foreground = Brushes.White;

                    o.TitleTask.Foreground = Brushes.White;
                    o.DescriptionTask.Foreground = Brushes.White;
                }

                LayoutLight = false;
            }
            else
            {
                ChangeLayoutBorder.Background = Brushes.White;
                WindowBorder.Background = (Brush)brush.ConvertFrom("#FFD4D4D4");
                ListsBorder.Background = (Brush)brush.ConvertFrom("#FFf2f3f5");

                CurrentTaskTitle.Foreground = Brushes.Black;
                ImportantTaskTitle.Foreground = Brushes.Black;
                FinishedTaskTitle.Foreground = Brushes.Black;

                CurrentListBorder.Background = Brushes.White;
                ImportantListBorder.Background = Brushes.White;
                FinishedListBorder.Background = Brushes.White;

                AddTask.Foreground = Brushes.Black;
                AddTaskText.FontWeight = FontWeights.Bold;
                EditTask.Foreground = Brushes.Black;
                EditTaskText.FontWeight = FontWeights.Bold;
                RemoveTask.Foreground = Brushes.Black;
                RemoveTaskText.FontWeight = FontWeights.Bold;
                SaveFile.Foreground = Brushes.Black;
                SaveFileText.FontWeight = FontWeights.Bold;
                LoadFile.Foreground = Brushes.Black;
                LoadFileText.FontWeight = FontWeights.Bold;

                foreach (OpenTask o in openedTasks)
                {
                    o.WindowBackground.Background = (Brush)brush.ConvertFrom("#FFf2f3f5");

                    o.TitleTextBorder.Background = (Brush)brush.ConvertFrom("#FFFFFFFF");
                    o.TitleText.Foreground = Brushes.Black;

                    o.DescriptionTextBorder.Background = (Brush)brush.ConvertFrom("#FFFFFFFF");
                    o.DescriptionText.Foreground = Brushes.Black;

                    o.TitleTask.Foreground = Brushes.Black;
                    o.DescriptionTask.Foreground = Brushes.Black;
                }

                LayoutLight = true;
            }
        }

        private void MinimizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) MinimizeButton.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else MinimizeButton.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void MaximizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) MaximizeButton.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else MaximizeButton.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseButton.Background = (Brush)brush.ConvertFrom("#FFFF0000");
        }

        private void MinimizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            MinimizeButton.Background = Brushes.Transparent;
        }

        private void MaximizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            MaximizeButton.Background = Brushes.Transparent;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseButton.Background = Brushes.Transparent;
        }

        private void ChangeLayoutBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) ChangeLayoutBorder.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else ChangeLayoutBorder.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void ChangeLayoutBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            if (LayoutLight) ChangeLayoutBorder.Background = (Brush)brush.ConvertFrom("#FFFFFFFF");
            else ChangeLayoutBorder.Background = (Brush)brush.ConvertFrom("#FF212226");
        }

        private void AddTask_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) AddTask.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else AddTask.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void AddTask_MouseLeave(object sender, MouseEventArgs e)
        {
            AddTask.Background = Brushes.Transparent;
        }

        private void EditTask_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) EditTask.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else EditTask.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void EditTask_MouseLeave(object sender, MouseEventArgs e)
        {
            EditTask.Background = Brushes.Transparent;
        }

        private void RemoveTask_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) RemoveTask.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else RemoveTask.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void RemoveTask_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveTask.Background = Brushes.Transparent;
        }

        private void SaveFile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) SaveFile.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else SaveFile.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void SaveFile_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveFile.Background = Brushes.Transparent;
        }

        private void LoadFile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LayoutLight) LoadFile.Background = (Brush)brush.ConvertFrom("#FFABABAB");
            else LoadFile.Background = (Brush)brush.ConvertFrom("#FF303136");
        }

        private void LoadFile_MouseLeave(object sender, MouseEventArgs e)
        {
            LoadFile.Background = Brushes.Transparent;
        }
    }

    public class Tasks
    {
        public int TaskID { get; set; }
        public string TitleOfTask { get; set; }
        public string DescriptionOfTask { get; set; }
        public string LinkOfTask { get; set; }
        public int ListNameOfTask { get; set; }
    }
}
