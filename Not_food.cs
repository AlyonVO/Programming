using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    public class Not_food : Thing
    {
        protected int Discount { get; set; }

        public override void Read(string line)
        {
            var parts = line.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
            Name = parts[1].Trim('"'); // Так убираются кавычки вокруг названия
            Quantity = int.Parse(parts[2]);
            Date = parts[3];
            Discount = int.Parse(parts[4]);
        }

        public override void Write()
        {
            Console.WriteLine($"Это не еда: Название = {Name}, Количество = {Quantity}, Дата = {Date}, Скидка = {Discount}%");
        }
    }
}
