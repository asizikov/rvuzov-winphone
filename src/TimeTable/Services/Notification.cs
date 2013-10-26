using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class NotificationService : INotificationService
    {
        public void ShowToast(string title, string message)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                var toast = new ToastPrompt
                {
                    Title = title,
                    Message = message,
                    TextWrapping = TextWrapping.Wrap,
                    Background = new SolidColorBrush(Color.FromArgb(255, 76, 109, 167)),
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    ImageSource = new BitmapImage(new Uri("/Resources/Images/notification_logo.png", UriKind.Relative))
                };

                toast.Show();
            });
            
        }
    }
}