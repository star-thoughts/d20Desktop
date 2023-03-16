using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    public sealed class SelectCampaignObjects : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SelectCampaignObjects"/> class
        /// </summary>
        static SelectCampaignObjects()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectCampaignObjects), new FrameworkPropertyMetadata(typeof(SelectCampaignObjects)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the items to choose from
        /// </summary>
        public IEnumerable<IFilterable> Items
        {
            get { return (IEnumerable<IFilterable>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected items
        /// </summary>
        public ObservableCollection<IFilterable> SelectedItems
        {
            get { return (ObservableCollection<IFilterable>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not to allow selection of multiple items
        /// </summary>
        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Items"/>
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IEnumerable<IFilterable>), typeof(SelectCampaignObjects));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedItems"/>
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(ObservableCollection<IFilterable>), typeof(SelectCampaignObjects));
        /// <summary>
        /// DependencyProperty for <see cref="MultiSelect"/>
        /// </summary>
        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(nameof(MultiSelect), typeof(bool), typeof(SelectCampaignObjects));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
        }
        #endregion
    }
}
