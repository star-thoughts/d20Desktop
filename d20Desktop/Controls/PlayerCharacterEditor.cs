using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Editor for adding/editing player characters
    /// </summary>
    public sealed class PlayerCharacterEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="PlayerCharacterEditor"/> class
        /// </summary>
        static PlayerCharacterEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlayerCharacterEditor), new FrameworkPropertyMetadata(typeof(PlayerCharacterEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the character to edit
        /// </summary>
        public EditPlayerCharacterViewModel Character
        {
            get { return (EditPlayerCharacterViewModel)GetValue(CharacterProperty); }
            set { SetValue(CharacterProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Character"/>
        /// </summary>
        public static readonly DependencyProperty CharacterProperty = DependencyProperty.Register(nameof(Character), typeof(EditPlayerCharacterViewModel), typeof(PlayerCharacterEditor));
        #endregion
        #region Methods
        #endregion
    }
}
