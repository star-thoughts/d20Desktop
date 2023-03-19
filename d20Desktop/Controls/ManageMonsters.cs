using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.ViewModels;
using Fiction.GameScreen.ViewModels.EditMonsterViewModels;
using Fiction.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing monsters in a campaign
    /// </summary>
    public sealed class ManageMonsters : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManageMonsters"/> class
        /// </summary>
        static ManageMonsters()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageMonsters), new FrameworkPropertyMetadata(typeof(ManageMonsters)));
        }
        #endregion
        #region Member Variables
        private MonsterList? _monsterList;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for managing monsters
        /// </summary>
        public ManageMonstersViewModel ViewModel
        {
            get { return (ManageMonstersViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value);}
        }
        /// <summary>
        /// Gets or sets the currently selected monster
        /// </summary>
        public Monster SelectedMonster
        {
            get { return (Monster)GetValue(SelectedMonsterProperty); }
            set { SetValue(SelectedMonsterProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageMonstersViewModel), typeof(ManageMonsters));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedMonster"/>
        /// </summary>
        public static readonly DependencyProperty SelectedMonsterProperty = DependencyProperty.Register(nameof(SelectedMonster), typeof(Monster), typeof(ManageMonsters));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.CreateCopy, CreateCopy_Executed, CreateCopy_CanExecute));

            _monsterList = Template.FindName("PART_MonsterList", this) as MonsterList;
            if (_monsterList != null)
                _monsterList.AddHandler(FilterableList.ItemDoubleClickedEvent, new ItemDoubleClickedEventHandler(_monsterList_MonsterDoubleClick));
        }

        private void _monsterList_MonsterDoubleClick(object sender, ItemDoubleClickedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.Item is Monster monster)
                    EditMonster(monster);
            });
        }

        private void CreateCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is Monster monster)
                {
                    EditMonster(monster, createCopy: true);
                }
            });
        }

        private void CreateCopy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (e.Parameter is Monster);
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is Monster monster)
                {
                    ViewModel.Manager.Monsters.Remove(monster);
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is Monster;
        }

        private void Monster_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is FrameworkElement element && element.DataContext is Monster monster)
                EditMonster(monster);
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is Monster monster)
                {
                    EditMonster(monster);
                }
            });
        }

        private void EditMonster(Monster monster, bool createCopy = false)
        {
            EditMonsterViewModel viewModel = new EditMonsterViewModel(ViewModel.Campaign, monster);
            EditWindow window = new EditWindow();
            window.DataContext = viewModel;
            window.Owner = Window.GetWindow(this);

            if (createCopy)
                viewModel.MarkAsCopy();

            if (window.ShowDialog() == true && viewModel?.Monster != null)
            {
                viewModel.Save();
                    ViewModel.Monsters.Add(viewModel.Monster);
                SelectedMonster = viewModel.Monster;
                ViewModel?.Campaign?.MonsterManager?.Reconcile();
            }
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is Monster;
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel?.Monsters != null)
                {
                    EditMonsterViewModel viewModel = new EditMonsterViewModel(ViewModel.Campaign);
                    EditWindow window = new EditWindow();
                    window.DataContext = viewModel;
                    window.Owner = Window.GetWindow(this);

                    if (window.ShowDialog() == true)
                    {
                        viewModel.Save();
                        //await (_monsterList?.DispatchDisplayMonster(viewModel.Monster) ?? Task.CompletedTask);
                    }
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel?.Monsters != null;
        }

        /// <summary>
        /// Gets a filter to use for filtering out unused stats
        /// </summary>
        public static FilterEventHandler MonsterViewerStatFilter { get { return new FilterEventHandler(MonsterViewerStatFilterMethod); } }
        private static void MonsterViewerStatFilterMethod(object sender, FilterEventArgs e)
        {
            if (e.Item is IMonsterStatViewModel)
            {
                switch (e.Item)
                {
                    case CollectionStatViewModel stat:
                        e.Accepted = stat.Value?.Any() == true;
                        break;
                    case StringStatViewModel stat:
                        e.Accepted = !string.IsNullOrWhiteSpace(stat.Value);
                        break;
                    case AlignmentStatViewModel stat:
                        e.Accepted = stat.Value != Alignment.Unknown;
                        break;
                    case SizeStatViewModel stat:
                        e.Accepted = stat.Value != MonsterSize.Unknown;
                        break;
                }
            }
            else
                e.Accepted = false;
        }

        /// <summary>
        /// Gets a filter to use for showing only defensive stats
        /// </summary>
        public static FilterEventHandler MonsterDefenseStatFilter { get { return new FilterEventHandler(MonsterDefenseStatFilterMethod); } }

        private static void MonsterDefenseStatFilterMethod(object sender, FilterEventArgs e)
        {
            if (e.Item is IMonsterStatViewModel group && string.Equals(group.Category, GameScreen.Resources.Resources.DefenseLabel, StringComparison.CurrentCulture))
            {
                switch (e.Item)
                {
                    case CollectionStatViewModel stat:
                        e.Accepted = stat.Value?.Any() == true;
                        break;
                    case StringStatViewModel stat:
                        e.Accepted = !string.IsNullOrWhiteSpace(stat.Value);
                        break;
                    case AlignmentStatViewModel stat:
                        e.Accepted = stat.Value != Alignment.Unknown;
                        break;
                    case SizeStatViewModel stat:
                        e.Accepted = stat.Value != MonsterSize.Unknown;
                        break;
                }
            }
            else
                e.Accepted = false;
        }
        #endregion
    }
}
