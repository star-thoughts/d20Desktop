using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.Windows
{
	/// <summary>
	/// ListView that shows is horizontal, comma separated items with the ability to add/remove items
	/// </summary>
	public class HorizontalListView : Control
	{
		#region Constructors
		static HorizontalListView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalListView), new FrameworkPropertyMetadata(typeof(HorizontalListView)));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the items to display
		/// </summary>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		/// <summary>
		/// Gets or sets the data template to use for each item
		/// </summary>
		public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether or not to show the Remove (-) next to each item
        /// </summary>
		public bool ShowRemove
        {
            get { return (bool)GetValue(ShowRemoveProperty); }
            set { SetValue(ShowRemoveProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether or not to show the Add (+) at the end of the list
        /// </summary>
        public bool ShowAdd
        {
            get { return (bool)GetValue(ShowAddProperty); }
            set { SetValue(ShowAddProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter for the Add command
        /// </summary>
        public object AddCommandParameter
        {
            get { return GetValue(AddCommandParameterProperty); }
            set { SetValue(AddCommandParameterProperty, value); }
        }

		#endregion
		#region Dependency Properties
		/// <summary>
		/// DependencyProperty for ItemsSource
		/// </summary>
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(HorizontalListView));
		/// <summary>
		/// DependencyProperty for ItemTemplate
		/// </summary>
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(HorizontalListView));
		/// <summary>
		/// DependencyProperty for ShowRemove
		/// </summary>
		public static readonly DependencyProperty ShowRemoveProperty = DependencyProperty.Register(nameof(ShowRemove), typeof(bool), typeof(HorizontalListView));
        /// <summary>
        /// DependencyProperty for ShowAdd
        /// </summary>
        public static readonly DependencyProperty ShowAddProperty = DependencyProperty.Register(nameof(ShowAdd), typeof(bool), typeof(HorizontalListView));
        /// <summary>
        /// DependencyProperty for AddCommandParameter
        /// </summary>
        public static readonly DependencyProperty AddCommandParameterProperty = DependencyProperty.Register(nameof(AddCommandParameter), typeof(object), typeof(HorizontalListView));
        #endregion
    }
}
