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
    /// Control for editing a magic item filter
    /// </summary>
    public sealed class MagicItemFilter : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MagicItemFilter"/> class
        /// </summary>
        static MagicItemFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MagicItemFilter), new FrameworkPropertyMetadata(typeof(MagicItemFilter)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the filter to edit
        /// </summary>
        public MagicItemFilterViewModel ViewModel
        {
            get { return (MagicItemFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MagicItemFilterViewModel), typeof(MagicItemFilter));
        #endregion
    }
}
