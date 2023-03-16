using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fiction.Windows
{
    /// <summary>
    /// Interface for tools used in the definition and use of maps
    /// </summary>
    public interface IViewTool
    {
        /// <summary>
        /// Tool is being activated for use
        /// </summary>
        /// <param name="view">View that it is being used on</param>
        void Activate(UIElement view);
        /// <summary>
        /// Tool is being deactivated for use
        /// </summary>
        /// <param name="view">View that it was being used on</param>
        void Deactivate(UIElement view);
        /// <summary>
        /// Handler for mouse down events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        void MouseDown(UIElement view, MouseButtonEventArgs e);

        /// <summary>
        /// Handler for mouse up events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        void MouseUp(UIElement view, MouseButtonEventArgs e);

        /// <summary>
        /// Handler for mouse move events
        /// </summary>
        /// <param name="view">UIElement that has the event</param>
        /// <param name="e">Event information</param>
        void MouseMove(UIElement view, MouseEventArgs e);
    }
}
