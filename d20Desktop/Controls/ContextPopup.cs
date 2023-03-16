using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Popup used for right clicking on objects
    /// </summary>
    public sealed class ContextPopup : ContextMenu
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ContextPopup"/> class
        /// </summary>
        static ContextPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextPopup), new FrameworkPropertyMetadata(typeof(ContextPopup)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the content to display in this popup
        /// </summary>
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        /// <summary>
        /// Gets or sets the template for the content
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selector for data templates
        /// </summary>
        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Content"/>
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(ContextPopup));
        /// <summary>
        /// DependencyProperty for <see cref="ContentTemplate"/>
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContextPopup));
        /// <summary>
        /// DependencyProperty for <see cref="ContentTemplateSelector"/>
        /// </summary>
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(ContextPopup));
        #endregion
        #region Methods
        #endregion
    }
}
