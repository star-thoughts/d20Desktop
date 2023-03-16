using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing monster types and subtypes in a campaign
    /// </summary>
    public sealed class ManageMonsterTypes : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManageMonsterTypes"/> class
        /// </summary>
        static ManageMonsterTypes()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageMonsterTypes), new FrameworkPropertyMetadata(typeof(ManageMonsterTypes)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for this control
        /// </summary>
        public ManageMonsterTypesViewModel ViewModel
        {
            get { return (ManageMonsterTypesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageMonsterTypesViewModel), typeof(ManageMonsterTypes));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            StringListEditor editor = Template.FindName("PART_Types", this) as StringListEditor;
            if (editor != null)
            editor.StringEdited += TypesEditor_StringEdited;

            editor = Template.FindName("PART_SubTypes", this) as StringListEditor;
            if (editor != null)
                editor.StringEdited += SubTypes_StringEdited;
        }

        private void SubTypes_StringEdited(object sender, StringEditedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.EventType == StringEditEventType.Removed)
                    ViewModel.Factory.Campaign.MonsterManager.RemoveSubType(e.String);
            });
        }

        private void TypesEditor_StringEdited(object sender, StringEditedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.EventType == StringEditEventType.Removed)
                    ViewModel.Factory.Campaign.MonsterManager.RemoveType(e.String);
            });
        }
        #endregion
    }
}
