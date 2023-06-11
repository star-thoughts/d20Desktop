using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d20Web.Models.Combat
{
    public class CombatScenario
    {
        /// <summary>
        /// Gets the ID of this scenario
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of this combat scenario
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the grouping for this scenario
        /// </summary>
        public string? Group { get; set; }
        /// <summary>
        /// Gets or sets any extra details for this combat scenario
        /// </summary>
        public string? Details { get; set; }
    }
}
