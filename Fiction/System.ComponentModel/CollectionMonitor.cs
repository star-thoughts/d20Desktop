using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public sealed class CollectionMonitor : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Constructors
        /// <summary>
        /// Monitors a collection for changes to it's members
        /// </summary>
        /// <param name="collection">Collection to monitor</param>
        public CollectionMonitor(INotifyCollectionChanged collection)
        {
            _collection = collection;
            _collection.CollectionChanged += _collection_CollectionChanged;
            AttachItems(collection as IEnumerable);
        }
        #endregion
        #region Member Variables
        private INotifyCollectionChanged _collection;
        #endregion
        #region Methods
        private void _collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                AttachItems(e.NewItems);
            }
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged prop)
                        prop.PropertyChanged -= Member_PropertyChanged;
                }
            }
            CollectionChanged?.Invoke(sender, e);
        }

        private void AttachItems(IEnumerable newItems)
        {
            foreach (object item in newItems)
            {
                if (item is INotifyPropertyChanged prop)
                    prop.PropertyChanged += Member_PropertyChanged;
            }
        }

        private void Member_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //  Re-raise it so the monitor shows that the item changed
            PropertyChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Detaches from the collection and all of its members
        /// </summary>
        public void Detach()
        {
            foreach (object item in (IEnumerable)_collection)
            {
                if (item is INotifyPropertyChanged prop)
                    prop.PropertyChanged -= Member_PropertyChanged;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion
    }
}
