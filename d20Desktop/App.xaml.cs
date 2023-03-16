using Fiction.GameScreen;
using Fiction.GameScreen.Characters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using Fiction.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors
        public App()
        {
            AppSettings = new AppSettings();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the application's settings
        /// </summary>
        public static AppSettings AppSettings { get; private set; }
        #endregion
        #region Methods
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CommonWindows.Initialize();
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
        #endregion
    }
}
