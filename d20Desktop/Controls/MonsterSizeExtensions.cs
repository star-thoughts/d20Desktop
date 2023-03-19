using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiction.GameScreen.Monsters;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Extension methods for <see cref="MonsterSize"/>
    /// </summary>
    public static class MonsterSizeExtensions
    {
        /// <summary>
        /// Gets a display string for a given <see cref="MonsterSize"/>
        /// </summary>
        /// <param name="size">Size to get display string for</param>
        /// <returns>Display string for the given size</returns>
        public static string ToDisplayString(this MonsterSize size)
        {
            switch (size)
            {
                case MonsterSize.Unknown:
                    return Resources.Resources.UnknownLabel;
                case MonsterSize.Collosal:
                    return Resources.Resources.CollosalSize;
                case MonsterSize.Diminutive:
                    return Resources.Resources.DiminutiveSize;
                case MonsterSize.Fine:
                    return Resources.Resources.FineSize;
                case MonsterSize.Gargantuan:
                    return Resources.Resources.GargantuanSize;
                case MonsterSize.Huge:
                    return Resources.Resources.HugeSize;
                case MonsterSize.Large:
                    return Resources.Resources.LargeSize;
                case MonsterSize.Medium:
                    return Resources.Resources.MediumSize;
                case MonsterSize.Small:
                    return Resources.Resources.SmallSize;
                case MonsterSize.Tiny:
                    return Resources.Resources.TinySize;
                default:
                    throw new ArgumentException("Unknown size " + size.ToString(), nameof(size));
            }
        }

        private static IEnumerable? _itemsSource;
        /// <summary>
        /// Gets a collection usable for a selector
        /// </summary>
        public static IEnumerable? ItemsSource
        {
            get
            {
                if (_itemsSource == null)
                {
                    MonsterSize[] sizes = new MonsterSize[]
                    {
                        MonsterSize.Unknown, MonsterSize.Fine, MonsterSize.Diminutive, MonsterSize.Tiny, MonsterSize.Small, MonsterSize.Medium, MonsterSize.Large, MonsterSize.Huge, MonsterSize.Gargantuan, MonsterSize.Collosal,
                    };
                    _itemsSource = sizes
                        .Select(p => new { Display = p.ToDisplayString(), Value = p })
                        .ToArray();
                }
                return _itemsSource;
            }
        }
    }
}
