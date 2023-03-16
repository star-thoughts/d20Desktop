using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.EditMonsterViewModels
{
    /// <summary>
    /// View model for an alignment based stat
    /// </summary>
    public sealed class AlignmentStatViewModel : MonsterStatViewModel<Alignment>
    {
        /// <summary>
        /// Constructs a new <see cref="AlignmentStatViewModel"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="category">Category for the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        public AlignmentStatViewModel(string name, string category, Monster monster, string statName)
            : base(name, category, monster, statName)
        {
        }
        protected override Alignment CreateDefaultValue()
        {
            return Alignment.Unknown;
        }
    }
}
