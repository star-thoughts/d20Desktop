using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Contains resource keys for finding resources in XAML
    /// </summary>
    public static class ResourceKeys
    {
        #region Brushes
        /// <summary>
        /// Gets the key for FadeToRightBrush
        /// </summary>
        public static ComponentResourceKey FadeToRightBrush { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(FadeToRightBrush));
        /// <summary>
        /// Gets the key for FadeToLeftBrush
        /// </summary>
        public static ComponentResourceKey FadeToLeftBrush { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(FadeToLeftBrush));
        /// <summary>
        /// Gets the key for FadeFromCenterBrush
        /// </summary>
        public static ComponentResourceKey FadeFromCenterBrush { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(FadeFromCenterBrush));
        #endregion
        #region Fonts
        /// <summary>
        /// Gets the key for TextFontSize
        /// </summary>
        public static ComponentResourceKey TextFontSize { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(TextFontSize));
        /// <summary>
        /// Gets the key for the size of headers
        /// </summary>
        public static ComponentResourceKey HeaderFontSize { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(HeaderFontSize));
        /// <summary>
        /// Gets the key for the size of subheaders
        /// </summary>
        public static ComponentResourceKey SubheaderFontSize { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(SubheaderFontSize));
        /// <summary>
        /// Gets the key for the font color in headers
        /// </summary>
        public static ComponentResourceKey HeaderFontColor { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(HeaderFontColor));
        /// <summary>
        /// Gets the key for the default text color
        /// </summary>
        public static ComponentResourceKey TextFontColor { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(TextFontColor));
        #endregion
        #region Styles
        /// <summary>
        /// Gets the key for the default style for vertical splitters
        /// </summary>
        public static ComponentResourceKey VerticalSplitterStyle { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(VerticalSplitterStyle));
        /// <summary>
        /// Gets the key for the default style for horizontal splitters
        /// </summary>
        public static ComponentResourceKey HorizontalSplitterStyle { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(HorizontalSplitterStyle));
        /// <summary>
        /// Gets the key for the default style for ListBoxItems
        /// </summary>
        public static ComponentResourceKey ListBoxItemStyle { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(ListBoxItemStyle));
        #endregion
        #region Data Templates
        /// <summary>
        /// Gets the key for a DataTemplate for a <see cref="CombatantPreparer"/>
        /// </summary>
        public static ComponentResourceKey CombatantPreparerTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(CombatantPreparerTemplate));
        /// <summary>
        /// Gets the key for a readonly DataTemplate for a <see cref="CombatantPreparer"/>
        /// </summary>
        public static ComponentResourceKey CombatantPreparerViewTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(CombatantPreparerViewTemplate));
        /// <summary>
        /// Gets the key for a template to view monster stats
        /// </summary>
        public static ComponentResourceKey MonsterViewTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(MonsterViewTemplate));
        /// <summary>
        /// Gets the key for a template for viewing the defensive stats of a monster
        /// </summary>
        public static ComponentResourceKey MonsterDefenseTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(MonsterDefenseTemplate));
        /// <summary>
        /// Gets the key for a template for beginning to import monsters
        /// </summary>
        public static ComponentResourceKey BeginMonsterImportTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(BeginMonsterImportTemplate));
        /// <summary>
        /// Gets the key for a template for beginning to import spells
        /// </summary>
        public static ComponentResourceKey BeginSpellImportTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(BeginSpellImportTemplate));
        /// <summary>
        /// Gets the key for a template for beginning to import magic items
        /// </summary>
        public static ComponentResourceKey BeginMagicItemImportTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(BeginMagicItemImportTemplate));
        /// <summary>
        /// Gets the key for a template to view the monsters before finally importing them
        /// </summary>
        public static ComponentResourceKey ViewImportedMonstersTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(ViewImportedMonstersTemplate));
        /// <summary>
        /// Gets the key for a template to view an object with a built-in scrollviewer
        /// </summary>
        public static ComponentResourceKey ObjectViewTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(ObjectViewTemplate));
        /// <summary>
        /// Gets the key for a template to view an object without scrolling
        /// </summary>
        public static ComponentResourceKey StaticObjectViewTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(StaticObjectViewTemplate));
        /// <summary>
        /// Gets the key for a template to view a player character
        /// </summary>
        public static ComponentResourceKey PlayerCharacterTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(PlayerCharacterTemplate));
        /// <summary>
        /// Gets the key for a template to view a spell's description
        /// </summary>
        public static ComponentResourceKey SpellTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(SpellTemplate));
        /// <summary>
        /// Gets the key for a template to use for displaying an effect's name in a list
        /// </summary>
        public static ComponentResourceKey EffectNameTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(EffectNameTemplate));
        /// <summary>
        /// Gets a key for a template to use for displaying an effect
        /// </summary>
        public static ComponentResourceKey EffectTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(EffectTemplate));
        /// <summary>
        /// Gets a key for a template to use for displaying a magic item
        /// </summary>
        public static ComponentResourceKey MagicItemTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(MagicItemTemplate));
        /// <summary>
        /// Gets a key for a template to use for group headers
        /// </summary>
        public static ComponentResourceKey GroupHeaderTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(GroupHeaderTemplate));
        /// <summary>
        /// Gets a key for a template to use for displaying a spell name
        /// </summary>
        public static ComponentResourceKey SpellNameTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(SpellNameTemplate));
        /// <summary>
        /// Gets a key for a template to use for displaying a magic item's name
        /// </summary>
        public static ComponentResourceKey MagicItemNameTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(MagicItemNameTemplate));
        /// <summary>
        /// Gets a key for a template to use for displaying a monster's name
        /// </summary>
        public static ComponentResourceKey MonsterNameTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(MonsterNameTemplate));
        /// <summary>
        /// Gets a key for a templet to use for editing damage reduction
        /// </summary>
        public static ComponentResourceKey EditDamageReductionTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(EditDamageReductionTemplate));
        #endregion
        #region Control Templates
        /// <summary>
        /// Gets the key for the control template for a spell list editor with buttons on the side instead of underneath
        /// </summary>
        public static ComponentResourceKey StringListEditorSideBySideTemplate { get; } = new ComponentResourceKey(typeof(ResourceKeys), nameof(StringListEditorSideBySideTemplate));
        #endregion
    }
}
