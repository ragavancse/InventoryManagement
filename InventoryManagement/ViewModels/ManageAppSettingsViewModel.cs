using Inventory.Helpers;
using Inventory.Interfaces;
using Inventory.Models;
using InventoryManagement.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    class ManageAppSettingsViewModel : BaseViewModel
    {
        private uint _autoLogoutLengthMinutes;

        public ManageAppSettingsViewModel(IChangeViewModel viewModelChanger) : base(viewModelChanger)
        {
            _autoLogoutLengthMinutes =Settings.Default.AutoLogoutLength;
        }

        #region Properties

        public uint AutoLogoutLengthMinutes
        {
            get { return _autoLogoutLengthMinutes; }
            set { _autoLogoutLengthMinutes = value; NotifyPropertyChanged(); }
        }

        #endregion

        public ICommand ReturnToMainMenu
        {
            get { return new RelayCommand(PopToMainMenu); }
        }

        private void PopToMainMenu()
        {
            PopViewModel();
        }

        public ICommand SaveAppSettings
        {
            get { return new RelayCommand(SaveAndPop); }
        }

        private void SaveAndPop()
        {
            // save
            Settings.Default.AutoLogoutLength = AutoLogoutLengthMinutes;
            // pop
            PopToMainMenu();
        }
    }
}
