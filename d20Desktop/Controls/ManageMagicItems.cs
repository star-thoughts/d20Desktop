using Fiction.GameScreen.Equipment;
using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing magic items in the campaign
    /// </summary>
    public sealed class ManageMagicItems : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManageMagicItems"/> class
        /// </summary>
        static ManageMagicItems()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageMagicItems), new FrameworkPropertyMetadata(typeof(ManageMagicItems)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public MagicItemsViewModel ViewModel
        {
            get { return (MagicItemsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MagicItemsViewModel), typeof(ManageMagicItems));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.CreateCopy, Copy_Executed, Copy_CanExecute));

            FilterableList? list = Template.FindName("PART_List", this) as FilterableList;
            if (list != null)
                list.ItemDoubleClicked += List_ItemDoubleClicked;
        }

        private void List_ItemDoubleClicked(object sender, ItemDoubleClickedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.Item is MagicItem item)
                    EditItem(new EditMagicItemViewModel(ViewModel.Factory.Campaign, item));
            });
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                EditItem(new EditMagicItemViewModel(ViewModel.Factory.Campaign));
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is MagicItem item)
                {
                    EditMagicItemViewModel vm = new EditMagicItemViewModel(ViewModel.Factory.Campaign, item);
                    EditItem(vm);
                }
            });
        }

        private void EditItem(EditMagicItemViewModel vm)
        {
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = vm;

            if (window.ShowDialog() == true)
                vm.Save();
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is MagicItem;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is MagicItem item)
                {
                    ViewModel.Factory.Campaign.EquipmentManager.MagicItems.Remove(item);
                    ViewModel.Factory.Campaign.EquipmentManager.Reconcile();
                    ViewModel.Factory.Campaign.ReconcileSources();
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is MagicItem;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is MagicItem item)
                {
                    EditMagicItemViewModel vm = new EditMagicItemViewModel(ViewModel.Factory.Campaign, item);
                    vm.MarkAsCopy();
                    EditItem(vm);
                }
            });
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is MagicItem;
        }
        #endregion
    }
}
