using Fiction;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    public static class INotifyPropertyChangedExtensions
    {
        public static void RaisePropertyChanged(this INotifyPropertyChanged item, [CallerMemberName] string property = "")
        {
            Exceptions.ThrowIfArgumentNull(item, nameof(item));
            Exceptions.ThrowIfArgumentNull(property, nameof(property));

            TestProperty(item, property);

            TypeInfo itemType = item.GetType().GetTypeInfo();
            EventInfo? info = itemType.GetDeclaredEvent("PropertyChanged")
                ?? item.GetType().GetRuntimeEvent("PropertyChanged");

            if (info == null)
                throw new InvalidOperationException($"Class of type {item.GetType().Name} does not properly implement INotifyPropertyChanged.");

            RaisePropertyChanged(item, info, itemType, property);
        }

        private static void RaisePropertyChanged(INotifyPropertyChanged item, EventInfo info, TypeInfo itemType, params string[] properties)
        {
            //  We'll assume if it implements INotifyPropertyChanged that it has the event
            //if (info.IsMulticast)
            {
                TypeInfo? notifyType = itemType;
                FieldInfo? field = null;

                while (field == null && notifyType != null)
                {
                    field = notifyType.DeclaredFields.FirstOrDefault(p => !p.IsPublic && !p.IsStatic && p.Name == info.Name);
                    if (field == null)
                        notifyType = notifyType?.BaseType?.GetTypeInfo();
                }

                Type? t = info.EventHandlerType;
                if (t != null)
                {
                    MethodInfo invocListInfo = t.GetRuntimeMethods().First(p => p.Name == "GetInvocationList");
                    object? target = field?.GetValue(item);
                    if (target != null)
                    {
                        Delegate[]? delegates = invocListInfo.Invoke(target, null) as Delegate[];
                        if (delegates != null)
                        {
                            foreach (string property in properties)
                            {
                                object[] parameters = new object[]
                                {
                            item,
                            new PropertyChangedEventArgs(property),
                                };


                                foreach (Delegate del in delegates)
                                {
                                    try { del.DynamicInvoke(parameters); }
                                    catch (Exception exc) { Exceptions.RaiseIgnoredException(exc); }
                                }
                            }
                        }
                    }
                }
            }
            //else
            //{
            //	MethodInfo method = info.RaiseMethod;
            //	if (method != null)
            //	{
            //		foreach (string property in properties)
            //		{
            //			object[] parameters = new object[]
            //			{
            //				item,
            //				new PropertyChangedEventArgs(property),
            //			};


            //			method.Invoke(item, parameters);
            //		}
            //	}
            //}
        }
        public static void RaisePropertiesChanged(this INotifyPropertyChanged item, params string[] properties)
        {
            Exceptions.ThrowIfArgumentNull(item, nameof(item));

            TestProperties(item, properties);

            TypeInfo itemType = item.GetType().GetTypeInfo();
            EventInfo? info = itemType.GetDeclaredEvent("PropertyChanged")
                ?? item.GetType().GetRuntimeEvent("PropertyChanged");

            if (info == null)
                throw new InvalidOperationException($"Class of type {item.GetType().Name} does not properly implement INotifyPropertyChanged.");

            RaisePropertyChanged(item, info, itemType, properties);
        }

        [Conditional("DEBUG")]
        private static void TestProperties(INotifyPropertyChanged item, string[] properties)
        {
            foreach (string property in properties)
                TestProperty(item, property);
        }
        public static void RaiseAllPropertiesChanged(this INotifyPropertyChanged item)
        {
            item.RaisePropertyChanged("");
        }
        [Conditional("DEBUG")]
        private static void TestProperty(INotifyPropertyChanged item, string property)
        {
            if (string.IsNullOrEmpty(property))
                return;

            Type type = item.GetType();

            if (type.GetRuntimeProperty(property) == null)
                throw new PropertyNotFoundException(property);
        }
        /// <summary>
        /// Determines whether or not the given property is included in the property changed event
        /// </summary>
        /// <param name="e">Event args for the event</param>
        /// <param name="property">Property to check for</param>
        /// <returns>Whether or not the property is included as a change in the event</returns>
        public static bool IsProperty(this PropertyChangedEventArgs e, string property)
        {
            Exceptions.ThrowIfArgumentNull(e, nameof(e));
            Exceptions.ThrowIfArgumentNullOrEmpty(property, nameof(property));

            return string.IsNullOrEmpty(e.PropertyName) || e.PropertyName.Equals(property, StringComparison.Ordinal);
        }
        /// <summary>
        /// Determines whether any of the given properties have changed in the event
        /// </summary>
        /// <param name="e">Event args for the event</param>
        /// <param name="properties">Properties to check for</param>
        /// <returns>Whether or not any of the properties are included as a change in the event</returns>
        public static bool IsAnyProperty(this PropertyChangedEventArgs e, params string[] properties)
        {
            Exceptions.ThrowIfArgumentNull(e, nameof(e));
            Exceptions.ThrowIfArgumentNull(properties, nameof(properties));

            return string.IsNullOrEmpty(e.PropertyName) || properties.Contains(e.PropertyName, StringComparer.Ordinal);
        }
    }
}
