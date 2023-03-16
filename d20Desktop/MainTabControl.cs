using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fiction.GameScreen
{
    public sealed class MainTabControl : Selector
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MainTabControl"/>
        /// </summary>
        public MainTabControl()
        {
        }
        /// <summary>
        /// Initializes the <see cref="MainTabControl"/> class
        /// </summary>
        static MainTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainTabControl), new FrameworkPropertyMetadata(typeof(MainTabControl)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the template to use for tab contents
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        /// <summary>
        /// Gets or sets the template selector to use for tab contents
        /// </summary>
        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ContentTemplate"/>
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(MainTabControl));
        /// <summary>
        /// DependencyProperty for <see cref="ContentTemplateSelector"/>
        /// </summary>
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(MainTabControl));
        #endregion
    }
}
