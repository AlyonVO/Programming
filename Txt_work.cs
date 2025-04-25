using System.IO;
using System.Windows;


namespace Things_control
{
    internal interface ITxt_work
    {
        void AddThing(Thing thing);
        void DeleteThingByName(string name);
        void Read_txt(string filePath);
        void SaveDataToFile(string filePath);

    }

    public class Txt_work : ITxt_work
    {
        private List<Thing> things;
        private string filePath;

        public Txt_work(List<Thing> things, string filePath)
        {
            this.things = things;
            this.filePath = filePath;
        }

        public void Read_txt(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
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
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddThing(Thing thing)
        {
            things.Add(thing);
            SaveDataToFile(filePath);
        }

        public void DeleteThingByName(string name)
        {
            var itemToRemove = things.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (itemToRemove != null)
            {
                things.Remove(itemToRemove);
                SaveDataToFile(filePath);
            }

            else
            {
                throw new ArgumentException($"Товар '{name}' не найден.");
            }
        }

        public void SaveDataToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var item in things)
                    {
                        if (item is Food food)
                        {
                            writer.WriteLine($"food \"{food.Name}\" {food.Quantity} {food.Date} {food.Type}");
                        }
                        else if (item is Not_food notFood)
                        {
                            writer.WriteLine($"notfood \"{notFood.Name}\" {notFood.Quantity} {notFood.Date} {notFood.Discount}");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Ошибка при записи в файл: {ex.Message}");
            }
        }
    }

}
