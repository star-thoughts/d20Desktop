using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control that contains an add, edit and remove button
    /// </summary>
    public sealed class AddRemoveEdit : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="AddRemoveEdit"/> class
        /// </summary>
        static AddRemoveEdit()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddRemoveEdit), new FrameworkPropertyMetadata(typeof(AddRemoveEdit)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets whether or not the add button is visible
        /// </summary>
        public bool AllowAdd
        {
            get { return (bool)GetValue(AllowAddProperty); }
            set { SetValue(AllowAddProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether the edit button is visible
        /// </summary>
        public bool AllowEdit
        {
            get { return (bool)GetValue(AllowEditProperty); }
            set { SetValue(AllowEditProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not the copy button is visible
        /// </summary>
        public bool AllowCopy
        {
            get { return (bool)GetValue(AllowCopyProperty); }
            set { SetValue(AllowCopyProperty, value);}
        }
        /// <summary>
        /// Gets or sets the orientation of the control
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// Gets or sets the parametr to supply for the <see cref="Commands.Remove"/> and <see cref="Commands.Edit"/> commands
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="AllowAdd"/>
        /// </summary>
        public static readonly DependencyProperty AllowAddProperty = DependencyProperty.Register(nameof(AllowAdd), typeof(bool), typeof(AddRemoveEdit));
        /// <summary>
        /// DependencyProperty for <see cref="AllowEdit"/>
        /// </summary>
        public static readonly DependencyProperty AllowEditProperty = DependencyProperty.Register(nameof(AllowEdit), typeof(bool), typeof(AddRemoveEdit));
        /// <summary>
        /// DependencyProperty for <see cref="AllowCopy"/>
        /// </summary>
        public static readonly DependencyProperty AllowCopyProperty = DependencyProperty.Register(nameof(AllowCopy), typeof(bool), typeof(AddRemoveEdit));
        /// <summary>
        /// DependencyProperty for <see cref="Orientation"/>
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(AddRemoveEdit));
        /// <summary>
        /// DependencyProperty for <see cref="CommandParameter"/>
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(AddRemoveEdit));
        #endregion
    }
}
