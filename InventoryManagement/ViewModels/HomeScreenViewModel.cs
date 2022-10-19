using Microsoft.Win32;
using Inventory.Helpers;
using Inventory.Interfaces;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory.Properties;
using InventoryManagement.Properties;

namespace Inventory.ViewModels
{
    class HomeScreenViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public HomeScreenViewModel(IChangeViewModel viewModelChanger) : base(viewModelChanger)
        {
        }

        public ICommand MoveToManageItemsScreen
        {
            get { return new RelayCommand(LoadManageItemsScreen); }
        }

        private void LoadManageItemsScreen()
        {
            CurrentViewModel = new ManageItemsViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageCurrenciesScreen
        {
            get { return new RelayCommand(LoadManageCurrenciesScreen); }
        }

        private void LoadManageCurrenciesScreen()
        {
            CurrentViewModel = new ViewCurrenciesViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToScanItemsScreen
        {
            get { return new RelayCommand(LoadScanItemsScreen); }
        }

        private void LoadScanItemsScreen()
        {
            CurrentViewModel = new ScanItemsViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToScanAndPurchaseItemsScreen
        {
            get { return new RelayCommand(LoadScanAndPurchaseItemsScreen); }
        }

        private void LoadScanAndPurchaseItemsScreen()
        {
            CurrentViewModel = new ScanAndPurchaseViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToGenerateBarcodesScreen
        {
            get { return new RelayCommand(LoadGenerateBarcodesScreen); }
        }

        private void LoadGenerateBarcodesScreen()
        {
            CurrentViewModel = new GenerateBarcodesViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToReportsScreen
        {
            get { return new RelayCommand(LoadReportsScreen); }
        }

        private void LoadReportsScreen()
        {
            CurrentViewModel = new ViewReportsViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageItemCategoriesScreen
        {
            get { return new RelayCommand(LoadViewItemTypesScreen); }
        }

        private void LoadViewItemTypesScreen()
        {
            CurrentViewModel = new ViewItemTypesViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand BackupData
        {
            get { return new RelayCommand(BackupDatabase); }
        }

        private void BackupDatabase()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Sqlite file (*.db)|*.db";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.FileName = "inventory-backup-" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss");

            var lastBackupLocation = Settings.Default.LastBackupFolder;
            if (!string.IsNullOrWhiteSpace(lastBackupLocation) && Directory.Exists(Path.GetDirectoryName(lastBackupLocation)))
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.InitialDirectory = lastBackupLocation;
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                var dbHelper = new DatabaseHelper();
                File.Copy(dbHelper.GetDatabaseFilePath(), saveFileDialog.FileName);
                Settings.Default.LastBackupFolder = Path.GetDirectoryName(saveFileDialog.FileName);
            }
        }

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; NotifyPropertyChanged(); }
        }


        public ICommand Logout
        {
            get { return new RelayCommand(PerformLogout); }
        }
        
        private void PerformLogout()
        {
            PopViewModel();
        }

        public ICommand MoveToManageUsersScreen
        {
            get { return new RelayCommand(LoadManageUsersScreen); }
        }

        private void LoadManageUsersScreen()
        {
            CurrentViewModel = new ManageUsersViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToChangePasswordScreen
        {
            get { return new RelayCommand(LoadChangePasswordScreen); }
        }

        private void LoadChangePasswordScreen()
        {
            CurrentViewModel = new ChangePasswordViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageAppSettingsScreen
        {
            get { return new RelayCommand(LoadManageAppSettingsScreen); }
        }

        private void LoadManageAppSettingsScreen()
        {
            CurrentViewModel = new ManageAppSettingsViewModel(ViewModelChanger) { CurrentUser = CurrentUser };
             // PushViewModel(CurrentViewModel);
        }
    }
}
