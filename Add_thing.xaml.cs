using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Things_control
{
    public partial class Add_thing : Window
    {
        public Add_thing()
        {
            InitializeComponent();
            TypeComboBox.SelectedIndex = 0;
            UpdateTypeDiscountLabel();
        }

        public bool IsFood { get; private set; }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsFood = TypeComboBox.SelectedIndex == 0;
            UpdateTypeDiscountLabel();
        }

        private void UpdateTypeDiscountLabel()
        {
            if (TypeDiscountLabel == null)
            {
                System.Diagnostics.Debug.WriteLine("Предупреждение: Элемент TypeDiscountLabel не найден!");
                return; 
            }

            else
            {
                if (IsFood == true)
                {
                    TypeDiscountLabel.Text = "Тип:";
                    TypeDiscountTextBox.Text = "";
                    TypeDiscountTextBox.ToolTip = "Введите тип продукта";

                }

                else
                {
                    TypeDiscountLabel.Text = "Скидка (%):";
                    TypeDiscountTextBox.Text = "";
                    TypeDiscountTextBox.ToolTip = "Введите целое число от 0 до 100";
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка введенных данных
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || 
                string.IsNullOrWhiteSpace(TypeDiscountTextBox.Text))
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0) 
            {
                MessageBox.Show("Количество должно быть целым", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Дополнительная проверка для Not_Food
            if (!IsFood && !int.TryParse(TypeDiscountTextBox.Text, out int discount))
            {
                MessageBox.Show("Скидка должна быть числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Свойства для доступа к данным
        public string ItemName => NameTextBox.Text;
        public int Quantity => int.Parse(QuantityTextBox.Text);
        public string Date => DateTextBox.Text;
        public object TypeOrDiscount
        {
            get
            {
                if (IsFood)
                {
                    // Для Food возвращаем строку (тип продукта)
                    return TypeDiscountTextBox.Text.Trim();
                }
                else
                {
                    // Для Not_food возвращаем число (скидку)
                    if (int.TryParse(TypeDiscountTextBox.Text, out int discount))
                    {
                        return discount;
                    }
                    return 0; // Значение по умолчанию при ошибке парсинга
                }
            }
        }

        private void DateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            // Разрешаем ввод только цифр
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }

            // Находим следующую позицию для ввода цифры
            int pos = GetNextDigitPosition(textBox.Text, textBox.CaretIndex);
            if (pos == -1)
            {
                e.Handled = true;
                return;
            }

            // Вставляем цифру вместо подчеркивания
            var chars = textBox.Text.ToCharArray();
            chars[pos] = e.Text[0];
            textBox.Text = new string(chars);
            textBox.CaretIndex = pos + 1;

            e.Handled = true;
        }

        private void DateTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;

            // Backspace и Delete
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                int pos = textBox.CaretIndex;
                if (e.Key == Key.Back && pos > 0) pos--;
                if (e.Key == Key.Delete && pos >= textBox.Text.Length) return;

                // Замена цифры на подчеркивание, если это не точка
                if (pos >= 0 && pos < textBox.Text.Length && textBox.Text[pos] != '.')
                {
                    var chars = textBox.Text.ToCharArray();
                    chars[pos] = '_';
                    textBox.Text = new string(chars);
                    textBox.CaretIndex = pos;
                }

                e.Handled = true;
            }
        }

        private void DateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            // Восстанавливаем маску, если она была повреждена
            if (textBox.Text.Length != 10 ||
                textBox.Text[4] != '.' ||
                textBox.Text[7] != '.')
            {
                textBox.Text = "____.__.__";
            }
        }

        private int GetNextDigitPosition(string text, int currentPos)
        {
            for (int i = currentPos; i < text.Length; i++)
            {
                if (text[i] == '_') return i;
                if (char.IsDigit(text[i])) continue;
                if (text[i] == '.' && i + 1 < text.Length && text[i + 1] == '_')
                    return i + 1;
            }
            return -1;
        }
    }

}