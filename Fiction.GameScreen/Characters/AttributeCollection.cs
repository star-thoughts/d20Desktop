using Fiction.GameScreen.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains a collection of calculated attributes
    /// </summary>
    public sealed class AttributeCollection : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeCollection"/>
        /// </summary>
        public AttributeCollection(ICampaignSerializer serializer)
        {
            _attributes = new Dictionary<object, CalculatedAttribute>();
            _semaphore = new SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Constructs a new <see cref="AttributeCollection"/>
        /// </summary>
        /// <param name="attributes">Attributes already calculated</param>
        public AttributeCollection(ICampaignSerializer serializer, IEnumerable<CalculatedAttribute> attributes)
        {
            _attributes = attributes.ToDictionary(p => p.Definition.Id, p => p);
            _semaphore = new SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Constructs a new <see cref="AttributeCollection"/>
        /// </summary>
        /// <param name="attributes">Attributes already calculated</param>
        public AttributeCollection(ICampaignSerializer serializer, params CalculatedAttribute[] attributes)
            : this(serializer, (IEnumerable<CalculatedAttribute>)attributes)
        {
        }
        #endregion
        #region Member Variables
        private Dictionary<object, CalculatedAttribute> _attributes;
        private SemaphoreSlim _semaphore;
        #endregion
        #region Properties
        public CalculatedAttribute this[AttributeDefinition definition]
        {
            get
            {
                return GetAttribute(definition);
            }
            set
            {
                SetAttributeValue(value);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Gets all attributes currently calculated
        /// </summary>
        /// <returns>Collection of all calculated attributes</returns>
        public async Task<CalculatedAttribute[]> GetAttributesAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                CalculatedAttribute[] attributes = _attributes.Values.ToArray();
                return attributes;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        /// <summary>
        /// Gets all attributes currently calculated
        /// </summary>
        /// <returns>Collection of all calculated attributes</returns>
        public CalculatedAttribute[] GetAttributes()
        {
            _semaphore.Wait();
            try
            {
                CalculatedAttribute[] attributes = _attributes.Values.ToArray();
                return attributes;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        /// <summary>
        /// Gets a <see cref="CalculatedAttribute"/> for a given definition
        /// </summary>
        /// <param name="definition">Definition to get a <see cref="CalculatedAttribute"/> for</param>
        /// <returns><see cref="CalculatedAttribute"/> for the given definition, or null if the attribute has not been calculated</returns>
        public CalculatedAttribute GetAttribute(AttributeDefinition definition)
        {
            Exceptions.ThrowIfArgumentNull(definition, nameof(definition));

            _semaphore.Wait();
            try
            {
                if (_attributes.TryGetValue(definition.Id, out CalculatedAttribute attribute))
                    return attribute;
            }
            finally
            {
                _semaphore.Release();
            }

            return null;
        }
        /// <summary>
        /// Gets a <see cref="CalculatedAttribute"/> for a given definition
        /// </summary>
        /// <param name="definition">Definition to get a <see cref="CalculatedAttribute"/> for</param>
        /// <returns><see cref="CalculatedAttribute"/> for the given definition, or null if the attribute has not been calculated</returns>
        public async Task<CalculatedAttribute> GetAttributeAsync(AttributeDefinition definition)
        {
            Exceptions.ThrowIfArgumentNull(definition, nameof(definition));

            await _semaphore.WaitAsync();
            try
            {
                if (_attributes.TryGetValue(definition.Id, out CalculatedAttribute attribute))
                    return attribute;
            }
            finally
            {
                _semaphore.Release();
            }

            return null;
        }
        /// <summary>
        /// Sets an attribute's <see cref="CalculatedAttribute"/>
        /// </summary>
        /// <param name="value">Value to set the attribute to</param>
        public void SetAttributeValue(CalculatedAttribute value)
        {
            Exceptions.ThrowIfArgumentNull(value, nameof(value));

            AttributeDefinition definition = value.Definition;

            _semaphore.Wait();
            try
            {
                ThrowIfCantSetAttribute(value, definition);
                _attributes[definition.Id] = value;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        /// <summary>
        /// Sets an attribute's <see cref="CalculatedAttribute"/>
        /// </summary>
        /// <param name="value">Value to set the attribute to</param>
        public async Task SetAttributeValueAsync(CalculatedAttribute value)
        {
            Exceptions.ThrowIfArgumentNull(value, nameof(value));

            AttributeDefinition definition = value.Definition;

            await _semaphore.WaitAsync();
            try
            {
                ThrowIfCantSetAttribute(value, definition);
                _attributes[definition.Id] = value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void ThrowIfCantSetAttribute(CalculatedAttribute value, AttributeDefinition definition)
        {
            if (_attributes.TryGetValue(definition.Id, out CalculatedAttribute att))
            {
                //  Once an attribute is in the system, don't allow it to be re-added
                throw new AttributeStateException("Cannot add the same attribute multiple times.");
            }
        }

        /// <summary>
        /// Resets the attributes so they can be calculated again
        /// </summary>
        public void Clear()
        {
            _semaphore.Wait();
            try
            {
                _attributes.Clear();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Resets the attributes so they can be calculated again
        /// </summary>
        public async Task ClearAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                _attributes.Clear();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Disposes of this object
        /// </summary>
        public void Dispose()
        {
            _semaphore?.Dispose();
            _semaphore = null;
        }
        #endregion
    }
}
