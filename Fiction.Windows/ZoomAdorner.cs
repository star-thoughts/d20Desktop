using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Fiction.Windows
{
    /// <summary>
    /// Adorner used to draw zoom rectangles
    /// </summary>
    class ZoomAdorner : Adorner
    {
        #region Constructors
        public ZoomAdorner(UIElement view)
            : base(view)
        {
            this.IsHitTestVisible = false;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the starting point of the zoom
        /// </summary>
        public Point? StartPoint { get; private set; }
        /// <summary>
        /// Gets the ending point of the zoom
        /// </summary>
        public Point? EndPoint { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the start point
        /// </summary>
        /// <param name="start">Start point of the rectangle</param>
        public void SetStart(Point start)
        {
            StartPoint = start;
            EndPoint = start;
            InvalidateVisual();
        }
        /// <summary>
        /// Sets the end point
        /// </summary>
        /// <param name="end">End point of the rectangle</param>
        public void SetEndPoint(Point end)
        {
            EndPoint = end;
            InvalidateVisual();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            Exceptions.ThrowIfArgumentNull(drawingContext, nameof(drawingContext));

            if (StartPoint != EndPoint)
            {
                drawingContext.DrawRectangle(null, new Pen(Brushes.DarkBlue, 2), new Rect(StartPoint.Value, EndPoint.Value));
            }
            base.OnRender(drawingContext);
        }
        #endregion
    }
}
