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
    /// Control for editing a monster filter
    /// </summary>
    public sealed class MonsterFilter : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MonsterFilter"/> class
        /// </summary>
        static MonsterFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonsterFilter), new FrameworkPropertyMetadata(typeof(MonsterFilter)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the filter view model
        /// </summary>
        public MonsterFilterViewModel ViewModel
        {
            get { return (MonsterFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MonsterFilterViewModel), typeof(MonsterFilter));
        #endregion
    }
}
