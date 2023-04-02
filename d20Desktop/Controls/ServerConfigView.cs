using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// View for configuring server connections
    /// </summary>
    public sealed class ServerConfigView : Control
    {
        /// <summary>
        /// Initializes the <see cref="ServerConfigView"/> class
        /// </summary>
        static ServerConfigView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ServerConfigView), new FrameworkPropertyMetadata(typeof(ServerConfigView)));
        }

        /// <summary>
        /// Gets or sets the view model to use for configuring server connection
        /// </summary>
        public ServerConfigViewModel? ViewModel
        {
            get { return (ServerConfigViewModel?)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ServerConfigViewModel), typeof(ServerConfigView));
    }
}
