using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing a list of strings
    /// </summary>
    public sealed class StringListEditor : Selector
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="StringListEditor"/> class
        /// </summary>
        static StringListEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringListEditor), new FrameworkPropertyMetadata(typeof(StringListEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the collection of strings to edit
        /// </summary>
        public ObservableCollection<string> StringCollection
        {
            get { return (ObservableCollection<string>)GetValue(StringCollectionProperty); }
            set {  SetValue(StringCollectionProperty, value); }
        }
        /// <summary>
        /// Gets or sets optional strings that can be added
        /// </summary>
        public IEnumerable<string> Options
        {
            get { return (IEnumerable<string>)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not new items can be added to the Options collection
        /// </summary>
        public bool CanAddNew
        {
            get { return (bool)GetValue(CanAddNewProperty); }
            set { SetValue(CanAddNewProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="StringCollection"/>
        /// </summary>
        public static readonly DependencyProperty StringCollectionProperty = DependencyProperty.Register(nameof(StringCollection), typeof(ObservableCollection<string>), typeof(StringListEditor));
        /// <summary>
        /// DependencyProperty for <see cref="Options"/>
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(IEnumerable<string>), typeof(StringListEditor));
        /// <summary>
        /// DependencyProperty for <see cref="CanAddNew"/>
        /// </summary>
        public static readonly DependencyProperty CanAddNewProperty = DependencyProperty.Register(nameof(CanAddNew), typeof(bool), typeof(StringListEditor));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (StringCollection != null)
                {
                    string value = EnterValueWindow.GetValue(Window.GetWindow(this), string.Empty, Options, CanAddNew);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        StringCollection.Add(value);
                        RaiseEvent(new StringEditedEventArgs(this, StringEditEventType.Added, value));
                    }
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = StringCollection != null;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is string s && StringCollection != null)
                {
                    StringCollection.Remove(s);
                    RaiseEvent(new StringEditedEventArgs(this, StringEditEventType.Removed, s));
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (e.Parameter is string) && StringCollection != null;
        }
        #endregion
        #region Events
        /// <summary>
        /// RoutedEvent for <see cref="StringEdited"/>
        /// </summary>
        public static readonly RoutedEvent StringEditedEvent = EventManager.RegisterRoutedEvent(nameof(StringEdited), RoutingStrategy.Bubble, typeof(StringEditedEventHandler), typeof(StringListEditor));
        /// <summary>
        /// Event that is triggered when a string is added, removed, or edited
        /// </summary>
        public event StringEditedEventHandler StringEdited
        {
            add { AddHandler(StringEditedEvent, value); }
            remove { RemoveHandler(StringEditedEvent, value); }
        }
        #endregion
    }
}
