using System.Windows;
using System.Windows.Controls;

namespace CommonLogic.WPF
{
    public partial class NumericKeypadWindow : Window
    {
        public string EnteredValue { get; private set; }

        public NumericKeypadWindow(string initialValue = "")
        {
            InitializeComponent();
            ValueTextBox.Text = initialValue;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ValueTextBox.Text += button.Content.ToString();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueTextBox.Text.Length > 0)
            {
                ValueTextBox.Text = ValueTextBox.Text.Substring(0, ValueTextBox.Text.Length - 1);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredValue = ValueTextBox.Text;
            DialogResult = true; // Сигналізує, що користувач натиснув "ОК"
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Сигналізує, що користувач натиснув "Скасувати"
            Close();
        }
    }
}