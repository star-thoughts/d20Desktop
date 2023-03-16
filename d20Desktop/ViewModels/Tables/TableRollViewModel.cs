using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels.Tables
{
    /// <summary>
    /// View model for rolling on a table
    /// </summary>
    public sealed class TableRollViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="TableRollViewModel"/>
        /// </summary>
        public TableRollViewModel()
        {
            _results = new ObservableCollection<TableResultViewModel>();
            Results = new ReadOnlyObservableCollection<TableResultViewModel>(_results);
        }
        #endregion
        #region Properties
        private readonly ObservableCollection<TableResultViewModel> _results;
        /// <summary>
        /// Gets the results of rolling on a table
        /// </summary>
        public ReadOnlyObservableCollection<TableResultViewModel> Results { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Rolls on a given table
        /// </summary>
        /// <param name="table">Table to roll on</param>
        /// <param name="times">Number of times to roll</param>
        public void Roll(TableViewModel table, int times = 1)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            if (!table.Entries.Any())
                throw new ArgumentException("Tables must have at least 1 entry to roll on.", nameof(table));
            if (times < 1)
                throw new ArgumentException("Must roll at least 1 time on a table.", nameof(times));

            foreach (TableResultViewModel result in InnerRoll(table, times))
                _results.Add(result);
        }

        private IEnumerable<TableResultViewModel> InnerRoll(TableViewModel table, int times)
        {
            for (int i = 0; i < times; i++)
            {
                int chancesTotal = table.Entries.Sum(p => p.EntrySize);

                int choice = Dice.Roll(1, chancesTotal);
                TableEntryViewModel entry = table.GetEntry(choice);
                yield return GetRolledEntry(table, entry);
            }
        }

        private TableResultViewModel GetRolledEntry(TableViewModel table, TableEntryViewModel entry)
        {
            TableResultViewModel[] otherEntries = Array.Empty<TableResultViewModel>();
            if (entry.OtherTable is OtherTableViewModel otherTable)
                otherEntries = InnerRoll(otherTable.Table, otherTable.NumberOfRolls).ToArray();

            return new TableResultViewModel(table, entry, otherEntries);
        }
        #endregion
    }
}
