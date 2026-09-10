using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Celestite.I18N;

namespace Celestite.Utils
{
    public class NotificationHelper
    {
        private static INotificationManager? _notificationManager;

        public static void Init()
        {
            if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime t) return;
            var notification = Dispatcher.UIThread.Invoke(() => new WindowNotificationManager(t.MainWindow)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 10
            });
            _notificationManager = notification;
        }

        private static void EnsureNotification()
        {
            if (LaunchHelper.IsInGuiMode() && _notificationManager == null)
                Init();
        }

        public static void Info(string message)
        {
            EnsureNotification();
            if (_notificationManager != null)
                Dispatcher.UIThread.Invoke(() =>
                _notificationManager!.Show(new Notification(Localization.InfoBarDefault, message)));
            else
                Console.WriteLine(message);
        }

        private static string _lastErrorMessage = string.Empty;
        private static DateTime _lastErrorTime = DateTime.MinValue;

        public static void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var now = DateTime.UtcNow;
            if (message == _lastErrorMessage && (now - _lastErrorTime).TotalSeconds < 2) return;
            _lastErrorMessage = message;
            _lastErrorTime = now;

            WindowTrayHelper.RequestShow();
            EnsureNotification();
            if (_notificationManager != null)
                Dispatcher.UIThread.Invoke(() =>
                    _notificationManager!.Show(new Notification(Localization.InfoBarError, message, NotificationType.Error)));
            else
                Console.WriteLine(message);
        }

        private static string _lastWarnMessage = string.Empty;
        private static DateTime _lastWarnTime = DateTime.MinValue;

        public static void Warn(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var now = DateTime.UtcNow;
            if (message == _lastWarnMessage && (now - _lastWarnTime).TotalSeconds < 2) return;
            _lastWarnMessage = message;
            _lastWarnTime = now;

            WindowTrayHelper.RequestShow();
            EnsureNotification();
            if (_notificationManager != null)
                Dispatcher.UIThread.Invoke(() =>
                _notificationManager!.Show(new Notification(Localization.InfoBarDefault, message, NotificationType.Warning)));
            else
                Console.WriteLine(message);
        }

        public static void Warn(string message, Action clickAction)
        {
            WindowTrayHelper.RequestShow();
            EnsureNotification();
            if (_notificationManager != null)
                Dispatcher.UIThread.Invoke(() =>
                _notificationManager!.Show(new Notification(Localization.InfoBarDefault, message, NotificationType.Warning, onClick: clickAction)));
            else
                Console.WriteLine(message);
        }

        public static void Success(string message)
        {
            WindowTrayHelper.RequestShow();
            EnsureNotification();
            if (_notificationManager != null)
                Dispatcher.UIThread.Invoke(() =>
                _notificationManager!.Show(new Notification(Localization.InfoBarDefault, message, NotificationType.Success)));
            else
                Console.WriteLine(message);
        }
    }
}
