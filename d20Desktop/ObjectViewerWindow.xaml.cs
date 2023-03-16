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
using System.Windows.Shapes;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Window that allows the user to view an object's information
    /// </summary>
    public partial class ObjectViewerWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="OverflowMode"/>
        /// </summary>
        public ObjectViewerWindow(object item)
        {
            DataContext = item;
            InitializeComponent();
        }
        #endregion
        #region Methods
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        #endregion
    }
}
