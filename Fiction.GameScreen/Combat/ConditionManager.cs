using System.Collections.ObjectModel;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Manages conditions in a campaign
    /// </summary>
    public class ConditionManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ConditionManager"/>
        /// </summary>
        public ConditionManager()
        {
            Conditions = new ObservableCollection<Condition>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of conditions
        /// </summary>
        public ObservableCollection<Condition> Conditions { get; private set; }
        #endregion
    }
}