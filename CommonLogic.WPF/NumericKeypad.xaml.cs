using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonLogic.WPF
{
    public partial class NumericKeypad : UserControl
    {
        public string EnteredText => DisplayTextBox.Text;
        public event Action<string> OnOk;
        public event Action OnCancel;
        public NumericKeypad()
        {
            InitializeComponent();
            OkButton.Click += (s, e) => OnOk?.Invoke(DisplayTextBox.Text);
            CancelButton.Click += (s, e) => OnCancel?.Invoke();
           
        }
        public void SetInitialText(string text)
        {
            DisplayTextBox.Text = text;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string content = button.Content.ToString();
                // Забороняємо вводити більше однієї коми/крапки
                if ((content == "," || content == ".") && (DisplayTextBox.Text.Contains(",") || DisplayTextBox.Text.Contains(".")))
                {
                    return;
                }

                // Дозволяємо мінус тільки на початку
                //if (content == "-" && DisplayTextBox.Text.Length > 0)
                //{
                //    return;
                //}

                DisplayTextBox.Text += content;
            }
        }
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayTextBox.Text.Length > 0)
            {
                DisplayTextBox.Text = DisplayTextBox.Text.Substring(0, DisplayTextBox.Text.Length - 1);
            }
        }
        // Метод для кнопки CE (Clear Entry)
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = string.Empty;
        }
    }
}