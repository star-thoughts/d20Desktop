using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for configuring the filter for spells
    /// </summary>
    public sealed class SpellFilter : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SpellFilter"/> class
        /// </summary>
        static SpellFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpellFilter), new FrameworkPropertyMetadata(typeof(SpellFilter)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the spell filter
        /// </summary>
        public SpellFilterViewModel ViewModel
        {
            get { return (SpellFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Methods
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(SpellFilterViewModel), typeof(SpellFilter));
        #endregion
    }
}
