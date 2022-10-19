using Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(UsernameInput);
            Loaded -= Login_Loaded;
            var dataContext = DataContext as LoginViewModel;
            dataContext.Username = "admin";
            var secureString = new System.Security.SecureString();
            var password = "changeme";
            foreach (var c in password)
            {
                secureString.AppendChar(c);
            }
            dataContext.Password = secureString;
           // dataContext.AttemptLogin.Execute(null);
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as LoginViewModel;
            if (dataContext != null)
            {
                dataContext.Password = PasswordInput.SecurePassword;
            }
        }

        private void UsernameTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.Focus(PasswordInput);
            }
        }

        private void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var dataContext = DataContext as LoginViewModel;
                if (dataContext != null)
                {
                    dataContext.AttemptLogin.Execute(null);
                }
            }
        }
    }
}
