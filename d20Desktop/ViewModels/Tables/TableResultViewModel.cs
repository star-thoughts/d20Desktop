using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels.Tables
{
    /// <summary>
    /// View model containing the results of a roll on a table
    /// </summary>
    public class TableResultViewModel
    {
        /// <summary>
        /// Constructs a new <see cref="TableResultViewModel"/>
        /// </summary>
        /// <param name="table">Table rolled on</param>
        /// <param name="entry">Entry rolled</param>
        /// <param name="otherEntries">Any extra rolls made due to the entry requiring it</param>
        public TableResultViewModel(TableViewModel table, TableEntryViewModel entry, IEnumerable<TableResultViewModel> otherEntries)
        {
            Table = table;
            Entry = entry;
            ExtraRolls = new ReadOnlyCollection<TableResultViewModel>(otherEntries.ToList());
        }
        /// <summary>
        /// Gets the table that was rolled on
        /// </summary>
        public TableViewModel Table { get; }
        /// <summary>
        /// Gets the entry that was rolled
        /// </summary>
        public TableEntryViewModel Entry { get; }
        /// <summary>
        /// Gets additional entries, when <see cref="Entry"/> specifies more rolls
        /// </summary>
        public ReadOnlyCollection<TableResultViewModel> ExtraRolls { get; }
    }
}