using Inventory.Helpers;
using Inventory.Interfaces;
using InventoryManagement.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Inventory.ViewModels
{
    class MainWindowViewModel : ChangeNotifier, IChangeViewModel
    {
        BaseViewModel _currentViewModel;
        Stack<BaseViewModel> _viewModels;

        // https://stackoverflow.com/a/4970019 -- logic for inactivity
        private readonly DispatcherTimer _activityTimer;
        private LoginViewModel initialViewModel;
        private bool isNotLogin;

        public MainWindowViewModel()
        {
            // upgrading settings: https://stackoverflow.com/a/534335
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            _viewModels = new Stack<BaseViewModel>();
            initialViewModel = new LoginViewModel(this);
            isNotLogin = false;
            _viewModels.Push(initialViewModel);
            CurrentViewModel = initialViewModel;
            // setup inactivity timer
            InputManager.Current.PreProcessInput += InputPreProcessInput;
            var autoLogoutTime = Settings.Default.AutoLogoutLength;
            if (autoLogoutTime < 1)
            {
                autoLogoutTime = 10;
            }
            _activityTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(autoLogoutTime), IsEnabled = true };
            _activityTimer.Tick += ActivityTimerTick;
        }

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; NotifyPropertyChanged(); }
        }

        private void ActivityTimerTick(object sender, EventArgs e)
        {
            // set UI to inactivity
            PopToBaseViewModel();
        }

        private void InputPreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            InputEventArgs inputEventArgs = e.StagingItem.Input;

            if (inputEventArgs is MouseEventArgs || inputEventArgs is KeyboardEventArgs)
            {
                // reset timer
                _activityTimer.Stop();
                _activityTimer.Start();
            }
        }

        public bool IsNotLogin
        {
            get => isNotLogin;
            set
            {
                isNotLogin = value;
                NotifyPropertyChanged();
            }
        }

        #region IChangeViewModel

        public void PushViewModel(BaseViewModel model)
        {
            _viewModels.Push(model);
            CurrentViewModel = model;

            IsNotLogin = !(CurrentViewModel is LoginViewModel);
        }

        public void PopViewModel()
        {
            if (_viewModels.Count > 1)
            {
                _viewModels.Pop();
            }
            CurrentViewModel = _viewModels.Peek();
        }

        public void PopToBaseViewModel()
        {
            while (_viewModels.Count > 1)
            {
                _viewModels.Pop();
                CurrentViewModel = _viewModels.Peek();
            }
        }

        public ICommand MoveToManageItemsScreen
        {
            get { return new RelayCommand(LoadManageItemsScreen); }
        }

        private void LoadManageItemsScreen()
        {
            CurrentViewModel = new ManageItemsViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageCurrenciesScreen
        {
            get { return new RelayCommand(LoadManageCurrenciesScreen); }
        }

        private void LoadManageCurrenciesScreen()
        {
            CurrentViewModel = new ViewCurrenciesViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToScanItemsScreen
        {
            get { return new RelayCommand(LoadScanItemsScreen); }
        }

        private void LoadScanItemsScreen()
        {
            CurrentViewModel = new ScanItemsViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToScanAndPurchaseItemsScreen
        {
            get { return new RelayCommand(LoadScanAndPurchaseItemsScreen); }
        }

        private void LoadScanAndPurchaseItemsScreen()
        {
            CurrentViewModel = new ScanAndPurchaseViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToGenerateBarcodesScreen
        {
            get { return new RelayCommand(LoadGenerateBarcodesScreen); }
        }

        private void LoadGenerateBarcodesScreen()
        {
            CurrentViewModel = new GenerateBarcodesViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToReportsScreen
        {
            get { return new RelayCommand(LoadReportsScreen); }
        }

        private void LoadReportsScreen()
        {
            CurrentViewModel = new ViewReportsViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageItemCategoriesScreen
        {
            get { return new RelayCommand(LoadViewItemTypesScreen); }
        }

        private void LoadViewItemTypesScreen()
        {
            CurrentViewModel = new ViewItemTypesViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
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

        public ICommand Logout
        {
            get { return new RelayCommand(PerformLogout); }
        }

        private void PerformLogout()
        {
            PushViewModel(new LoginViewModel(this));
        }

        public ICommand MoveToManageUsersScreen
        {
            get { return new RelayCommand(LoadManageUsersScreen); }
        }

        private void LoadManageUsersScreen()
        {
            CurrentViewModel = new ManageUsersViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToChangePasswordScreen
        {
            get { return new RelayCommand(LoadChangePasswordScreen); }
        }

        private void LoadChangePasswordScreen()
        {
            CurrentViewModel = new ChangePasswordViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        public ICommand MoveToManageAppSettingsScreen
        {
            get { return new RelayCommand(LoadManageAppSettingsScreen); }
        }

        private void LoadManageAppSettingsScreen()
        {
            CurrentViewModel = new ManageAppSettingsViewModel(this) { CurrentUser = initialViewModel.CurrentUser };
            PushViewModel(CurrentViewModel);
        }

        #endregion
    }
}
