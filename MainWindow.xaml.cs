using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Things_control
{

    public partial class MainWindow : Window
    {
        private List<Thing> things = new List<Thing>();
        private ITxt_work txt_work;
        private string path = "C://Users/alyon/source/repos/Things_control/things.txt";

        public MainWindow()
        {
            InitializeComponent();
            txt_work = new Txt_work(things, path);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            txt_work.Read_txt(path);
            List_of_things.ItemsSource = things;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var new_thing = new Add_thing();
            if (new_thing.ShowDialog() == true)
            {
                Thing newItem;
                if (new_thing.IsFood)
                {
                    newItem = new Food(
                        name: new_thing.ItemName,
                        quantity: new_thing.Quantity,
                        date: new_thing.Date,
                        type: new_thing.TypeOrDiscount.ToString()
                    );
                }

                else
                {
                    newItem = new Not_food(
                        name: new_thing.ItemName,
                        quantity: new_thing.Quantity,
                        date: new_thing.Date,
                        discount: (int)new_thing.TypeOrDiscount
                    );
                }

                txt_work.AddThing(newItem);
                List_of_things.Items.Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedThing = List_of_things.SelectedItem as Thing;

            if (selectedThing != null)
            {
                try
                {
                    txt_work.DeleteThingByName(selectedThing.Name);
                    List_of_things.Items.Refresh();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                MessageBox.Show("Не выбран товар, который нужно удалить.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // Вывод данных о товаре в зависимости от класса
        private void List_of_things_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List_of_things.SelectedItem is Not_food selectedNotFood)
            {
                DetailsText.Text = $"Название: {selectedNotFood.Name}\n" +
                                  $"Количество: {selectedNotFood.Quantity}\n" +
                                  $"Дата: {selectedNotFood.Date}\n" +
                                  $"Скидка: {selectedNotFood.Discount}%";
            }

            if (List_of_things.SelectedItem is Food selectedFood)
            {
                DetailsText.Text = $"Название: {selectedFood.Name}\n" +
                                  $"Количество: {selectedFood.Quantity}\n" +
                                  $"Дата: {selectedFood.Date}\n" +
                                  $"Тип: {selectedFood.Type}";
            }

            Details.Visibility = Visibility.Visible;
        }

        private void CloseDetails_Click(object sender, RoutedEventArgs e)
        {
            Details.Visibility = Visibility.Collapsed;
            List_of_things.SelectedItem = null;
        }

        private void Cmd_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var commandProcessor = new CommandProcessor();
                    commandProcessor.ProcessCommands(openFileDialog.FileName, things, txt_work);      
                    List_of_things.Items.Refresh();
                    txt_work.SaveDataToFile(path);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка выполнения команд: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}