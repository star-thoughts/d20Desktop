using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Class used to represent no item in a list or combobox, so the user can effectively
    /// deselect an item in the list.  The <see cref="Selector.SelectedValue"/> or <see cref="Selector.SelectedItem"/>
    /// should use a <see cref="CampaignObjectConverter"/> to interpret the value.
    /// </summary>
    public sealed class NoCampaignObject : ICampaignObject
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="NoCampaignObject"/>
        /// </summary>
        public NoCampaignObject()
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a static object to represent an unselected object
        /// </summary>
        public static NoCampaignObject Value { get; } = new NoCampaignObject();
        /// <summary>
        /// Gets the display name of the item
        /// </summary>
        public string Name => Resources.Resources.NoneLabel;
        /// <summary>
        /// Gets the campaign.  Since it's not in a campaign, this is always null.
        /// </summary>
        public CampaignSettings Campaign => null;
        /// <summary>
        /// Gets the ID of this object
        /// </summary>
        public int Id => 0;
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether this object is equivalent to <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Whether or not the two objects are equivalent</returns>
        public override bool Equals(object obj)
        {
            return obj is NoCampaignObject;
        }
        /// <summary>
        /// Gets a hash code associated with this object
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Display name of the object
        /// </summary>
        /// <returns>Display name string</returns>
        public override string ToString()
        {
            return Resources.Resources.NoneLabel;
        }
        #endregion
    }
}
