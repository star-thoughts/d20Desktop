using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Popup to allow quick navigation to group headers in a Selector
    /// </summary>
    public sealed class GroupPopup : ListBox
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="GroupPopup"/> class
        /// </summary>
        static GroupPopup()
        {
        }
        #endregion
        #region Member Variables
        private Popup _parentPopup;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the groups
        /// </summary>
        public IEnumerable<CollectionViewGroup> Groups
        {
            get { return (IEnumerable<CollectionViewGroup>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not the popup is open
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Groups"/>
        /// </summary>
        public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(nameof(Groups), typeof(IEnumerable<CollectionViewGroup>), typeof(GroupPopup));
        /// <summary>
        /// DependencyProperty for <see cref="IsOpen"/>
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GroupPopup),
            new FrameworkPropertyMetadata(false, IsOpenChanged));

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GroupPopup group)
            {
                if ((bool)e.NewValue)
                {
                    group._parentPopup.IsOpen = true;
                }
                else
                {
                    group._parentPopup.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// DependencyProperty for Popup
        /// </summary>
        public static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached("Popup", typeof(GroupPopup), typeof(GroupPopup),
            new FrameworkPropertyMetadata(null, PopupChanged));


        /// <summary>
        /// Gets the popup associated with the group item
        /// </summary>
        /// <param name="group">GroupItem to get associated popup for</param>
        /// <returns>Popup associated with the GroupItem</returns>
        public static GroupPopup GetPopup(GroupItem group)
        {
            return (GroupPopup)group.GetValue(PopupProperty);
        }

        /// <summary>
        /// Sets the popup associated with the GroupItem
        /// </summary>
        /// <param name="group">GroupItem to set associated group for</param>
        /// <param name="popup">Popup to associated with the GroupItem</param>
        public static void SetPopup(GroupItem group, GroupPopup popup)
        {
            group.SetValue(PopupProperty, popup);
        }

        private static void PopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GroupItem group)
            {
                UpdateGroup(group, e.NewValue as GroupPopup);
            }
        }
        #endregion
        #region Methods

        private void HookupParentPopup(GroupItem target)
        {
            _parentPopup = new Popup();

            _parentPopup.AllowsTransparency = true;
            _parentPopup.PlacementTarget = target;

            _parentPopup.SetResourceReference(Popup.PopupAnimationProperty, SystemParameters.MenuPopupAnimationKey);

            // Hooks up the popup properties from this menu to the popup so that
            // setting them on this control will also set them on the popup.
            Popup.CreateRootPopup(_parentPopup, this);
        }

        private static void UpdateGroup(GroupItem group, GroupPopup groupPopup)
        {
            if (group.IsLoaded)
            {
                ContentPresenter presenter = group.Template.FindName("PART_Header", group) as ContentPresenter;
                if (presenter != null)
                    presenter.PreviewMouseLeftButtonDown += Presenter_PreviewMouseLeftButtonDown;

                if (groupPopup._parentPopup == null)
                    groupPopup.HookupParentPopup(group);
            }
            else
                group.Loaded += Group_Loaded;
        }

        private static void Presenter_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ContentPresenter presenter)
            {
                GroupItem group = VisualTreeHelperEx.GetParent<GroupItem>(presenter);
                if (group != null)
                {
                    GroupPopup popup = GetPopup(group);
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }

        private static void Group_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is GroupItem group)
            {
                group.Loaded -= Group_Loaded;
                UpdateGroup(group, GetPopup(group));
            }
        }
        #endregion
    }
}
