using Celestite.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Celestite.ViewModels.Dialogs
{
    public partial class DefaultLoginFormDialogViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty] private bool _saveEmail = true;
        [ObservableProperty] private bool _savePassword = true;
        [ObservableProperty] private bool _autoLogin = true;

        [ObservableProperty] private string[] _autoCompleteItems = ConfigUtils.GetAllSavedEmails();

        [ObservableProperty] private bool _lockSaveEmail;
        [ObservableProperty] private bool _lockSavePassword;

        partial void OnEmailChanged(string value)
        {
            if (!ConfigUtils.TryGetGuidByAccountEmail(value, out var guid) ||
                !ConfigUtils.TryGetAccountObjectByGuid(guid, out var accountObject)) return;
            if (!accountObject.SaveEmail) return;
            Email = accountObject.Email;
            if (accountObject.SavePassword)
                Password = accountObject.Password;
            SaveEmail = accountObject.SaveEmail;
            SavePassword = accountObject.SavePassword;
            AutoLogin = accountObject.AutoLogin;
        }

        public void Reset(bool lockSave = false)
        {
            LockSaveEmail = lockSave;
            LockSavePassword = lockSave;
            SaveEmail = true;
            SavePassword = true;
            AutoLogin = true;
            if (lockSave)
            {
                Email = string.Empty;
                Password = string.Empty;
                AutoCompleteItems = [];
            }
            else
            {
                AutoCompleteItems = ConfigUtils.GetAllSavedEmails();
                if (ConfigUtils.TryGetLastLogin(out var lastLogin) && lastLogin != null && lastLogin.SaveEmail && !string.IsNullOrEmpty(lastLogin.Email))
                {
                    Email = lastLogin.Email;
                    if (lastLogin.SavePassword && !string.IsNullOrEmpty(lastLogin.Password))
                    {
                        Password = lastLogin.Password;
                    }
                    else
                    {
                        Password = string.Empty;
                    }
                }
                else
                {
                    Email = string.Empty;
                    Password = string.Empty;
                }
            }
        }
    }
}
