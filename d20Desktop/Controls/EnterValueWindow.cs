using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fiction.GameScreen.Controls
{
    static class EnterValueWindow
    {
        /// <summary>
        /// Gets a value from a user
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <param name="initial">Initial value to seed with</param>
        /// <param name="options">List of strings to choose from</param>
        /// <param name="canAddNew">Whether or not the user can enter a value manually</param>
        /// <returns>Value entered, or empty string if no value entered</returns>
        public static string GetValue(Window owner, string initial, IEnumerable<string> options, bool canAddNew)
        {
            EnterValueViewModel value = new EnterValueViewModel(initial, options, canAddNew);
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(owner);
            window.DataContext = value;
            window.SizeToContent = SizeToContent.Height;

            if (window.ShowDialog() == true)
                return value.Value;
            return string.Empty;
        }
        /// <summary>
        /// Gets a value from a user
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <param name="initial">Initial value to seed with</param>
        /// <returns>Value entered, or empty string if no value entered</returns>
        public static string GetValue(Window owner, string initial)
        {
            EnterValueViewModel vm = new EnterValueViewModel();
            EditWindow window = new EditWindow();
            window.SizeToContent = SizeToContent.Height;
            window.Owner = Window.GetWindow(owner);
            window.DataContext = vm;

            if (window.ShowDialog() == true)
                return vm.Value;
            return string.Empty;
        }
        /// <summary>
        /// Gets a value from a user
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <returns>Value entered, or empty string if no value entered</returns>
        public static string GetValue(Window owner)
        {
            return GetValue(owner, string.Empty);
        }
    }
}
