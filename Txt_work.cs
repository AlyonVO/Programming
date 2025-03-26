using System.IO;


namespace Things_control
{
    internal interface ITxt_work
    {
        void AddThing(Thing thing);
        void DeleteThingByName(string name);

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

        // Добавление объекта
        public void AddThing(Thing thing)
        {
            things.Add(thing);
            SaveDataToFile();
        }

        // Удаление объекта по имени
        public void DeleteThingByName(string name)
        {
            var itemToRemove = things.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (itemToRemove != null)
            {
                things.Remove(itemToRemove);
                SaveDataToFile();
            }

            else
            {
                throw new ArgumentException($"Товар '{name}' не найден.");
            }
        }

        // Сохранение данных в файл
        private void SaveDataToFile()
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
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
