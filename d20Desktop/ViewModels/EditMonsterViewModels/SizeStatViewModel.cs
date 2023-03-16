using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.EditMonsterViewModels
{
    /// <summary>
    /// View model for a size-based stat
    /// </summary>
    public sealed class SizeStatViewModel : MonsterStatViewModel<MonsterSize>
    {
        /// <summary>
        /// Constructs a new <see cref="SizeStatViewModel"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="category">Category for the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        public SizeStatViewModel(string name, string category, Monster monster, string statName)
            : base(name, category, monster, statName)
        {
        }
        protected override MonsterSize CreateDefaultValue()
        {
            return MonsterSize.Unknown;
        }
    }
}
