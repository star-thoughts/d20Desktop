namespace Fiction.GameScreen.Characters
{
    public sealed class ClassLevelDefinition : Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ClassLevelDefinition"/>
        /// </summary>
        /// <param name="name">Name of the class level</param>
        /// <param name="modifiers">Modifiers associated with this class level</param>
        public ClassLevelDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
        }
        #endregion
    }
}