using Fiction.GameScreen.ViewModels;
using Fiction.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for showing a list of items that support text filtering
    /// </summary>
    public sealed class FilterableList : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="FilterableList"/>
        /// </summary>
        static FilterableList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterableList), new FrameworkPropertyMetadata(typeof(FilterableList)));
        }
        #endregion
        #region Member Variables
        private ListCollectionView? _source;
        private TextBox? _filterTextBox;
        private ListBox? _itemsList;
        private bool _updatingSelection;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the items to display
        /// </summary>
        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the filter text
        /// </summary>
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value);}
        }
        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public IFilterable SelectedItem
        {
            get { return (IFilterable)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value);}
        }
        /// <summary>
        /// Gets or sets a collection of selected items
        /// </summary>
        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not the user can pick multiple items
        /// </summary>
        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value);}
        }
        /// <summary>
        /// Gets or sets the filter to use
        /// </summary>
        public IFilterViewModel Filter
        {
            get { return (IFilterViewModel)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Items"/>
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IEnumerable), typeof(FilterableList),
            new FrameworkPropertyMetadata(null, ItemsChanged));
        /// <summary>
        /// DependencyProperty for <see cref="FilterText"/>
        /// </summary>
        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(nameof(FilterText), typeof(string), typeof(FilterableList),
            new FrameworkPropertyMetadata(null, FilterTextChanged));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedItem"/>
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(IFilterable), typeof(FilterableList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedItems"/>
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(IEnumerable), typeof(FilterableList),
            new FrameworkPropertyMetadata(null, SelectedItemsChanged));
        /// <summary>
        /// DependencyProperty for <see cref="MultiSelect"/>
        /// </summary>
        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(nameof(MultiSelect), typeof(bool), typeof(FilterableList));
        /// <summary>
        /// DependencyProperty for <see cref="Filter"/>
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(IFilterViewModel), typeof(FilterableList));

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is FilterableList view)
                {
                    if (e.OldValue is ListCollectionView oldSource)
                    {
                        view._source = null;
                        oldSource.Filter = null;
                    }
                    if (e.NewValue is ListCollectionView newSource)
                    {
                        view._source = newSource;
                        newSource.Filter = view.Items_Filter;
                    }
                }
            });
        }

        private static void FilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is FilterableList view)
                    view?.RefreshList();
            });
        }

        private static void SelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is FilterableList view)
                    view?.UpdateItemSelection((ObservableCollection<ICampaignObject>)e.NewValue);
            });
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            _filterTextBox = Template.FindName("PART_FilterText", this) as TextBox;
            Panel? root = Template.FindName("PART_RootGrid", this) as Panel;
            if (root != null)
                root.PreviewKeyDown += _filter_PreviewKeyDown;

            _itemsList = Template.FindName("PART_List", this) as ListBox;
            if (_itemsList != null)
            {
                _itemsList.SelectionChanged += ItemList_SelectionChanged;
                _itemsList.PreviewTextInput += _itemsList_PreviewTextInput;

                Style containerStyle = _itemsList.GetOrCreateItemContainerStyle<ListBoxItem>();
                containerStyle.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(_itemsList_DoubleClick)));
            }

            Button? filterButton = Template.FindName("PART_FilterButton", this) as Button;
            if (filterButton != null)
                filterButton.Click += FilterButton_Click;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (Filter != null && _source != null)
                {
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = Filter;

                    if (window.ShowDialog() != true)
                        Filter.Reset();

                    _source.Refresh();
                }
            });
        }

        private void _itemsList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is ListBoxItem item && item.DataContext is IFilterable filterable)
                    RaiseEvent(new ItemDoubleClickedEventArgs(filterable, this));
            });
        }

        private void _filter_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (!e.IsRepeat && e.Key == Key.Escape)
                {
                    FilterText = string.Empty;
                    Filter?.Reset();
                    _itemsList?.Focus();
                }
            });
        }

        private async void _itemsList_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                if (!string.IsNullOrWhiteSpace(e.Text))
                {
                    FilterText = e.Text;
                    if (_filterTextBox != null)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            _filterTextBox.Focus();
                            _filterTextBox.CaretIndex = FilterText.Length;
                        });
                    }
                }
            });
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (SelectedItems is IList list)
                {
                    if (e.AddedItems != null)
                    {
                        foreach (IFilterable filterable in e.AddedItems)
                            list.Add(filterable);
                    }
                    if (e.RemovedItems != null)
                    {
                        foreach (IFilterable filterable in e.RemovedItems)
                            list.Remove(filterable);
                    }
                }
            });
        }

        private void RefreshList()
        {
            _source?.Refresh();
        }


        private async Task UpdateItemSelection(ObservableCollection<ICampaignObject> items)
        {
            if (items != null)
            {
                if (!IsLoaded)
                    await Dispatcher.InvokeAsync(() => InnerUpdateItemSelection(items), System.Windows.Threading.DispatcherPriority.Render);
                else
                    InnerUpdateItemSelection(items);
            }
        }

        private void InnerUpdateItemSelection(ObservableCollection<ICampaignObject> items)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (_itemsList != null && !_updatingSelection)
                {
                    _updatingSelection = true;
                    try
                    {
                        if (MultiSelect)
                        {
                            _itemsList.SelectedItems.Clear();
                            if (items != null)
                            {
                                foreach (ICampaignObject combatant in items)
                                    _itemsList.SelectedItems.Add(combatant);
                            }
                        }
                        else
                            _itemsList.SelectedValue = items.FirstOrDefault();
                    }
                    finally
                    {
                        _updatingSelection = false;
                    }
                }
            });
        }

        private bool Items_Filter(object item)
        {
            return Exceptions.FailSafeMethodCall(() =>
            {
                if (Filter != null && Filter.HasFilter)
                    return Filter.Matches(item);
                else if (item is IFilterable filterable && !string.IsNullOrEmpty(FilterText))
                    return filterable.CanDisplay(FilterText);

                return true;
            });
        }
        #endregion
        #region Events
        /// <summary>
        /// RoutedEvent for <see cref="ItemDoubleClicked"/>
        /// </summary>
        public static RoutedEvent ItemDoubleClickedEvent = EventManager.RegisterRoutedEvent(nameof(ItemDoubleClicked), RoutingStrategy.Bubble, typeof(ItemDoubleClickedEventHandler), typeof(FilterableList));
        /// <summary>
        /// Triggered when the user double clicks an item in the list
        /// </summary>
        public event ItemDoubleClickedEventHandler ItemDoubleClicked
        {
            add { AddHandler(ItemDoubleClickedEvent, value); }
            remove { RemoveHandler(ItemDoubleClickedEvent, value); }
        }
        #endregion
    }
}
