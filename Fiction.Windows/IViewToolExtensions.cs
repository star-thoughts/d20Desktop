using System.Windows;

namespace Fiction.Windows
{
    /// <summary>
    /// Extensions for IViewTool
    /// </summary>
    public static class IViewToolExtensions
    {
        static IViewToolExtensions()
        {
            _attached = new Dictionary<FrameworkElement, IViewTool>();
        }
        private static Dictionary<FrameworkElement, IViewTool> _attached;
        /// <summary>
        /// Attaches this tool to a view
        /// </summary>
        /// <param name="tool">Tool to attach</param>
        /// <param name="view">View to attach to</param>
        public static void AttachToolView(this IViewTool tool, FrameworkElement view)
        {
            Exceptions.ThrowIfArgumentNull(view, nameof(view));
            Exceptions.ThrowIfArgumentNull(tool, nameof(tool));

            _attached[view] = tool;

            view.PreviewMouseDown += view_MouseDown;
            view.PreviewMouseMove += view_MouseMove;
            view.PreviewMouseUp += view_MouseUp;
            view.Unloaded += view_Unloaded;


            tool.Activate(view);
        }

        /// <summary>
        /// Detaches the tool from the view it is attached to
        /// </summary>
        /// <param name="tool">Tool to detach</param>
        public static void DetachFromView(this IViewTool tool)
        {
            if (tool != null)
            {
                FrameworkElement? view = _attached
                    .Where(p => object.ReferenceEquals(tool, p.Value))
                    .Select(p => p.Key)
                    .FirstOrDefault();

                if (view != null)
                {
                    _attached.Remove(view);

                    view.PreviewMouseDown -= view_MouseDown;
                    view.PreviewMouseMove -= view_MouseMove;
                    view.PreviewMouseUp -= view_MouseUp;
                    view.Unloaded -= view_Unloaded;

                    tool.Deactivate(view);
                }
            }
        }

        /// <summary>
        /// Determines whether or not the given tool is currently attached to any views
        /// </summary>
        /// <param name="tool">Tool to test</param>
        /// <returns>Whether or not the tool is attached to a view</returns>
        public static bool IsAttached(this IViewTool tool)
        {
            return _attached.Any(p => object.ReferenceEquals(tool, p.Value));
        }

        static void view_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                IViewTool tool = _attached[(FrameworkElement)sender];
                if (tool != null)
                    tool.DetachFromView();
            }
        }

        static void view_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                IViewTool tool = _attached[(FrameworkElement)sender];
                if (tool != null)
                    tool.MouseUp((UIElement)sender, e);
            }
        }

        static void view_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                IViewTool tool = _attached[(FrameworkElement)sender];
                if (tool != null)
                    tool.MouseMove((UIElement)sender, e);
            }
        }

        static void view_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                IViewTool tool = _attached[(FrameworkElement)sender];
                if (tool != null)
                    tool.MouseDown((UIElement)sender, e);

            }
        }
    }
}
