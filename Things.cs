using System.Globalization;

namespace Things_control
{

    public abstract class Thing
    {
        private string _name;
        private int _quantity;
        private string _date;

        protected Thing(string name, int quantity, string date)
        {
            _name = name;
            _quantity = quantity;
            _date = date;
        }

        public string Name
        {
            get => _name;
            protected set => _name = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Имя не может быть пустым");
        }

        public int Quantity
        {
            get => _quantity;
            protected set => _quantity = value > 0
                ? value
                : throw new ArgumentOutOfRangeException("Количество должно быть положительным");
        }

        public string Date
        {
            get => _date;
            protected set => _date = IsValidDate(value)
                ? value
                : throw new ArgumentException("Неверный формат даты");
        }

        protected virtual bool IsValidDate(string date)
        {
            return DateTime.TryParseExact(date, "yyyy.MM.dd",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out _);
        }

        public static Thing Read(string line)
        {
            if (line.StartsWith("food "))
                return Food.Read(line);
            if (line.StartsWith("notfood "))
                return Not_food.Read(line);

            throw new FormatException("Неизвестный тип товара");
        }
    }

    public class Food : Thing
    {
        private string _type;

        public Food(string name, int quantity, string date, string type)
            : base(name, quantity, date)
        {
            Type = type;
        }

        public string Type
        {
            get => _type;
            private set => _type = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Тип не может быть пустым");
        }

        public static new Food Read(string line)
        {
            var parts = line.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
            return new Food(
                name: parts[1].Trim('"'),
                quantity: int.Parse(parts[2]),
                date: parts[3],
                type: parts[4]
            );
        }
    }


    public class Not_food : Thing
    {
        private int _discount;

        public Not_food(string name, int quantity, string date, int discount)
            : base(name, quantity, date)
        {
            Discount = discount;
        }

        public int Discount
        {
            get => _discount;
            private set => _discount = value >= 0 && value <= 100
                ? value
                : throw new ArgumentOutOfRangeException("Скидка должна быть от 0 до 100%");
        }

        public static new Not_food Read(string line)
        {
            var parts = line.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
            return new Not_food(
                name : parts[1].Trim('"'), // Так убираются кавычки вокруг названия
                quantity : int.Parse(parts[2]),
                date : parts[3],
                discount : int.Parse(parts[4])
            );
            
        }
    }
}
