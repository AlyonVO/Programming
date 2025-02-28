using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    public class Food : Thing
    {
        protected string Type { get; set; }

        public override void Read(string line)
        {
            var parts = line.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
            Name = parts[1].Trim('"');
            Quantity = int.Parse(parts[2]);
            Date = parts[3];
            Type = parts[4];
        }

        public override void Write()
        {
            Console.WriteLine($"Это еда: Название = {Name}, Количество = {Quantity}, Дата = {Date}, Тип = {Type}");
        }
    }
}
