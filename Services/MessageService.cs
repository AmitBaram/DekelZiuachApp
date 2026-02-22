using System.Windows;

namespace DekelApp.Services
{
    public interface IMessageService
    {
        void ShowMessage(string message, string caption = "Information");
        void ShowWarning(string message, string caption = "Warning");
        void ShowError(string message, string caption = "Error");
        bool ShowQuestion(string message, string caption = "Question");
    }

    public class MessageService : IMessageService
    {
        public void ShowMessage(string message, string caption = "Information")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarning(string message, string caption = "Warning")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowError(string message, string caption = "Error")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool ShowQuestion(string message, string caption = "Question")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
