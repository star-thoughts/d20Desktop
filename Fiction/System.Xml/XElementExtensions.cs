using Fiction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace System.Xml
{
    /// <summary>
    /// Extensions for XElement and XAttribute
    /// </summary>
    public static class XElementExtensionsEx
    {
        /// <summary>
        /// Reads an attribute from an element and returns it's value as a string
        /// </summary>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="defaultValue">Default value if the value doesn't exist</param>
        /// <returns>Value of the attribute</returns>
        public static string ReadAttributeString(this XElement element, string name, string? defaultValue = null)
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
                return attribute.Value;

            return defaultValue ?? string.Empty;
        }
        /// <summary>
        /// Reads an attribute from an element and returns it's value as a double
        /// </summary>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="defaultValue">Default value if the value doesn't exist</param>
        /// <returns>Value of the attribute</returns>
        public static double ReadAttributeDouble(this XElement element, string name, double defaultValue = 0.0)
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
            {
                if (double.TryParse(attribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                    return result;
            }

            return defaultValue;
        }
        /// <summary>
        /// Reads an attribute from an element and returns it's value as a Guid
        /// </summary>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="create">Whether or not to create a new guid if there isn't a guid</param>
        /// <returns>Value of the attribute</returns>
        public static Guid ReadAttributeGuid(this XElement element, string name, bool create = false)
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
            {
                Guid result;
                if (Guid.TryParse(attribute.Value, out result))
                    return result;
            }

            return create ? Guid.NewGuid() : Guid.Empty;
        }

        /// <summary>
        /// Reads an attribute from an element and returns it's value as a bool
        /// </summary>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="defaultValue">Default value if the value doesn't exist</param>
        /// <returns>Value of the attribute</returns>
        public static bool ReadAttributeBool(this XElement element, string name, bool defaultValue = false)
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
            {
                bool result;
                if (bool.TryParse(attribute.Value, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Reads an attribute from an element and returns it's value as a int
        /// </summary>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="defaultValue">Default value if the value doesn't exist</param>
        /// <returns>Value of the attribute</returns>
        public static int ReadAttributeInt(this XElement element, string name, int defaultValue = -1)
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
            {
                int result;
                if (int.TryParse(attribute.Value, out result))
                    return result;
            }

            return defaultValue;
        }
        /// <summary>
        /// Reads an enum value from an attribute
        /// </summary>
        /// <typeparam name="T">Type of enum to read</typeparam>
        /// <param name="element">Element to read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="defaultValue">Default value if the attribute doesn't exist</param>
        /// <returns>Value read from attribute, or <paramref name="defaultValue"/> if attribute doesn't exist</returns>
        public static T ReadAttributeEnum<T>(this XElement element, string name, T defaultValue = default) where T : struct
        {
            Exceptions.ThrowIfArgumentNull(element, nameof(element));

            XAttribute? attribute = element.Attribute(name);
            if (attribute != null)
            {
                T result = defaultValue;
                if (Enum.TryParse(attribute.Value, out result))
                    return result;
            }
            return defaultValue;
        }
        /// <summary>
        /// Reads a collection of strings using the given reader and element name to get the values from
        /// </summary>
        /// <param name="reader">XmlReader to read from</param>
        /// <param name="name">Element names to use to get values</param>
        /// <returns>Collection of values from the elements of the given name</returns>
        public static IEnumerable<string> ReadCollectionElement(this XElement reader, string name)
        {
            foreach (XElement child in reader.Elements(name))
            {
                yield return child.Value;
            }
        }

        /// <summary>
        /// Reads an attribute value as a DateTime
        /// </summary>
        /// <param name="reader">XElement that is being read from</param>
        /// <param name="name">Name of the attribute to read</param>
        /// <param name="default">Default value if the attribute does not exist, or does not have a valid DateTime</param>
        /// <returns>DateTime read from the attribute</returns>
        public static DateTime ReadAttributeDate(this XElement reader, string name, DateTime @default = default)
        {
            string? dateTime = reader.Attribute(name)?.Value;
            DateTime result = @default;

            if (string.IsNullOrEmpty(dateTime) || !DateTime.TryParse(dateTime, CultureInfo.CurrentCulture, DateTimeStyles.RoundtripKind, out result))
                result = @default;

            return result;
        }

        /// <summary>
        /// Gets the value in a child node
        /// </summary>
        /// <param name="reader">XElement that is being read from</param>
        /// <param name="name">Name of the child node</param>
        /// <param name="default">Default value if the node does not exist</param>
        /// <returns>Value read from the node</returns>
        public static string ReadElementValue(this XElement reader, string name, string @default = "")
        {
            return reader.Descendants(name)?.FirstOrDefault()?.Value ?? @default;
        }
    }
}
