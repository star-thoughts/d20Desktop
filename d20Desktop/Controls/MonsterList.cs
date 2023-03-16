using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.ViewModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Listbox for showing monsters that allows filtering and sorting
    /// </summary>
    public sealed class MonsterList : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MonsterList"/> class
        /// </summary>
        static MonsterList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonsterList), new FrameworkPropertyMetadata(typeof(MonsterList)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a collection of monsters
        /// </summary>
        public IEnumerable Monsters
        {
            get { return (IEnumerable)GetValue(MonstersProperty); }
            set { SetValue(MonstersProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected monster
        /// </summary>
        public Monster SelectedMonster
        {
            get { return (Monster)GetValue(SelectedMonsterProperty); }
            set { SetValue(SelectedMonsterProperty, value); }
        }
        /// <summary>
        /// Gets or sets hte filter to use
        /// </summary>
        public MonsterFilterViewModel Filter
        {
            get { return (MonsterFilterViewModel)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        /// <summary>
        /// Gets or sets a collection of selected monsters
        /// </summary>
        public ObservableCollection<Monster> SelectedMonsters
        {
            get { return (ObservableCollection<Monster>)GetValue(SelectedMonsterProperty); }
            set { SetValue(SelectedMonsterProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Monsters"/>
        /// </summary>
        public static readonly DependencyProperty MonstersProperty = DependencyProperty.Register(nameof(Monsters), typeof(IEnumerable), typeof(MonsterList));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedMonster"/>
        /// </summary>
        public static readonly DependencyProperty SelectedMonsterProperty = DependencyProperty.Register(nameof(SelectedMonster), typeof(Monster), typeof(MonsterList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedMonsters"/>
        /// </summary>
        public static readonly DependencyProperty SelectedMonstersProperty = DependencyProperty.Register(nameof(SelectedMonsters), typeof(ObservableCollection<Monster>), typeof(MonsterList));
        /// <summary>
        /// DependencyProperty for <see cref="Filter"/>
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(MonsterFilterViewModel), typeof(MonsterList));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {

        }

        #endregion
    }
}
