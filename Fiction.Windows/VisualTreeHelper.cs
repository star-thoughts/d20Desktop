using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Fiction;

namespace System.Windows.Media
{
	/// <summary>
	/// Helper methods for navigating the visual tree
	/// </summary>
	public static class VisualTreeHelperEx
	{
		/// <summary>
		/// Gets a collection of children that are considered focusable
		/// </summary>
		/// <param name="source">Item to start search from</param>
		/// <returns>IEnumerable of UIElement objects that are focusable and children of the source item</returns>
		public static IEnumerable<UIElement> FindFocusableChildren(DependencyObject source)
		{
			foreach (DependencyObject current in GetChildren(source))
			{
				UIElement element = current as UIElement;
				if (element != null && element.Focusable)
					yield return element;
				foreach (UIElement child in FindFocusableChildren(current))
					yield return child;
			}
		}

		/// <summary>
		/// Gets a collection of children of an item
		/// </summary>
		/// <param name="source">Source item to get children from</param>
		/// <returns>IEnumerable of DependencyObject objects that are the child of the source item</returns>
		public static IEnumerable<DependencyObject> GetChildren(DependencyObject source)
		{
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

			for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(source); i++)
			{
				yield return System.Windows.Media.VisualTreeHelper.GetChild(source, i);
			}
		}

        /// <summary>
        /// Gets all of the child elements of a specified type, recursively
        /// </summary>
        /// <param name="source">Source object to get children for</param>
        /// <typeparam name="T">Type of child element to get</typeparam>
        /// <returns>All child elements of the specified type, recursive</returns>
        public static IEnumerable<T> GetAllChildren<T>(DependencyObject source)
        {
            IEnumerable<DependencyObject> elements = VisualTreeHelperEx.GetChildren(source);
            IEnumerable<T> result = elements.OfType<T>().Concat(elements.SelectMany(p => GetAllChildren<T>(p)));
            return result.OfType<T>();
        }

        /// <summary>
        /// Gets the closest parent of the given type
        /// </summary>
        /// <typeparam name="T">Type of parent to find</typeparam>
        /// <param name="source">Source item to get parent of</param>
        /// <returns>Parent item of the given type, or null if no parent of that type exists</returns>
        public static T GetParent<T>(DependencyObject source) where T : DependencyObject
        {
            T result = null;
            DependencyObject item = source;
            while (result == null)
            {
                item = VisualTreeHelper.GetParent(item);
                result = item as T;
            }
            return result;
        }

        /// <summary>
        /// Gets a collection of all FrameworkElement items that fall within the given rectangle
        /// </summary>
        /// <typeparam name="T">Type of items to look for</typeparam>
        /// <param name="source">Source item to look for children within</param>
        /// <param name="area">Area to search</param>
        /// <returns>Collection of all FrameworkElement items that are within the area</returns>
        public static IEnumerable<T> GetAllChildren<T>(Visual source, Rect area) where T : FrameworkElement
        {
            foreach (T item in GetAllChildren<T>(source))
            {
                Point topLeft = source.TransformToAncestor(item).Transform(new Point(0, 0));
                Size size = new Size(item.ActualWidth, item.ActualHeight);
                Rect rect = new Rect(topLeft, size);

                if (area.IntersectsWith(rect))
                    yield return item;
            }
        }

        /// <summary>
        /// Determines whether the given item is visible within the container
        /// </summary>
        /// <param name="container">Container to test</param>
        /// <param name="item">Item to test against</param>
        /// <returns>Whether or not the given item is visible in the container</returns>
        public static bool IsItemVisibleIn(this FrameworkElement container, FrameworkElement item)
        {
            Exceptions.ThrowIfArgumentNull(container, nameof(container));
            Exceptions.ThrowIfArgumentNull(item, nameof(item));
            //  If item isn't visible, then it's not visible within the container
            if (!item.IsVisible)
                return false;

            Rect bounds = item.TransformToAncestor(container)
                .TransformBounds(new Rect(0.0, 0.0, item.ActualWidth, item.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }
        /// <summary>
        /// Determines whether the given item is visible within the container
        /// </summary>
        /// <param name="container">Container to test</param>
        /// <param name="item">Item to test against</param>
        /// <returns>Whether or not the given item is visible in the container</returns>
        public static bool IsItemCompletelyVisibleIn(this FrameworkElement container, FrameworkElement item)
        {
            Exceptions.ThrowIfArgumentNull(container, nameof(container));
            Exceptions.ThrowIfArgumentNull(item, nameof(item));
            //  If item isn't visible, then it's not visible within the container
            if (!item.IsVisible)
                return false;

            Rect bounds = item.TransformToAncestor(container)
                .TransformBounds(new Rect(0.0, 0.0, item.ActualWidth, item.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return rect.Contains(bounds.TopLeft) && rect.Contains(bounds.BottomRight);
        }
        /// <summary>
        /// Gets a container in the items control at the given point
        /// </summary>
        /// <param name="itemsControl">Items control to get container from</param>
        /// <param name="pt">Point to find container at</param>
        /// <returns>Container found, or null if no container could be located</returns>
        public static FrameworkElement GetItemsControlContainerAtPoint(ItemsControl itemsControl, Point pt)
        {
            DependencyObject element = itemsControl.InputHitTest(pt) as DependencyObject;
            DependencyObject container = null;
            while (container == null && element != null)
            {
                if (itemsControl.IsItemItsOwnContainer(element))
                    container = element;
                else if (element is FrameworkContentElement fce)
                    element = fce.Parent;
                else
                    element = VisualTreeHelper.GetParent(element);
            }
            return container as FrameworkElement;
        }
    }
}
