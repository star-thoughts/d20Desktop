using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.ViewModels;
using Fiction.GameScreen.ViewModels.EditMonsterViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for editing a monster and it's stats
    /// </summary>
    public sealed class MonsterEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MonsterEditor"/> class
        /// </summary>
        static MonsterEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonsterEditor), new FrameworkPropertyMetadata(typeof(MonsterEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the monster to edit
        /// </summary>
        public EditMonsterViewModel ViewModel
        {
            get { return (EditMonsterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditMonsterViewModel), typeof(MonsterEditor));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.OriginalSource is FrameworkElement element && element.DataContext is CollectionStatViewModel stat && e.Parameter is string value)
                    stat.Value?.Remove(value);
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.OriginalSource is FrameworkElement element && element.DataContext is CollectionStatViewModel && e.Parameter is string;
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.OriginalSource is FrameworkElement element && element.DataContext is CollectionStatViewModel stat)
                {
                    IEnumerable<string>? options = stat.Source;

                    string value = EnterValueWindow.GetValue(Window.GetWindow(this), string.Empty, options ?? Array.Empty<string>(), stat.CanAddNew);

                    if (!string.IsNullOrWhiteSpace(value) && (stat.Value?.Contains(value, StringComparer.CurrentCultureIgnoreCase) == false))
                        stat.Value.Add(value);
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.OriginalSource is FrameworkElement element && element.DataContext is CollectionStatViewModel;
        }

        #endregion
    }
}
