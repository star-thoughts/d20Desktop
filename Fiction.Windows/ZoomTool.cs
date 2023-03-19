using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Fiction.Windows
{
    /// <summary>
    /// Tool that allows zooming into a rectangular region
    /// </summary>
    public class ZoomTool : IViewTool
    {
        #region Member Variables
        private ZoomAdorner? _adorner;
        #endregion
        #region IUIElementTool Members
        /// <summary>
        /// Tool is being activated for use
        /// </summary>
        /// <param name="view">View that it is being used on</param>
        public void Activate(UIElement view)
        {
        }
        /// <summary>
        /// Tool is being deactivated for use
        /// </summary>
        /// <param name="view">View that it was being used on</param>
        public void Deactivate(UIElement view)
        {
        }
        /// <summary>
        /// Handler for mouse down events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        public void MouseDown(UIElement view, MouseButtonEventArgs e)
        {
            Exceptions.ThrowIfArgumentNull(e, nameof(e));

            ZoomViewer? viewer = view as ZoomViewer;
            if (viewer != null)
            {
                if (_adorner != null)
                    AdornerLayer.GetAdornerLayer(view).Remove(_adorner);

                _adorner = new ZoomAdorner(view);
                AdornerLayer.GetAdornerLayer(view).Add(_adorner);
                _adorner.SetStart(e.GetPosition(view));

                view.CaptureMouse();
            }
        }

        /// <summary>
        /// Handler for mouse up events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        public void MouseUp(UIElement view, MouseButtonEventArgs e)
        {
            ZoomViewer? viewer = view as ZoomViewer;
            if (viewer != null)
            {
                if (_adorner != null)
                {
                    AdornerLayer.GetAdornerLayer(view).Remove(_adorner);

                    if (_adorner.StartPoint.HasValue && _adorner.EndPoint.HasValue)
                    {
                        Point start = _adorner.StartPoint.Value;
                        Point end = _adorner.EndPoint.Value;

                        start = viewer.GetContentPointAtZoomViewerPoint(start);
                        end = viewer.GetContentPointAtZoomViewerPoint(end);

                        viewer.ZoomToRectangle(new Rect(start, end));
                    }
                }

                if (view.IsMouseCaptured)
                    view.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Handler for mouse move events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        public void MouseMove(UIElement view, MouseEventArgs e)
        {
            Exceptions.ThrowIfArgumentNull(e, nameof(e));

            ZoomViewer? viewer = view as ZoomViewer;
            if (viewer != null)
            {
                if (_adorner != null)
                {
                    Point pt = e.GetPosition(view);
                    _adorner.SetEndPoint(pt);
                }
            }
        }
        #endregion
    }
}
