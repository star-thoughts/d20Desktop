using Fiction.GameScreen.Spells;
using Fiction.GameScreen.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control that shows a list of spells and allows different sorting/filtering options
    /// </summary>
    public sealed class SpellList : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SpellList"/> class
        /// </summary>
        static SpellList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpellList), new FrameworkPropertyMetadata(typeof(SpellList)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the list of spells to display
        /// </summary>
        public IEnumerable<Spell> Spells
        {
            get { return (IEnumerable<Spell>)GetValue(SpellsProperty); }
            set { SetValue(SpellsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the campaign the spells are in
        /// </summary>
        public CampaignSettings Campaign
        {
            get { return (CampaignSettings)GetValue(CampaignProperty); }
            set { SetValue(CampaignProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected spell
        /// </summary>
        public Spell SelectedSpell
        {
            get { return (Spell)GetValue(SelectedSpellProperty); }
            set { SetValue(SelectedSpellProperty, value); }
        }
        /// <summary>
        /// Gets or sets the current name/school filter
        /// </summary>
        public SpellFilterViewModel Filter
        {
            get { return (SpellFilterViewModel)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Spells"/>
        /// </summary>
        public static readonly DependencyProperty SpellsProperty = DependencyProperty.Register(nameof(Spells), typeof(IEnumerable<Spell>), typeof(SpellList));
        /// <summary>
        /// DependencyProperty for <see cref="Campaign"/>
        /// </summary>
        public static readonly DependencyProperty CampaignProperty = DependencyProperty.Register(nameof(Campaign), typeof(CampaignSettings), typeof(SpellList));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedSpell"/>
        /// </summary>
        public static readonly DependencyProperty SelectedSpellProperty = DependencyProperty.Register(nameof(SelectedSpell), typeof(Spell), typeof(SpellList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// DependencyProperty for <see cref="Filter"/>
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(SpellFilterViewModel), typeof(SpellList));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
        }
        #endregion
    }
}
