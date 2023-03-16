using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for an object that contains attributes
    /// </summary>
    public interface IAttributeContainer
    {
        /// <summary>
        /// Gets the <see cref="AttributeManager"/> used to calculate attributes
        /// </summary>
        IAttributeManager AttributeManager { get; }
        /// <summary>
        /// Gets the attributes on this <see cref="IAttributeContainer"/>
        /// </summary>
        AttributeCollection Attributes { get; }
    }
}