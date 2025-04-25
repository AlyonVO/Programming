using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Things_control
{
    internal interface ICommandProcessor
    {
        void ProcessCommands(string filePath, List<Thing> things, ITxt_work txtWork);
    }

    internal class CommandProcessor : ICommandProcessor
    {
        public void ProcessCommands(string filePath, List<Thing> things, ITxt_work txtWork)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл команд не найден: {filePath}");

            var commands = File.ReadAllLines(filePath);

            foreach (var commandLine in commands.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                ProcessCommand(commandLine, things, txtWork);
            }
        }

        private void ProcessCommand(string commandLine, List<Thing> things, ITxt_work txtWork)
        {
            var parts = commandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            switch (parts[0].ToUpper())
            {
                case "ADD":
                    Add_cmd(parts, things);
                    break;
                case "REM":
                    Remove_cmd(parts, things);
                    break;
                case "SAVE":
                    Save_cmd(parts, txtWork);
                    break;
            }
        }

        private void Add_cmd(string[] parts, List<Thing> things)
        {
            if (parts.Length < 5) return;

            try
            {
                var type = parts[1];
                var name = parts[2].Trim('"');
                var quantity = int.Parse(parts[3]);
                var date = parts[4];

                Thing newItem = type.ToLower() == "food"
                    ? new Food(name, quantity, date, parts.Length > 5 ? parts[5] : "Без комментария")
                    : new Not_food(name, quantity, date, parts.Length > 5 ? int.Parse(parts[5]) : 0);

                things.Add(newItem);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Перепроверьте запись ADD: {ex.Message}");
            }
        }

        private void Remove_cmd(string[] parts, List<Thing> things)
        {
            if (parts.Length < 2) return;

            try
            {
                var conditionType = parts[1].ToLower();
                var conditionValue = parts.Length > 2 ? parts[2] : string.Empty;

                Predicate<Thing> predicate = conditionType switch
                {
                    "name" => t => t.Name.Equals(conditionValue.Trim('"'), StringComparison.OrdinalIgnoreCase),
                    "quantity" => t => t.Quantity == int.Parse(conditionValue),
                    "date" => t => t.Date == conditionValue,
                    _ => null
                };

                if (predicate != null)
                {
                    things.RemoveAll(predicate);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обработки команды REM: {ex.Message}");
            }
        }

        private void Save_cmd(string[] parts, ITxt_work txtWork)
        {
            if (parts.Length < 2) return;

            try
            {
                var savePath = string.Join(" ", parts.Skip(1)).Trim('"');
                var directory = Path.GetDirectoryName(savePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                txtWork.SaveDataToFile(savePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обработки команды SAVE: {ex.Message}");
                throw;
            }
        }
    }
}