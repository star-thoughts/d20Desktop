using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.Windows
{
    /// <summary>
    /// Data template selector that supports XAML
    /// </summary>
    public class XamlTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Constructs a new <see cref="XamlTemplateSelector"/>
        /// </summary>
        public XamlTemplateSelector()
        {
            Templates = new XamlTemplateCollection();
        }
        /// <summary>
        /// Gets a collection of templates to use
        /// </summary>
        public XamlTemplateCollection Templates { get; set; }
        /// <summary>
        /// Gets a template to fall back on if no other template is found
        /// </summary>
        public DataTemplate Fallback { get; set; }

        /// <summary>
        /// Selects the template for the data
        /// </summary>
        /// <param name="item">Data to select template for</param>
        /// <param name="container">Objec containing the data</param>
        /// <returns>Template to use</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = Templates.OfType<XamlTemplate>().FirstOrDefault(p => p.Type.IsAssignableFrom(item?.GetType()))?.Template;
            if (item != null && template == null)
                template = Fallback;

            return template;
        }
    }

    /// <summary>
    /// Collection of <see cref="XamlTemplate"/> objects
    /// </summary>
    /// <remarks>
    /// This list directly inherits from <see cref="IList"/> and <see cref="IList{T}"/> so that the xaml designer won't complain about
    /// trying to add items to it.
    /// </remarks>
    public class XamlTemplateCollection : List<XamlTemplate>, IList, IList<XamlTemplate>
    {
    }
}
