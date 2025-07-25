using System.Windows;

namespace CommonLogic.WPF
{
    public partial class InputDialog : Window
    {
        public string ResultText { get; private set; }

        // Новий конструктор, який приймає початкове значення
        public InputDialog(string initialValue = "")
        {
            InitializeComponent();
            KeypadControl.SetInitialText(initialValue);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.ResultText = KeypadControl.EnteredText;
            this.DialogResult = true; // Закриває вікно і повертає true
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закриває вікно і повертає false
        }
    }
}