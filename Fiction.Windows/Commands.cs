using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fiction.Windows
{
	/// <summary>
	/// Commands for controls in the Fiction.Windows namespace
	/// </summary>
	public static class Commands
	{
		/// <summary>
		/// Add command
		/// </summary>
		public static readonly RoutedCommand Add = new RoutedCommand("Add", typeof(Commands));
		/// <summary>
		/// Remove command
		/// </summary>
		public static readonly RoutedCommand Remove = new RoutedCommand("Remove", typeof(Commands));
		/// <summary>
		/// Edit command
		/// </summary>
		public static readonly RoutedCommand Edit = new RoutedCommand("Edit", typeof(Commands));
		/// <summary>
		/// Ok command
		/// </summary>
		public static readonly RoutedCommand Ok = new RoutedCommand("Ok", typeof(Commands));
		/// <summary>
		/// Cancel command
		/// </summary>
		public static readonly RoutedCommand Cancel = new RoutedCommand("Cancel", typeof(Commands));
        /// <summary>
        /// Close command
        /// </summary>
        public static readonly RoutedCommand Close = new RoutedCommand("Close", typeof(Commands));
        /// <summary>
        /// Command to fit a view vertically and horizontally
        /// </summary>
        public static readonly RoutedCommand FitToPage = new RoutedCommand(nameof(FitToPage), typeof(Commands));
        /// <summary>
        /// Command to fit a view horizontally
        /// </summary>
        public static readonly RoutedCommand FitToWidth = new RoutedCommand(nameof(FitToWidth), typeof(Commands));
        /// <summary>
        /// Command to fit a view vertically
        /// </summary>
        public static readonly RoutedCommand FitToHeight = new RoutedCommand(nameof(FitToHeight), typeof(Commands));
	}
}
