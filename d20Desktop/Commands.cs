using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Commands used by the application
    /// </summary>
    public static class Commands
    {
        /// <summary>
        /// Command to create a new campaign
        /// </summary>
        public static RoutedCommand NewCampaign { get; private set; } = new RoutedCommand(nameof(NewCampaign), typeof(Commands));
        /// <summary>
        /// Command to open an existing campaign
        /// </summary>
        public static RoutedCommand OpenCampaign { get; } = new RoutedCommand(nameof(OpenCampaign), typeof(Commands));
        /// <summary>
        /// Command to save the current campaign
        /// </summary>
        public static RoutedCommand SaveCampaign { get; } = new RoutedCommand(nameof(SaveCampaign), typeof(Commands));
        /// <summary>
        /// Command to configure campaign attributes
        /// </summary>
        public static RoutedCommand CampaignAttributes { get; private set; } = new RoutedCommand(nameof(CampaignAttributes), typeof(Commands));
        /// <summary>
        /// Command to configure modifier types for the campaign
        /// </summary>
        public static RoutedCommand ModifierTypes { get; private set; } = new RoutedCommand(nameof(ModifierTypes), typeof(Commands));
        /// <summary>
        /// Command to configure races in the campaign
        /// </summary>
        public static RoutedCommand Races { get; private set; } = new RoutedCommand(nameof(Races), typeof(Commands));
        /// <summary>
        /// Command to configure campaign level modifiers
        /// </summary>
        public static RoutedCommand CampaignModifiers { get; private set; } = new RoutedCommand(nameof(CampaignModifiers), typeof(Commands));
        /// <summary>
        /// Command for add
        /// </summary>
        public static RoutedCommand Add { get; private set; } = new RoutedCommand(nameof(Add), typeof(Commands));
        /// <summary>
        /// Command for remove
        /// </summary>
        public static RoutedCommand Remove { get; private set; } = new RoutedCommand(nameof(Remove), typeof(Commands));
        /// <summary>
        /// Command for edit
        /// </summary>
        public static RoutedCommand Edit { get; private set; } = new RoutedCommand(nameof(Edit), typeof(Commands));
        /// <summary>
        /// Command to manage combat scenarios in the campaign
        /// </summary>
        public static RoutedCommand ManageCombatScenarios { get; } = new RoutedCommand(nameof(ManageCombatScenarios), typeof(Commands));
        /// <summary>
        /// Command to begin combat
        /// </summary>
        public static RoutedCommand BeginCombat { get; } = new RoutedCommand(nameof(BeginCombat), typeof(Commands));
        /// <summary>
        /// Command to load up the active combat
        /// </summary>
        public static RoutedCommand ActiveCombat { get; } = new RoutedCommand(nameof(ActiveCombat), typeof(Commands));
        /// <summary>
        /// Command to close a tab
        /// </summary>
        public static RoutedCommand CloseTab { get; } = new RoutedCommand(nameof(CloseTab), typeof(Commands));
        /// <summary>
        /// Command to move something up
        /// </summary>
        public static RoutedCommand Up { get; } = new RoutedCommand(nameof(Up), typeof(Commands));
        /// <summary>
        /// Command to move something down
        /// </summary>
        public static RoutedCommand Down { get; } = new RoutedCommand(nameof(Down), typeof(Commands));
        /// <summary>
        /// Command to roll something
        /// </summary>
        public static RoutedCommand Roll { get; } = new RoutedCommand(nameof(Roll), typeof(Commands));
        /// <summary>
        /// Command to reset the current view
        /// </summary>
        public static RoutedCommand Reset { get; } = new RoutedCommand(nameof(Reset), typeof(Commands));
        /// <summary>
        /// Command to end an active combat
        /// </summary>
        public static RoutedCommand EndCombat { get; } = new RoutedCommand(nameof(EndCombat), typeof(Commands));
        /// <summary>
        /// Command to move to the next combatant in combat
        /// </summary>
        public static RoutedCommand NextCombatant { get; } = new RoutedCommand(nameof(NextCombatant), typeof(Commands));
        /// <summary>
        /// Command to deal damage to combatants in combat
        /// </summary>
        public static RoutedCommand DamageCombatants { get; } = new RoutedCommand(nameof(DamageCombatants), typeof(Commands));
        /// <summary>
        /// Command to heal damage to combatants in combat
        /// </summary>
        public static RoutedCommand HealCombatants { get; } = new RoutedCommand(nameof(HealCombatants), typeof(Commands));
        /// <summary>
        /// Command to manage combat and combatants
        /// </summary>
        public static RoutedCommand ManageCombatants { get; } = new RoutedCommand(nameof(ManageCombatants), typeof(Commands));
        /// <summary>
        /// Command to move the selected combatant to before the current combatant
        /// </summary>
        public static RoutedCommand MoveBefore { get; } = new RoutedCommand(nameof(MoveBefore), typeof(Commands));
        /// <summary>
        /// Command to move the selected combatant to after the current combatant
        /// </summary>
        public static RoutedCommand MoveAfter { get; } = new RoutedCommand(nameof(MoveAfter), typeof(Commands));
        /// <summary>
        /// Command to manage monsters in a campaign
        /// </summary>
        public static RoutedCommand ManageMonsters { get; } = new RoutedCommand(nameof(ManageMonsters), typeof(Commands));
        /// <summary>
        /// Command to create a copy of something
        /// </summary>
        public static RoutedCommand CreateCopy { get; } = new RoutedCommand(nameof(CreateCopy), typeof(Commands));
        /// <summary>
        /// Command to go to the next page in a wizard
        /// </summary>
        public static RoutedCommand NextPage { get; } = new RoutedCommand(nameof(NextPage), typeof(Commands));
        /// <summary>
        /// Command to go to the previous page in a wizard
        /// </summary>
        public static RoutedCommand PrevPage { get; } = new RoutedCommand(nameof(PrevPage), typeof(Commands));
        /// <summary>
        /// Command to complete the wizard
        /// </summary>
        public static RoutedCommand Finish { get; } = new RoutedCommand(nameof(Finish), typeof(Commands));
        /// <summary>
        /// Command to cancel the wizard
        /// </summary>
        public static RoutedCommand CancelWizard { get; } = new RoutedCommand(nameof(CancelWizard), typeof(Commands));
        /// <summary>
        /// Command to choose a source for something
        /// </summary>
        public static RoutedCommand ChooseSource { get; } = new RoutedCommand(nameof(ChooseSource), typeof(Commands));
        /// <summary>
        /// Command to view an object in a new window
        /// </summary>
        public static RoutedCommand ViewObject { get; } = new RoutedCommand(nameof(ViewObject), typeof(Commands));
        /// <summary>
        /// Command to manage players in the campaign
        /// </summary>
        public static RoutedCommand ManagePlayers { get; } = new RoutedCommand(nameof(ManagePlayers), typeof(Commands));
        /// <summary>
        /// Command to undo previous operations
        /// </summary>
        public static RoutedCommand Undo { get; } = new RoutedCommand(nameof(Undo), typeof(Commands));
        /// <summary>
        /// Command to manage monster types and subtypes
        /// </summary>
        public static RoutedCommand ManageTypes { get; } = new RoutedCommand(nameof(ManageTypes), typeof(Commands));
        /// <summary>
        /// Command to manage spells in the campaign
        /// </summary>
        public static RoutedCommand ManageSpells { get; } = new RoutedCommand(nameof(ManageSpells), typeof(Commands));
        /// <summary>
        /// Command to manage spell schools, subschools and effect types
        /// </summary>
        public static RoutedCommand ManageSpellSchools { get; } = new RoutedCommand(nameof(ManageSpellSchools), typeof(Commands));
        /// <summary>
        /// Command to manage the sources in the campaign
        /// </summary>
        public static RoutedCommand ManageSources { get; } = new RoutedCommand(nameof(ManageSources), typeof(Commands));
        /// <summary>
        /// Command to create an effect, the parameter is the source of the effect
        /// </summary>
        public static RoutedCommand CreateEffect { get; } = new RoutedCommand(nameof(CreateEffect), typeof(Commands));
        /// <summary>
        /// Command to view magic items
        /// </summary>
        public static RoutedCommand MagicItems { get; } = new RoutedCommand(nameof(MagicItems), typeof(Commands));
        /// <summary>
        /// Command to remove a target
        /// </summary>
        public static RoutedCommand RemoveTarget { get; } = new RoutedCommand(nameof(RemoveTarget), typeof(Commands));
        /// <summary>
        /// Command to add an advanced filter
        /// </summary>
        public static RoutedCommand Filter { get; } = new RoutedCommand(nameof(Filter), typeof(Commands));
        /// <summary>
        /// Command to manage conditions in a campaign
        /// </summary>
        public static RoutedCommand ManageConditions { get; } = new RoutedCommand(nameof(ManageConditions), typeof(Commands));
        /// <summary>
        /// Command to add a condition to a combatant
        /// </summary>
        public static RoutedCommand AddCondition { get; } = new RoutedCommand(nameof(AddCondition), typeof(Commands));
        /// <summary>
        /// Command to remove a condition from a combatant
        /// </summary>
        public static RoutedCommand RemoveCondition { get; } = new RoutedCommand(nameof(RemoveCondition), typeof(Commands));
        /// <summary>
        /// Command to kill a combatant in combat, then remove them from the combat
        /// </summary>
        public static RoutedCommand KillCombatant { get; } = new RoutedCommand(nameof(KillCombatant), typeof(Commands));
    }
}
