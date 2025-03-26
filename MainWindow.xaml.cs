using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Things_control
{

    public partial class MainWindow : Window
    {
        List<Thing> things = new List<Thing>();
        string path = "C://Users/alyon/source/repos/Things_control/things.txt";
        private ITxt_work txt_work;

        public MainWindow()
        {
            InitializeComponent();
            txt_work = new Txt_work(things, path);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                things.Clear();
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("food"))
                        {
                            var food = Thing.Read(line);
                            things.Add(food);
                        }
                        else if (!line.StartsWith("food"))
                        {
                            var notFood = Thing.Read(line);
                            things.Add(notFood);
                        }
                    }
                }

                List_of_things.ItemsSource = things;
            }


            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
    }

}