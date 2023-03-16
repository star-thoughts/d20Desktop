using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Fiction.GameScreen.Monsters;
using System.Globalization;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information about damage reduction
    /// </summary>
    public class DamageReduction : INotifyPropertyChanged, IDamageReduction
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="DamageReduction"/>
        /// </summary>
        /// <param name="amount">Amount of damage reduction to apply</param>
        /// <param name="all">Whether or not all types must be present to overcome damage reduction</param>
        /// <param name="types">Types of attacks that overcome damage reduction</param>
        public DamageReduction(int amount, bool all, IEnumerable<string> types)
        {
            Amount = amount;
            RequiresAllTypes = all;
            Types = types.ToObservableCollection();
        }
        #endregion
        #region Properties
        private int _amount;
        /// <summary>
        /// Gets or sets the amount of damage reduction to apply
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of types that bypass damage reduction
        /// </summary>
        public ObservableCollection<string> Types { get; }
        private bool _requiresAllTypes;
        /// <summary>
        /// Gets or sets whether or not the attack must possess all types listed in order to bypass damage reduction
        /// </summary>
        public bool RequiresAllTypes
        {
            get { return _requiresAllTypes; }
            set
            {
                if (_requiresAllTypes != value)
                {
                    _requiresAllTypes = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Applies this damage reduction to an amount of damage
        /// </summary>
        /// <param name="amount">Amount of damage to apply to</param>
        /// <param name="types">Types of damage being dealt</param>
        /// <returns></returns>
        public int Apply(int amount, params string[] types)
        {
            int result = amount;
            if (RequiresAllTypes)
            {
                if (!Types.All(p => types.Contains(p, StringComparer.CurrentCultureIgnoreCase)))
                    result = amount - Amount;
            }
            else
            {
                if (!Types.Any(p => types.Contains(p, StringComparer.CurrentCultureIgnoreCase)))
                    result = amount - Amount;
            }
            return result;
        }
        /// <summary>
        /// Determines whether or not this damage reduction should apply
        /// </summary>
        /// <param name="types">Damage types</param>
        /// <returns>Whether or not this damage reduction applies</returns>
        public bool DoesApply(params string[] types)
        {
            if (RequiresAllTypes)
            {
                if (!Types.All(p => types.Contains(p, StringComparer.CurrentCultureIgnoreCase)))
                    return true;
            }
            else
            {
                if (!Types.Any(p => types.Contains(p, StringComparer.CurrentCultureIgnoreCase)))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Parses the fields of a source and determines what damage reduction can be found
        /// </summary>
        /// <param name="source">Source combatant to parse</param>
        /// <param name="statNames">Names of statistics to parse</param>
        /// <returns>Collection of damage reduction objects</returns>
        public static IEnumerable<DamageReduction> Parse(ICombatantTemplateSource source, params string[] statNames)
        {
            if (source is Monster monster)
            {
                foreach (string stat in statNames)
                {
                    if (monster.Stats[stat]?.Value is string value)
                    {
                        foreach (DamageReduction dr in Parse(value))
                            yield return dr;
                    }
                }
            }
        }

        /// <summary>
        /// Parses a string and finds any damage reduction qualities
        /// </summary>
        /// <param name="qualityString">String to parse</param>
        /// <returns>Collection of damage reduction objects</returns>
        public static IEnumerable<DamageReduction> Parse(string qualityString)
        {
            string[] parts = qualityString.Split(StringSplitOptions.RemoveEmptyEntries, ';', ',');
            Regex regex = new Regex(Resources.Resources.DamageReductionMask, RegexOptions.IgnoreCase);
            string[] typeSplitters = Resources.Resources.DamageReductionTypeSplitter.Split(StringSplitOptions.RemoveEmptyEntries, ';');

            foreach (string possibility in parts)
            {
                Match match = regex.Match(possibility);
                if (match.Success
                    && int.TryParse(match.Groups["amount"].Value, out int amount))
                {
                    string typeString = match.Groups["types"].Value.Trim();

                    if (typeString.Equals("-"))
                    {
                        yield return new DamageReduction(amount, false, Array.Empty<string>());
                    }
                    else
                    {
                        bool all = typeString.ToUpperInvariant().Contains(Resources.Resources.DamageReductionAll);

                        string[] types = typeString.Split(StringSplitOptions.RemoveEmptyEntries, typeSplitters)
                            .Select(p => p.Trim())
                            .ToArray();

                        yield return new DamageReduction(amount, all, types);
                    }
                }
            }
        }

        /// <summary>
        /// Converts this object to a string representation
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            if (Types.Any())
            {
                string separator = RequiresAllTypes ? " and " : " or ";
                return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Amount, string.Join(separator, Types.ToArray()));
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}/-", Amount);
            }
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}