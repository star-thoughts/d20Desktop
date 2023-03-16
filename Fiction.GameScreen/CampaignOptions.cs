using System.ComponentModel;

namespace Fiction.GameScreen
{
    public class CampaignOptions : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CampaignOptions"/>
        /// </summary>
        public CampaignOptions()
        {
            MonsterDeadAtOption = MonsterDeadAtDefaultOption.NegativeConstitution;
        }
        #endregion
        #region Properties
        private MonsterDeadAtDefaultOption _monsterDeadAtOption;
        /// <summary>
        /// Gets or sets the option to use for setting default dead at value for monsters
        /// </summary>
        public MonsterDeadAtDefaultOption MonsterDeadAtOption
        {
            get { return _monsterDeadAtOption; }
            set
            {
                if (_monsterDeadAtOption != value)
                {
                    _monsterDeadAtOption = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _monsterDefaultDeadAt;
        /// <summary>
        /// Gets or sets the default value to use if <see cref="MonsterDeadAtOption"/> is set to <see cref="MonsterDeadAtDefaultOption.SetValue"/>
        /// </summary>
        public int MonsterDefaultDeadAt
        {
            get { return _monsterDefaultDeadAt; }
            set
            {
                if (_monsterDefaultDeadAt != value)
                {
                    _monsterDefaultDeadAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}