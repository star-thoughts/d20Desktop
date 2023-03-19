using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Fiction.Windows
{
    /// <summary>
    /// Scrollviewer that allows for zooming content
    /// </summary>
    /// <remarks>
    /// Hosts content and allows the user to zoom using the mouse wheel controls.  Any controls placed in this must have a specific height and width set
    /// using the <see cref="FrameworkElement.Height"/> and <see cref="FrameworkElement.Width"/> properties.  It's not enough just to have the <see cref="FrameworkElement.ActualHeight"/>
    /// and <see cref="FrameworkElement.ActualWidth"/> properties set.
    /// </remarks>
    public class ZoomViewer : ScrollViewer
    {
        #region Constructors
        public ZoomViewer()
        {
            Loaded += ZoomViewer_Loaded;
            Unloaded += ZoomViewer_Unloaded;
            SizeChanged += ZoomViewer_SizeChanged;
        }

        /// <summary>
        /// Initializes the ZoomViewer class
        /// </summary>
        static ZoomViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomViewer), new FrameworkPropertyMetadata(typeof(ZoomViewer)));
        }
        #endregion
        #region Member Variables
        private bool _zoomChanging;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the amount of zoom
        /// </summary>
        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }
        /// <summary>
        /// Gets or sets the content presenter
        /// </summary>
        protected ScrollContentPresenter? ContentPresenterPart { get; set; }
        /// <summary>
        /// Gets or sets the current content element
        /// </summary>
        protected FrameworkElement? ContentElement { get; set; }
        /// <summary>
        /// Gets or sets the current state of the zoom
        /// </summary>
        public ZoomState ZoomState
        {
            get { return (ZoomState)GetValue(ZoomStateProperty); }
            set { SetValue(ZoomStateProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Zoom"/>
        /// </summary>
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ZoomViewer),
                new FrameworkPropertyMetadata(1.0, ZoomChanged, CoerceZoom));

        private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomViewer? viewer = d as ZoomViewer;
            if (viewer != null && !viewer._zoomChanging)
                viewer.ZoomState = ZoomState.Custom;
        }

        private static object CoerceZoom(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            if (value < 0.1)
                return 0.1;
            return value;
        }

        /// <summary>
        /// DependencyProperty for <see cref="ZoomState"/>
        /// </summary>
        public static readonly DependencyProperty ZoomStateProperty = DependencyProperty.Register(nameof(ZoomState), typeof(ZoomState), typeof(ZoomViewer),
            new FrameworkPropertyMetadata(ZoomState.Custom, ZoomStateChanged));

        private static void ZoomStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomViewer? viewer = d as ZoomViewer;
            if (viewer != null)
            {
                viewer.UpdateZoomState((ZoomState)e.NewValue);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Called to apply the template to the ZoomViewer
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(Commands.FitToPage, FitToPage_Executed, FitToPage_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.FitToWidth, FitToWidth_Executed, FitToWidth_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.FitToHeight, FitToHeight_Executed, FitToHeight_CanExecute));

            ContentPresenterPart = GetTemplateChild("PART_ContentPresenter") as ScrollContentPresenter;

            PreviewMouseWheel += ZoomViewer_MouseWheel;
        }

        private void FitToHeight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ZoomState = ZoomState.FitToHeight;
        }

        private void FitToHeight_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Content != null;
        }

        private void FitToWidth_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ZoomState = ZoomState.FitToWidth;
        }

        private void FitToWidth_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Content != null;
        }

        private void FitToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Content != null;
        }

        private void FitToPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ZoomState = ZoomState.Fit;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (IsLoaded)
            {
                AttachToContent();
            }
        }

        private void AttachToContent()
        {
            if (ContentPresenterPart != null)
            {
                if (ContentElement != null)
                    ContentElement.SizeChanged -= Content_SizeChanged;

                ContentElement = VisualTreeHelperEx.GetChildren(ContentPresenterPart).OfType<FrameworkElement>().FirstOrDefault();
                UpdateZoomState(ZoomState);

                if (ContentElement != null)
                    ContentElement.SizeChanged += Content_SizeChanged;
            }
        }

        private void Content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ContentElement != null)
            {
                //  Child control must have its height and width set at their root (not just ActualHeight and ActualWidth)
                if (!_zoomChanging && !double.IsNaN(ContentElement.Height) && !double.IsNaN(ContentElement.Width))
                {
                    UpdateZoomState(ZoomState);
                }
            }
        }

        private void UpdateZoomState(ZoomState state)
        {
            _zoomChanging = true;
            try
            {
                switch (state)
                {
                    case ZoomState.Custom:
                        //  Nothing to do here
                        break;
                    case ZoomState.Fit:
                        Fit();
                        break;
                    case ZoomState.FitToHeight:
                        FitToHeight();
                        break;
                    case ZoomState.FitToWidth:
                        FitToWidth();
                        break;
                }
            }
            finally
            {
                _zoomChanging = false;
            }
        }

        private void FitToWidth()
        {
            if (ContentElement != null && ContentPresenterPart != null)
            {
                double presenterWidth = ContentPresenterPart.ActualWidth * Zoom;
                double contentWidth = ContentElement.ActualWidth;

                if (contentWidth > 0)
                    Zoom = presenterWidth / contentWidth;
            }
        }

        private void FitToHeight()
        {
            if (ContentElement != null && ContentPresenterPart != null)
            {
                double presenterHeight = ContentPresenterPart.ActualHeight * Zoom;
                double contentHeight = ContentElement.ActualHeight;

                if (contentHeight > 0)
                    Zoom = presenterHeight / contentHeight;
            }
        }

        private void Fit()
        {
            if (ContentElement != null && ContentPresenterPart != null)
            {
                double presenterWidth = ContentPresenterPart.ActualWidth * Zoom;
                double contentWidth = ContentElement.ActualWidth;
                double widthZoom = presenterWidth / contentWidth;

                double presenterHeight = ContentPresenterPart.ActualHeight * Zoom;
                double contentHeight = ContentElement.ActualHeight;
                double heightZoom = presenterHeight / contentHeight;

                if (!double.IsInfinity(widthZoom) && !double.IsInfinity(heightZoom))
                    Zoom = Math.Min(widthZoom, heightZoom);
            }
        }

        void ZoomViewer_Loaded(object sender, RoutedEventArgs e)
        {
            AttachToContent();
        }

        private void ZoomViewer_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ContentElement != null)
            {
                ContentElement.SizeChanged -= Content_SizeChanged;
            }
        }

        private void ZoomViewer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                e.Handled = true;
                AdjustZoom(e.Delta / 2400d);
                //  Cause it to recenter on wherever the mouse cursor is if zooming in
                Point pt = e.GetPosition(this);
                Point content = GetContentPointAtZoomViewerPoint(pt);

                Dispatcher.BeginInvoke(() => SetPointLocation(content, pt), DispatcherPriority.Loaded, Array.Empty<object>());
            }
        }

        /// <summary>
        /// Zooms so that the rectangle on the content is visible to maximum extents of the zoom viewer
        /// </summary>
        /// <param name="rect">Rectangle to zoom to</param>
        public void ZoomToRectangle(Rect rect)
        {
            Size sz = new Size(ActualWidth, ActualHeight);
            double scaleX = ActualWidth / rect.Width;
            double scaleY = ActualHeight / rect.Height;

            double scale = Math.Min(scaleX, scaleY);
            Zoom = scale;
            CenterOnPoint(new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2)));
        }


        private void AdjustZoom(double delta)
        {
            Zoom = Math.Min(Math.Max(0.1, Zoom + delta), 10);
        }

        /// <summary>
        /// Sets the scroll viewer to center on a point in the content
        /// </summary>
        /// <param name="visibleCenter">Point to center on the control</param>
        public void CenterOnPoint(Point visibleCenter)
        {
            Point pt = new Point(visibleCenter.X * Zoom, visibleCenter.Y * Zoom);

            pt = new Point(pt.X - (ActualWidth / 2), pt.Y - (ActualHeight / 2));

            ScrollToHorizontalOffset(pt.X);
            ScrollToVerticalOffset(pt.Y);
        }

        /// <summary>
        /// Sets the location of a specific point in the content being displayed to the point specified on the zoom viewer
        /// </summary>
        /// <param name="pointOnContent">Point to set to a location</param>
        /// <param name="pointInZoomView">Location to move the point to</param>
        public void SetPointLocation(Point pointOnContent, Point pointInZoomView)
        {
            if (ContentElement != null)
            {
                Point pt = new Point(pointOnContent.X, pointOnContent.Y);
                pt = ContentElement.TransformToAncestor(this).Transform(pt);

                pt = new Point(pointInZoomView.X - pt.X, pointInZoomView.Y - pt.Y);

                ScrollToHorizontalOffset(HorizontalOffset - pt.X);
                ScrollToVerticalOffset(VerticalOffset - pt.Y);
            }
        }
        /// <summary>
        /// Gets the zoom viewer point from a point in the content
        /// </summary>
        /// <param name="pointOnContent">Point in the content</param>
        /// <returns>Point on the zoom viewer that matches the content point</returns>
        public Point GetPointLocation(Point pointOnContent)
        {
            Point pt = pointOnContent;
            if (ContentElement != null)
                pt = ContentElement.TransformToAncestor(this).Transform(pt);

            return pt;
        }

        /// <summary>
        /// Gets the zoom viewer rectangle from a rectangle in the content
        /// </summary>
        /// <param name="rectOnContent">Rect in the content</param>
        /// <returns>Rect on the zoom viewer that matches the content rect</returns>
        public Rect GetRectLocation(Rect rectOnContent)
        {
            Rect rect = rectOnContent;
            if (ContentElement != null)
                rect = ContentElement.TransformToAncestor(this).TransformBounds(rect);

            return rect;
        }

        /// <summary>
        /// Gets the center of the item contained within the scroll viewer
        /// </summary>
        /// <returns>Point on the item that is the center of the visible region</returns>
        public Point VisibleCenter
        {
            get
            {
                GeneralTransform transform = this.TransformToDescendant(ContentElement);
                if (transform != null)
                    return transform.Transform(new Point(ActualWidth / 2, ActualHeight / 2));
                return new Point(double.NegativeInfinity, double.NegativeInfinity);
            }
        }
        /// <summary>
        /// Gets the point of the content that is at the point in the Zoom Viewer
        /// </summary>
        /// <param name="pt">Point in the ZoomViewer</param>
        /// <returns>Point in the content being shown at that location</returns>
        public Point GetContentPointAtZoomViewerPoint(Point pt)
        {
            GeneralTransform transform = this.TransformToDescendant(ContentElement);
            return transform.Transform(pt);
        }

        private void ZoomViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_zoomChanging)
                UpdateZoomState(ZoomState);
        }
        #endregion
    }
}
