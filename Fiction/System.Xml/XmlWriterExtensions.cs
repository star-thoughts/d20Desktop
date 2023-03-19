using System.Collections;
using Fiction;

namespace System.Xml
{
    /// <summary>
    /// Extensions for the XmlWriter class
    /// </summary>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Writes a collection of items to the XMl output, all contained within a named root element
        /// </summary>
        /// <param name="writer">XmlWriter to use for XML output</param>
        /// <param name="root">Name of the root element to place all elements in</param>
        /// <param name="name">Name to use for each item's element</param>
        /// <param name="items">Collection of items to write</param>
        public static void WriteCollectionElement(this XmlWriter writer, string root, string name, IEnumerable items)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            writer.WriteStartElement(root);

            writer.WriteCollectionElement(name, items);

            writer.WriteEndElement();
        }
        /// <summary>
        /// Writes a collection of items to the XMl output
        /// </summary>
        /// <param name="writer">XmlWriter to use for XML output</param>
        /// <param name="name">Name to use for each item's element</param>
        /// <param name="items">Collection of items to write</param>
        public static void WriteCollectionElement(this XmlWriter writer, string name, IEnumerable items)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));
            Exceptions.ThrowIfArgumentNull(items, nameof(items));

            foreach (object item in items)
            {
                writer.WriteStartElement(name);
                writer.WriteValue(item.ToString());
                writer.WriteEndElement();
            }
        }
        /// <summary>
        /// Writes a collection of items to the XMl output asynchronously
        /// </summary>
        /// <param name="writer">XmlWriter to use for XML output</param>
        /// <param name="name">Name to use for each item's element</param>
        /// <param name="items">Collection of items to write</param>
        /// <returns>Task for task completion.</returns>
        public static async Task WriteCollectionElementAsync(this XmlWriter writer, string name, IEnumerable items)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));
            Exceptions.ThrowIfArgumentNull(items, nameof(items));

            foreach (object item in items)
            {
                await writer.WriteElementStringAsync(name, item.ToString() ?? string.Empty);
            }
        }
        /// <summary>
        /// Starts an XML element
        /// </summary>
        /// <param name="writer">XmlWriter to use to write the element</param>
        /// <param name="localName">Name to give the element</param>
        public static Task WriteStartElementAsync(this XmlWriter writer, string localName)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            return writer.WriteStartElementAsync(string.Empty, localName, string.Empty);
        }
        /// <summary>
        /// Writes an attribute to the current element
        /// </summary>
        /// <param name="writer">XmlWriter to use</param>
        /// <param name="localName">Name of the attribute</param>
        /// <param name="value">Value for the attribute</param>
        /// <returns></returns>
        public static Task WriteAttributeStringAsync(this XmlWriter writer, string localName, string value)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            return writer.WriteAttributeStringAsync(string.Empty, localName, string.Empty, value);
        }
        /// <summary>
        /// Writes an element with the given value to current element
        /// </summary>
        /// <param name="writer">XmlWriter to use</param>
        /// <param name="localName">Name of the element to write</param>
        /// <param name="value">Value to write to the element given in <paramref name="localName"/></param>
        /// <returns>Task for task completion</returns>
        public static Task WriteElementStringAsync(this XmlWriter writer, string localName, string value)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            return writer.WriteElementStringAsync(string.Empty, localName, string.Empty, value);
        }
        /// <summary>
        /// Writes an attribute to the XmlWriter
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        /// <param name="localName">Name to give the attribute</param>
        /// <param name="value">DateTime value to write to the attribute</param>
        /// <returns>Task for task completion</returns>
        public static Task WriteAttributeDateAsync(this XmlWriter writer, string localName, DateTime value)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            return writer.WriteAttributeStringAsync(string.Empty, localName, string.Empty, value.ToString("o"));
        }
        /// <summary>
        /// Writes a string as an attribute, as long as the string is not empty or null
        /// </summary>
        /// <param name="writer">Writer for xml</param>
        /// <param name="name">Name of the attribute to write</param>
        /// <param name="value">Value to write</param>
        /// <returns>Task for task completion</returns>
        public async static Task WriteOptionalAttributeStringAsync(this XmlWriter writer, string name, string value)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            if (!string.IsNullOrEmpty(value))
                await writer.WriteAttributeStringAsync(name, value);
        }
        /// <summary>
        /// Writes a string as an attribute, as long as the string is not empty or null
        /// </summary>
        /// <param name="writer">Writer for xml</param>
        /// <param name="name">Name of the attribute to write</param>
        /// <param name="value">Value to write</param>
        /// <returns>Task for task completion</returns>
        public static void WriteOptionalAttributeString(this XmlWriter writer, string name, string value)
        {
            Exceptions.ThrowIfArgumentNull(writer, nameof(writer));

            if (!string.IsNullOrEmpty(value))
                writer.WriteAttributeString(name, value);
        }
    }
}
