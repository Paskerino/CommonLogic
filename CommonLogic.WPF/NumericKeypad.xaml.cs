using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonLogic.WPF
{
    public partial class NumericKeypad : UserControl
    {
        // Події для зв'язку з вікном-контейнером
        public event EventHandler<string> EnterClicked;
        public event EventHandler CancelClicked;

        // Властивість для доступу до введеного тексту
        public string EnteredText => DisplayTextBox.Text;

        public NumericKeypad()
        {
            InitializeComponent();
            // Підписуємося на події кнопок Enter та Cancel
            EnterButton.Click += (s, e) => EnterClicked?.Invoke(this, DisplayTextBox.Text);
            CancelButton.Click += (s, e) => CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        // Метод для встановлення початкового значення
        public void SetInitialText(string text)
        {
            DisplayTextBox.Text = text;
            DisplayTextBox.Focus();
            DisplayTextBox.CaretIndex = text.Length;
        }

        // Обробник для всіх цифрових кнопок та коми
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string content = button.Content.ToString();
                if ((content == "," || content == ".") && (DisplayTextBox.Text.Contains(",") || DisplayTextBox.Text.Contains("."))) return;

                DisplayTextBox.Text += content;
            }
        }

        // Обробник для кнопки CE (Clear Entry)
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = string.Empty;
        }

        // Обробник для кнопки "назад" (Backspace)
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayTextBox.Text.Length > 0)
            {
                DisplayTextBox.Text = DisplayTextBox.Text.Substring(0, DisplayTextBox.Text.Length - 1);
            }
        }
    }
}