using System.Windows;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Contains settings information for the application
    /// </summary>
    public class AppSettings : DependencyObject
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether or not touch support is enabled
        /// </summary>
        public bool IsTouchEnabled
        {
            get { return (bool)GetValue(IsTouchEnabledProperty); }
            set { SetValue(IsTouchEnabledProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="IsTouchEnabled"/>
        /// </summary>
        public static readonly DependencyProperty IsTouchEnabledProperty = DependencyProperty.Register(nameof(IsTouchEnabled), typeof(bool), typeof(AppSettings));
        #endregion

    }
}