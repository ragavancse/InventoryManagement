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

namespace CustomControls
{
    /// <summary>
    /// Interaction logic for ToolbarMenu.xaml
    /// </summary>
    public partial class ToolbarMenu : UserControl
    {
        Window CurrentWindow;
        public ToolbarMenu()
        {
           
            InitializeComponent();
            this.Loaded += ToolbarMenu_Loaded;
        }

        private void ToolbarMenu_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentWindow = Window.GetWindow(this);
            CurrentWindow.StateChanged += MainWindowStateChangeRaised;
            this.Loaded -= ToolbarMenu_Loaded;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(CurrentWindow);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(CurrentWindow);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(CurrentWindow);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(CurrentWindow);
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (CurrentWindow.WindowState == WindowState.Maximized)
            {
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}

