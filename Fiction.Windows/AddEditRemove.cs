using System.Windows;
using System.Windows.Controls;

namespace Fiction.Windows
{
    /// <summary>
    /// Bar that has Add/Edit/Remove buttons
    /// </summary>
    [TemplatePart(Name = "PART_UpperBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_AddButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_EditButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_RemoveButton", Type = typeof(Button))]
    public class AddEditRemove : Control
    {
        #region Constructors
        /// <summary>
        /// Initiatializes the AddEditRemove class
        /// </summary>
        static AddEditRemove()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddEditRemove), new FrameworkPropertyMetadata(typeof(AddEditRemove)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets whether or not the edit button is shown
        /// </summary>
        public bool AllowEdit
        {
            get { return (bool)GetValue(AllowEditProperty); }
            set { SetValue(AllowEditProperty, value); }
        }
        /// <summary>
        /// Gets or sets the command parameter to use for the remove button
        /// </summary>
        public object RemoveParameter
        {
            get { return GetValue(RemoveParameterProperty); }
            set { SetValue(RemoveParameterProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for AllowEdit
        /// </summary>
        public static readonly DependencyProperty AllowEditProperty = DependencyProperty.Register(nameof(AllowEdit), typeof(bool), typeof(AddEditRemove));
        /// <summary>
        /// DependencyProperty for <see cref="RemoveParameter"/>
        /// </summary>
        public static readonly DependencyProperty RemoveParameterProperty = DependencyProperty.Register(nameof(RemoveParameter), typeof(object), typeof(AddEditRemove));
        #endregion
    }
}
