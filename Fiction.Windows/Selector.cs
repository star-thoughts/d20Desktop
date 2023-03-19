using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Fiction.Windows
{
    /// <summary>
    /// Helper methods and properties for Selector controls
    /// </summary>
    public static class Selector
    {
        #region Constructors
        static Selector()
        {
            SortPropertyNameProperty = DependencyProperty.RegisterAttached("SortPropertyName", typeof(string), typeof(Selector));
            IsSortableProperty = DependencyProperty.RegisterAttached("IsSortable", typeof(bool), typeof(Selector),
                new FrameworkPropertyMetadata(false, IsSortableChanged));
            IsGroupableProperty = DependencyProperty.RegisterAttached("IsGroupable", typeof(bool), typeof(Selector),
                new FrameworkPropertyMetadata(false, IsGroupableChanged));
            DefaultSortPropertyProperty = DependencyProperty.RegisterAttached("DefaultSortProperty", typeof(string), typeof(Selector),
                new FrameworkPropertyMetadata("", DefaultSortPropertyChanged));
            DefaultGroupPropertyProperty = DependencyProperty.RegisterAttached("DefaultGroupProperty", typeof(string), typeof(Selector),
                new FrameworkPropertyMetadata("", DefaultGroupPropertyChanged));
            CurrentSortPropertyProperty = DependencyProperty.RegisterAttached("CurrentSortProperty", typeof(string), typeof(Selector),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CurrentSortPropertyChanged));
        }
        #endregion
        #region ScrollIntoView
        /// <summary>
        /// DependencyProperty for ScrollOnSelection
        /// </summary>
        public static readonly DependencyProperty ScrollOnSelectionProperty =
            DependencyProperty.RegisterAttached("ScrollOnSelection", typeof(bool), typeof(Selector),
                new FrameworkPropertyMetadata(ScrollOnSelectionChanged));
        public static readonly DependencyProperty CenterOnSelectionProperty =
            DependencyProperty.RegisterAttached("CenterOnSelection", typeof(bool), typeof(Selector),
            new FrameworkPropertyMetadata(CenterOnSelectionChanged));

        private static void CenterOnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = d as System.Windows.Controls.Primitives.Selector;

            if (selector != null)
            {
                DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Primitives.Selector.ItemsSourceProperty, typeof(System.Windows.Controls.Primitives.Selector));

                if ((bool)e.NewValue)
                {
                    SetScrollOnSelection(selector, true);
                    descriptor.AddValueChanged(selector, CenterOnSelection_ItemsSourceChanged);
                }
                else
                {
                    descriptor.RemoveValueChanged(selector, CenterOnSelection_ItemsSourceChanged);
                }
            }
        }
        /// <summary>
        /// When the item source changes, we may need to re-center the new selected item
        /// </summary>
        /// <param name="sender">Item that sent the event</param>
        /// <param name="e">Event args</param>
        private static void CenterOnSelection_ItemsSourceChanged(object? sender, EventArgs e)
        {
            ListBox? listbox = sender as ListBox;
            if (listbox != null)
            {
                object selectedValue = listbox.SelectedValue;
                if (selectedValue != null)
                    listbox.Dispatcher.BeginInvoke(() => listbox.ScrollIntoViewCentered(selectedValue)).Priority = System.Windows.Threading.DispatcherPriority.Background;
            }
        }

        /// <summary>
        /// Dictionary to cache the "ScrollIntoView" method
        /// </summary>
        private static Dictionary<System.Windows.Controls.Primitives.Selector, MethodInfo> _scrollIntoViewMethod = new Dictionary<System.Windows.Controls.Primitives.Selector, MethodInfo>();

        private static void ScrollOnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = d as System.Windows.Controls.Primitives.Selector;
            if (selector != null)
            {
                if ((bool)e.NewValue)
                {
                    MethodInfo? method = selector.GetType().GetMethod("ScrollIntoView", BindingFlags.Public | BindingFlags.Instance);
                    if (method != null)
                    {
                        _scrollIntoViewMethod[selector] = method;
                        selector.SelectionChanged += selector_SelectionChangedScrollIntoView;
                        selector.Unloaded += selector_Unloaded;
                    }
                }
                else
                    TryUnhookSelectorScrollIntoView(selector);
            }
        }

        private static void TryUnhookSelectorScrollIntoView(System.Windows.Controls.Primitives.Selector? selector)
        {
            if (selector != null && _scrollIntoViewMethod.ContainsKey(selector))
            {
                _scrollIntoViewMethod.Remove(selector);
                selector.SelectionChanged -= selector_SelectionChangedScrollIntoView;
                selector.Unloaded -= selector_Unloaded;
            }
        }

        static void selector_Unloaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = sender as System.Windows.Controls.Primitives.Selector;
            TryUnhookSelectorScrollIntoView(selector);
        }

        static void selector_SelectionChangedScrollIntoView(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = sender as System.Windows.Controls.Primitives.Selector;

            if (selector != null)
            {
                ListBox? listBox = selector as ListBox;

                //  Get the item to scroll to
                object? item = null;
                if (e.AddedItems != null && e.AddedItems.Count == 1)
                    item = e.AddedItems[0];

                //  Get the ScrollIntoView method
                MethodInfo? scrollIntoView = null;
                if (_scrollIntoViewMethod.ContainsKey(selector))
                    scrollIntoView = _scrollIntoViewMethod[selector];

                if (item != null && scrollIntoView != null)
                {
                    if (listBox != null && GetCenterOnSelection(listBox))
                        listBox.ScrollIntoViewCentered(item);
                    else
                        scrollIntoView.Invoke(selector, new[] { item });
                }
            }
        }

        /// <summary>
        /// Sets whether or not to scroll into view when selection changes
        /// </summary>
        /// <param name="selector">Selector to scroll</param>
        /// <param name="value">Whether or not to scroll when selection changes</param>
        public static void SetScrollOnSelection(System.Windows.Controls.Primitives.Selector selector, bool value)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            selector.SetValue(ScrollOnSelectionProperty, value);
        }

        /// <summary>
        /// Gets whether or not to center into view when selection changes
        /// </summary>
        /// <param name="selector">Selector to scroll</param>
        /// <returns>Whether or not to scroll when selection changes</returns>
        public static bool GetScrollOnSelection(System.Windows.Controls.Primitives.Selector selector)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            return (bool)selector.GetValue(ScrollOnSelectionProperty);
        }

        /// <summary>
        /// Sets whether or not to scroll into view when selection changes
        /// </summary>
        /// <param name="selector">Selector to scroll</param>
        /// <param name="value">Whether or not to scroll when selection changes</param>
        public static void SetCenterOnSelection(ListBox selector, bool value)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            selector.SetValue(CenterOnSelectionProperty, value);
        }

        /// <summary>
        /// Gets whether or not to center into view when selection changes
        /// </summary>
        /// <param name="selector">Selector to scroll</param>
        /// <returns>Whether or not to scroll when selection changes</returns>
        public static bool GetCenterOnSelection(ListBox selector)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            return (bool)selector.GetValue(CenterOnSelectionProperty);
        }
        #endregion
        #region Focus First Control
        public static readonly DependencyProperty FocusFirstControlProperty = DependencyProperty.RegisterAttached("FocusFirstControl", typeof(bool), typeof(Selector),
            new FrameworkPropertyMetadata(FocusFirstControlChanged));

        /// <summary>
        /// Sets whether or not to focus the first item on selection changed
        /// </summary>
        /// <param name="selector">Selector to focus children</param>
        /// <param name="value">Whether or not to focus the first child when selection changes</param>
        public static void SetFocusFirstControl(System.Windows.Controls.Primitives.Selector selector, bool value)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            selector.SetValue(FocusFirstControlProperty, value);
        }

        /// <summary>
        /// Gets whether or not to focus the first item on selection changed
        /// </summary>
        /// <param name="selector">Selector to focus children</param>
        /// <returns>Whether or not to focus the first child when selection changes</returns>
        public static bool GetFocusFirstControl(System.Windows.Controls.Primitives.Selector selector)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            return (bool)selector.GetValue(FocusFirstControlProperty);
        }

        private static void FocusFirstControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = d as System.Windows.Controls.Primitives.Selector;

            if (selector != null)
            {
                if ((bool)e.NewValue)
                {
                    selector.SelectionChanged += selector_SelectionChangedFocusControl;
                    selector.Unloaded += selector_UnloadedScrollIntoView;
                }
                else
                {
                    TryUnhookSelectorFocusFirstControl(selector);
                }
            }
        }

        private static void TryUnhookSelectorFocusFirstControl(System.Windows.Controls.Primitives.Selector? selector)
        {
            Exceptions.ThrowIfArgumentNull(selector, nameof(selector));
            selector.SelectionChanged -= selector_SelectionChangedFocusControl;
            selector.Unloaded -= selector_UnloadedScrollIntoView;
        }

        private static void selector_UnloadedScrollIntoView(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = sender as System.Windows.Controls.Primitives.Selector;
            TryUnhookSelectorFocusFirstControl(selector);
        }

        private static void selector_SelectionChangedFocusControl(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = sender as System.Windows.Controls.Primitives.Selector;

            if (selector != null)
            {
                object? item = null;
                if (e.AddedItems != null && e.AddedItems.Count == 1)
                    item = e.AddedItems[0];

                ItemContainerGenerator generator = selector.ItemContainerGenerator;
                if (generator != null && item != null)
                {
                    selector.Dispatcher.BeginInvoke(() =>
                    {
                        DependencyObject element = generator.ContainerFromItem(item);
                        if (element != null)
                        {
                            UIElement? first = VisualTreeHelperEx.FindFocusableChildren(element).FirstOrDefault(p => p.IsMouseOver);
                            if (first == null)
                                first = VisualTreeHelperEx.FindFocusableChildren(element).FirstOrDefault();
                            if (first != null)
                                first.Focus();
                        }
                    }).Priority = System.Windows.Threading.DispatcherPriority.Background;

                }
            }
        }
        #endregion
        #region Sorting & Grouping
        public static readonly DependencyProperty SortPropertyNameProperty;
        public static readonly DependencyProperty IsSortableProperty;
        public static readonly DependencyProperty DefaultSortPropertyProperty;
        public static readonly DependencyProperty IsGroupableProperty;
        public static readonly DependencyProperty CurrentSortPropertyProperty;
        public static readonly DependencyProperty DefaultGroupPropertyProperty;

        private static void IsSortableChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView? listView = o as ListView;
            bool? sortable = e.NewValue as bool?;
            if ((listView != null) && sortable.HasValue)
            {
                if (sortable.Value)
                {
                    if (GetIsGroupable(listView))
                        throw new ArgumentException("Can not group and sort on column header click");
                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClicked));
                }
                else
                {
                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClicked));
                }
            }
        }

        private static void IsGroupableChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView? listView = o as ListView;
            bool? groupable = e.NewValue as bool?;
            if ((listView != null) && groupable.HasValue)
            {
                if (groupable.Value)
                {
                    if (GetIsSortable(listView))
                        throw new ArgumentException("Can not group and sort on column header click");
                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClicked));
                }
                else
                {
                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClicked));
                }
            }
        }

        private static void DefaultSortPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView? listView = o as ListView;
            string? property = e.NewValue as string;

            if ((listView != null) && !string.IsNullOrEmpty(property))
            {
                if (listView.View == null)
                    listView.Loaded += listView_LoadedSort;
                else
                    SetDefaultSortOrderForListView(listView, property);
            }
        }
        private static void DefaultGroupPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView? listView = o as ListView;
            string? property = e.NewValue as string;

            if ((listView != null) && !string.IsNullOrEmpty(property))
            {
                if (listView.View == null)
                    listView.Loaded += listView_LoadedGroup;
                else
                    SetDefaultGroupOrderForListView(listView, property);
            }
        }

        private static void CurrentSortPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView? listView = o as ListView;
            string? property = e.NewValue as string;
            if ((listView != null) && !property.IsNullOrEmpty())
            {
                if (listView.View == null)
                    listView.Loaded += listView_LoadedForCurrentSortProperty;
                else
                    FindAndSortColumn(listView, property);
            }
        }

        public static string GetSortPropertyName(GridViewColumn column)
        {
            Exceptions.ThrowIfArgumentNull(column, nameof(column));
            return (string)column.GetValue(SortPropertyNameProperty);
        }
        public static void SetSortPropertyName(GridViewColumn column, string propertyName)
        {
            Exceptions.ThrowIfArgumentNull(column, nameof(column));
            column.SetValue(SortPropertyNameProperty, propertyName);
        }
        public static bool GetIsSortable(ListView listView)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            return (bool)listView.GetValue(IsSortableProperty);
        }
        public static void SetIsSortable(ListView listView, bool sortable)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            listView.SetValue(IsSortableProperty, sortable);
        }
        public static string GetDefaultSortProperty(ListView listView)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            return (string)listView.GetValue(DefaultSortPropertyProperty);
        }
        public static void SetDefaultSortProperty(ListView listView, string propertyName)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            listView.SetValue(DefaultSortPropertyProperty, propertyName);
        }
        public static string GetDefaultGroupProperty(ListView listView)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            return (string)listView.GetValue(DefaultGroupPropertyProperty);
        }
        public static void SetDefaultGroupProperty(ListView listView, string propertyName)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            listView.SetValue(DefaultGroupPropertyProperty, propertyName);
        }
        public static bool GetIsGroupable(ListView listView)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            return (bool)listView.GetValue(IsGroupableProperty);
        }
        public static void SetIsGroupable(ListView listView, bool groupable)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            listView.SetValue(IsGroupableProperty, groupable);
        }
        public static void SetCurrentSortProperty(ListView listView, string value)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            listView.SetValue(CurrentSortPropertyProperty, value);
        }
        public static string GetCurrentSortProperty(ListView listView)
        {
            Exceptions.ThrowIfArgumentNull(listView, nameof(listView));
            return (string)listView.GetValue(CurrentSortPropertyProperty);
        }

        private static void SetSortOrder(GridViewColumn column, ListView view, string propertyName)
        {
            SortDescription? overrideSort = GetOverrideSort(propertyName);
            SortDescription description = view.Items.SortDescriptions.FirstOrDefault(p => p.PropertyName == propertyName);

            if (overrideSort.HasValue)
            {
                if (column != null)
                    SetColumnHeaderTemplate(view, column, overrideSort.Value.Direction);
                view.Items.SortDescriptions.Remove(description);
                view.Items.SortDescriptions.Add(overrideSort.Value);
            }
            else
            {
                if (description.PropertyName != null)
                {
                    view.Items.SortDescriptions.Remove(description);
                    if (column != null)
                        SetColumnHeaderTemplate(view, column, null);
                    if (description.Direction == ListSortDirection.Ascending)
                    {
                        if (column != null)
                            SetColumnHeaderTemplate(view, column, ListSortDirection.Descending);
                        view.Items.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Descending));
                    }
                }
                else
                {
                    if (column != null)
                        SetColumnHeaderTemplate(view, column, ListSortDirection.Ascending);
                    //  Can only sort by one column at a time
                    view.Items.SortDescriptions.Clear();
                    view.Items.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Ascending));
                }
                ICollectionViewLiveShaping shaping = view.Items;
                if (shaping != null && shaping.CanChangeLiveSorting)
                {
                    shaping.LiveSortingProperties.Clear();
                    shaping.LiveSortingProperties.Add(propertyName);
                    shaping.IsLiveSorting = true;
                }
            }
            view.Items.Refresh();
        }

        private static void SetColumnHeaderTemplate(ListView view, GridViewColumn column, ListSortDirection? direction)
        {
            if (!direction.HasValue)
                column.HeaderTemplate = null;
            else if (direction.Value == ListSortDirection.Descending)
                column.HeaderTemplate = view.FindResource("HeaderTemplateDescending") as DataTemplate;
            else
                column.HeaderTemplate = view.FindResource("HeaderTemplateAscending") as DataTemplate;
        }

        private static SortDescription? GetOverrideSort(string propertyName)
        {
            if (propertyName.Contains(';'))
            {
                string[] parts = propertyName.Split(';');
                return new SortDescription(parts[0], (ListSortDirection)Enum.Parse(typeof(ListSortDirection), parts[1]));
            }
            return null;
        }

        private static void SetDefaultSortOrderForListView(ListView listView, string property)
        {
            GridView? view = listView.View as GridView;
            GridViewColumn? column = null;

            if (view != null)
                column = view.Columns.FirstOrDefault(p => GetSortPropertyName(p) == property);

            if (column != null)
            SetSortOrder(column, listView, property);
        }

        private static void SetDefaultGroupOrderForListView(ListView listView, string property)
        {
            GridViewColumn? column = null;

            SetGroupOrder(column, listView, property, true);
        }

        private static void SetGroupOrder(GridViewColumn? gridViewColumn, ListView view, string propertyName, bool always)
        {
            PropertyGroupDescription? description = view.Items.GroupDescriptions.FirstOrDefault() as PropertyGroupDescription;
            view.Items.GroupDescriptions.Clear();
            if ((description == null) || (description.PropertyName != propertyName) || always)
            {
                description = new PropertyGroupDescription(propertyName);
                view.Items.GroupDescriptions.Add(description);
            }
            ICollectionViewLiveShaping shaping = view.Items;
            if (shaping != null && shaping.CanChangeLiveGrouping)
            {
                shaping.LiveGroupingProperties.Clear();
                shaping.LiveGroupingProperties.Add(propertyName);
                shaping.IsLiveGrouping = true;
            }
            view.Items.Refresh();
        }

        private static void FindAndSortColumn(ListView listView, string? property)
        {
            GridView? view = listView.View as GridView;
            if (view != null)
            {
                GridViewColumn? column = view.Columns.FirstOrDefault(p => GetSortPropertyName(p) == property);
                SortDescription description = listView.Items.SortDescriptions.FirstOrDefault();
                string previous = string.Empty;

                previous = description.PropertyName;

                if (!previous.IsNullOrEmpty())
                {
                    GridViewColumn? previousColumn = view.Columns.FirstOrDefault(p => GetSortPropertyName(p) == previous);
                    if (previousColumn != null)
                        previousColumn.HeaderTemplate = null;
                    listView.Items.SortDescriptions.Clear();
                }

                if (column != null && property != null)
                    SetSortOrder(column, listView, property);
            }
        }

        private static void ColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader? header = e.OriginalSource as GridViewColumnHeader;
            ListView? view = sender as ListView;
            if ((header != null) && (header.Column != null) && (view != null))
            {
                string propertyName = GetSortPropertyName(header.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    if (GetIsSortable(view))
                        SetSortOrder(header.Column, view, propertyName);
                    else
                        SetGroupOrder(header.Column, view, propertyName, false);
                }
            }
        }

        static void listView_LoadedSort(object sender, RoutedEventArgs e)
        {
            ListView? listView = sender as ListView;
            if (listView != null)
            {
                listView.Loaded -= listView_LoadedSort;
                string property = GetDefaultSortProperty(listView);
                SetDefaultSortOrderForListView(listView, property);
            }
        }

        static void listView_LoadedGroup(object sender, RoutedEventArgs e)
        {
            ListView? listView = sender as ListView;
            if (listView != null)
            {
                listView.Loaded -= listView_LoadedGroup;
                string property = GetDefaultGroupProperty(listView);
                SetDefaultGroupOrderForListView(listView, property);
            }
        }

        private static void listView_LoadedForCurrentSortProperty(object sender, RoutedEventArgs e)
        {
            ListView? listView = sender as ListView;
            if (listView != null)
            {
                listView.Loaded -= listView_LoadedSort;
                string property = GetCurrentSortProperty(listView);
                FindAndSortColumn(listView, property);
            }
        }
        #endregion
        #region Center Item Visible
        public static async void ScrollIntoViewCentered(this ListBox listBox, object item)
        {
            Exceptions.ThrowIfArgumentNull(listBox, nameof(listBox));
            Exceptions.ThrowIfArgumentNull(item, nameof(item));

            if (ScrollViewer.GetCanContentScroll(listBox))
            {
                int index = listBox.Items.IndexOf(item);
                if (index != -1)
                {
                    if (VisualTreeHelperEx.GetAllChildren<ScrollViewer>(listBox).FirstOrDefault() is ScrollViewer scrollInfo)
                    {
                        // Center the item by splitting the extra space
                        if (listBox.GetOrientation() == Orientation.Horizontal)
                        {
                            if (scrollInfo.ViewportWidth == 0)
                                await listBox.Dispatcher.InvokeAsync(() => scrollInfo.ScrollToHorizontalOffset(index - Math.Floor(scrollInfo.ViewportWidth / 2)), System.Windows.Threading.DispatcherPriority.Render);
                            else
                                scrollInfo.ScrollToHorizontalOffset(index - Math.Floor(scrollInfo.ViewportWidth / 2));
                        }
                        else
                        {
                            if (scrollInfo.ViewportHeight == 0)
                                await listBox.Dispatcher.InvokeAsync(() => scrollInfo.ScrollToVerticalOffset(index - Math.Floor(scrollInfo.ViewportHeight / 2)), System.Windows.Threading.DispatcherPriority.Render);
                            else
                                scrollInfo.ScrollToVerticalOffset(index - Math.Floor(scrollInfo.ViewportHeight / 2));
                        }
                    }
                }
            }
            // Get the container for the specified item
            else if (listBox.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement container)
            {
                // Get the bounds of the item container
                Rect rect = new Rect(new Point(), container.RenderSize);

                // Find constraining parent (either the nearest ScrollContentPresenter or the ListBox itself)
                FrameworkElement? constrainingParent = container;
                do
                {
                    constrainingParent = VisualTreeHelper.GetParent(constrainingParent) as FrameworkElement;
                } while ((null != constrainingParent) &&
                         (listBox != constrainingParent) &&
                         !(constrainingParent is ScrollContentPresenter));

                if (null != constrainingParent)
                {
                    // Inflate rect to fill the constraining parent
                    rect.Inflate(
                        Math.Max((constrainingParent.ActualWidth - rect.Width) / 2, 0),
                        Math.Max((constrainingParent.ActualHeight - rect.Height) / 2, 0));
                }

                // Bring the (inflated) bounds into view
                container.BringIntoView(rect);
            }
            else
            {
                if (listBox.Items.Contains(item))
                {
                    listBox.ScrollIntoView(item);
                    await listBox.Dispatcher.InvokeAsync(() => listBox.ScrollIntoViewCentered(item), System.Windows.Threading.DispatcherPriority.Render);
                }
            }
        }
        #endregion
        #region Focus Selected Item
        /// <summary>
        /// DependencyProperty for focusing the selected item when the control becomes visible
        /// </summary>
        public static DependencyProperty FocusSelectedItemProperty = DependencyProperty.RegisterAttached("FocusSelectedItem", typeof(bool), typeof(Selector),
            new FrameworkPropertyMetadata(false, FocusSelectedItemChanged));

        /// <summary>
        /// Gets whether or not the selector should focus the selected item
        /// </summary>
        /// <param name="selector">Selector to get value for</param>
        /// <returns>Whether or not the selected item should receive focus automatically</returns>
        public static bool GetFocusSelectedItem(System.Windows.Controls.Primitives.Selector selector)
        {
            return (bool)selector.GetValue(FocusSelectedItemProperty);
        }

        /// <summary>
        /// Sets whether or not the selector should focus the selected item
        /// </summary>
        /// <param name="selector">Selector to set value for</param>
        /// <param name="value">Whether or not the selector should focus the selected item automatically</param>
        public static void SetFocusSelectedItem(System.Windows.Controls.Primitives.Selector selector, bool value)
        {
            selector.SetValue(FocusSelectedItemProperty, value);
        }

        private static void FocusSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = d as System.Windows.Controls.Primitives.Selector;

            if (selector != null)
            {
                if ((bool)e.NewValue)
                {
                    if (!selector.IsLoaded)
                    {
                        selector.IsVisibleChanged += FocusSelectedItem_IsVisibleChanged;
                        selector.Loaded += FocusSelectedItem_Loaded;
                    }
                    else
                    {
                        FocusSelectedItem(selector);
                    }
                    selector.SelectionChanged += Selector_FocusItemSelectionChanged;
                }
                else
                {
                    selector.IsVisibleChanged += FocusSelectedItem_IsVisibleChanged;
                    selector.Loaded += FocusSelectedItem_Loaded;
                    selector.SelectionChanged -= Selector_FocusItemSelectionChanged;
                }
            }
        }

        private static void Selector_FocusItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.Selector selector)
                selector.FocusSelectedItem();
        }

        private static void FocusSelectedItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.Selector selector)
                FocusSelectedItem(selector);
        }

        private static void FocusSelectedItem_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Primitives.Selector? selector = sender as System.Windows.Controls.Primitives.Selector;
            if ((bool)e.NewValue && selector != null)
                FocusSelectedItem(selector);
        }

        /// <summary>
        /// Focuses the selected item in the given selector
        /// </summary>
        /// <param name="selector">Selector to focus item in</param>
        public static void FocusSelectedItem(this System.Windows.Controls.Primitives.Selector selector, bool retryIfNotFound = true)
        {
            if (selector?.SelectedItem != null)
            {
                ContentControl? control = selector.ItemContainerGenerator.ContainerFromItem(selector.SelectedItem) as ContentControl;
                if (control == null)
                {
                    if (retryIfNotFound)
                    {
                        selector.ItemContainerGenerator.StatusChanged += (s, e) =>
                        {
                            ItemContainerGenerator? gen = s as ItemContainerGenerator;
                            if (gen?.Status == GeneratorStatus.ContainersGenerated)
                                FocusSelectedItem(selector, false);
                        };
                    }
                }
                else
                    control.Focus();
            }
        }
        #endregion
        #region Full Page Scrolling
        public static readonly DependencyProperty FullPageScrollingProperty = DependencyProperty.RegisterAttached("FullPageScrolling", typeof(bool), typeof(Selector),
            new FrameworkPropertyMetadata(false, FullPageScrollingChanged));

        /// <summary>
        /// Gets whether or not full page scrolling is enabled for a control
        /// </summary>
        /// <param name="selector">Selector to get status of</param>
        /// <returns>Whether or not full page scrolling is enabled</returns>
        public static bool GetFullPageScrolling(System.Windows.Controls.Primitives.Selector selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return (bool)selector.GetValue(FullPageScrollingProperty);
        }

        /// <summary>
        /// Sets whether full page scrolling is enabled for a control
        /// </summary>
        /// <param name="selector">Selector to enable or disable full page scrolling for</param>
        /// <param name="enabled">Whether or not to enable full page scrolling</param>
        public static void SetFullPageScrolling(System.Windows.Controls.Primitives.Selector selector, bool enabled)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            selector.SetValue(FullPageScrollingProperty, enabled);
        }

        private static void FullPageScrollingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is System.Windows.Controls.Primitives.Selector selector)
            {
                if (e.NewValue is bool fullPage && fullPage)
                    selector.PreviewKeyDown += Selector_KeyDown;
                else
                    selector.PreviewKeyDown -= Selector_KeyDown;
            }
        }

        private static void Selector_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.Selector selector)
            {
                if (e.Key == System.Windows.Input.Key.PageDown)
                {
                    e.Handled = true;
                    ScrollPage(selector, down: true);
                }
                else if (e.Key == System.Windows.Input.Key.PageUp)
                {
                    e.Handled = true;
                    ScrollPage(selector, down: false);
                }
            }
        }

        private static void ScrollPage(System.Windows.Controls.Primitives.Selector selector, bool down)
        {
            ContentControl[] visibleItems = GetVisibleItems(selector);
            int count = visibleItems.Length;
            int index = selector.SelectedIndex;

            if (down)
                index = Math.Min(selector.Items.Count, index + count);
            else
                index = Math.Max(0, index - count);

            selector.SelectedIndex = index;
        }

        /// <summary>
        /// Gets a collection of visible items on the selector
        /// </summary>
        /// <param name="selector">Selector to get visible items for</param>
        /// <returns>Collection of visible items</returns>
        public static ContentControl[] GetVisibleItems(System.Windows.Controls.Primitives.Selector selector)
        {
            ItemContainerGenerator generator = selector.ItemContainerGenerator;

            //  Iterate through the Items list, not the source, because it should be sorted as expected
            return selector.Items
                .OfType<object>()
                .Select(p => generator.GetContentPresenterFor(p))
                .Where(p => p != null && VisualTreeHelperEx.IsItemCompletelyVisibleIn(selector, p))
                .OfType<ContentControl>()
                .ToArray();
        }

        /// <summary>
        /// Gets the content presenter for an item in a Selector
        /// </summary>
        /// <param name="generator">Selector's item container generator containing the item</param>
        /// <param name="item">Item to get content presenter for</param>
        /// <returns>Content presenter for the item</returns>
        private static ContentControl? GetContentPresenterFor(this ItemContainerGenerator generator, object item)
        {
            if (item is ContentControl)
                return (ContentControl)item;

            return generator.ContainerFromItem(item) as ContentControl;
        }
        #endregion
        #region Static Methods
        /// <summary>
        /// Gets the style for the <see cref="ItemsControl.ItemContainerStyle"/>, or creates a new one if one isn't set
        /// </summary>
        /// <typeparam name="T">TargetType for the style</typeparam>
        /// <param name="itemsControl">Items control to get <see cref="ItemsControl.ItemContainerStyle"/> for</param>
        /// <returns>Style that was retrieved or created</returns>
        public static Style GetOrCreateItemContainerStyle<T>(this ItemsControl itemsControl) where T : DependencyObject
        {
            Style style = itemsControl.ItemContainerStyle;
            if (style == null)
            {
                style = new Style(typeof(T));
                itemsControl.ItemContainerStyle = style;
            }
            else
            {
                style = new Style(style.TargetType, style);
                itemsControl.ItemContainerStyle = style;
            }
            return style;
        }
        /// <summary>
        /// Gets the orientation of the items control
        /// </summary>
        /// <param name="itemsControl">Items control to get the orientation for</param>
        /// <returns>Items control's panel's orientation</returns>
        /// <exception cref="InvalidOperationException">The panel in the ItemsControl doesn't have an orientation.</exception>
        public static Orientation GetOrientation(this ItemsControl itemsControl)
        {
            ItemsPresenter? presenter = VisualTreeHelperEx.GetAllChildren<ItemsPresenter>(itemsControl).FirstOrDefault();
            if (presenter != null)
            {
                Panel? panel = VisualTreeHelperEx.GetAllChildren<Panel>(presenter).FirstOrDefault();
                switch (panel)
                {
                    case StackPanel stack:
                        return stack.Orientation;
                    case VirtualizingStackPanel stack:
                        return stack.Orientation;
                    case WrapPanel wrap:
                        return wrap.Orientation;
                }
            }
            throw new InvalidOperationException("This method only works with ItemsControls that use a StackPanel or WrapPanel.");
        }
        #endregion
    }
}
