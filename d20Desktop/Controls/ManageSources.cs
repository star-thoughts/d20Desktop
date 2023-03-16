using Fiction.GameScreen.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control to manage sources in a campaign
    /// </summary>
    public sealed class ManageSources : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManageSources"/> class
        /// </summary>
        static ManageSources()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageSources), new FrameworkPropertyMetadata(typeof(ManageSources)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public ManageSourcesViewModel ViewModel
        {
            get { return (ManageSourcesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected source
        /// </summary>
        public string SelectedSource
        {
            get { return (string)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageSourcesViewModel), typeof(ManageSources));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedSource"/>
        /// </summary>
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register(nameof(SelectedSource), typeof(string), typeof(ManageSources));

        private static void SelectedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is ManageSources view)
                    view?.UpdateSourceItems(e.NewValue as string);
            });
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel != null && e.Parameter is string source && source != null)
                {
                    string displaySource = string.IsNullOrWhiteSpace(source) ? GameScreen.Resources.Resources.UnnamedSourceLabel : source;
                    string warning = String.Format(CultureInfo.CurrentCulture, GameScreen.Resources.Resources.RemoveSourceWarning, displaySource);
                    Window window = Window.GetWindow(this);

                    if (MessageBox.Show(window, warning, window.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        ViewModel.Factory.Campaign.RemoveSource(source);
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is string source && source != null;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel != null && e.Parameter is string source && source != null)
                {
                    Window window = Window.GetWindow(this);
                    string rename = EnterValueWindow.GetValue(window, source);
                    if (!string.IsNullOrWhiteSpace(rename) && !string.Equals(rename, source, StringComparison.CurrentCulture))
                    {
                        string warning = String.Format(CultureInfo.CurrentCulture, GameScreen.Resources.Resources.MergeSourceWarning, source, rename);
                        bool allow = string.Equals(rename, source, StringComparison.CurrentCultureIgnoreCase)
                            || !ViewModel.Sources.Contains(rename, StringComparer.CurrentCultureIgnoreCase)
                            || MessageBox.Show(window, warning, window.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;

                        if (allow)
                            ViewModel.Factory.Campaign.RenameSource(source, rename);
                    }
                }
            });
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is string source && source != null;
        }

        private void UpdateSourceItems(string source)
        {
            if (source != null)
                ViewModel?.ShowItemsFromSource(source);
        }
        #endregion
    }
}
