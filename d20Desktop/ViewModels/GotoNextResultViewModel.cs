using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for displaying <see cref="GotoNextResult"/>
    /// </summary>
    public sealed class GotoNextResultViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="GotoNextResultViewModel"/>
        /// </summary>
        /// <param name="result">Results of the goto next operation</param>
        public GotoNextResultViewModel(GotoNextResult result)
        {
            Result = result;
            ExpiredEffects = result
                .Combatants
                .SelectMany(p => p.ExpiredEffects, (c, e) => new ExpiredEffectViewModel(c.Combatant!, e))
                .ToArray();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the results of the operation
        /// </summary>
        public GotoNextResult Result { get; private set; }
        /// <summary>
        /// Gets a collection of effects that expired
        /// </summary>
        public ExpiredEffectViewModel[] ExpiredEffects { get; private set; }
        #endregion
    }
}
