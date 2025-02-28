using System;
using System.Text;
using System.IO;


class Program
{
    static void Main(string[] args)
    {
        List<First.Food> foods = new List<First.Food>();
        List<First.Not_food> not_Foods = new List<First.Not_food>();

        string path = "C://Users/alyon/source/repos/first/things.txt";
        //var srcEncoding = Encoding.GetEncoding(1251);


        //using (StreamReader reader = new StreamReader(path, encoding: srcEncoding))
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("food"))        // Удобная функция взять первое слово строки
                {
                    First.Food food = new First.Food();
                    food.Read(line);
                    food.Write();
                    foods.Add(food);

                }
                else if (!line.StartsWith("food"))
                {
                    First.Not_food notFood = new First.Not_food();
                    notFood.Read(line);
                    notFood.Write();
                    not_Foods.Add(notFood);
                }
            }
        }

    }
}