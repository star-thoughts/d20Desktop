using System;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of types allowed to be selected from
    /// </summary>
    [Flags]
    public enum SpecialDefinitionType
    {
        /// <summary>
        /// Special qualities are allowed
        /// </summary>
        SpecialQuality = 1,
        /// <summary>
        /// Feats are allowed
        /// </summary>
        Feat = 2,
    }
}