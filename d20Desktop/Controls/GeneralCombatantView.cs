using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using Fiction.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for the general viewing of a combatant
    /// </summary>
    public sealed class GeneralCombatantView : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="GeneralCombatantView"/> class
        /// </summary>
        static GeneralCombatantView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeneralCombatantView), new FrameworkPropertyMetadata(typeof(GeneralCombatantView)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combatant
        /// </summary>
        public ICombatant Combatant
        {
            get { return (ICombatant)GetValue(CombatantProperty); }
            set { SetValue(CombatantProperty, value);}
        }
        /// <summary>
        /// Gets or sets the combat
        /// </summary>
        public Combat.ActiveCombat Combat
        {
            get { return (Combat.ActiveCombat)GetValue(CombatProperty); }
            set { SetValue(CombatProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Combatant"/>
        /// </summary>
        public static readonly DependencyProperty CombatantProperty = DependencyProperty.Register(nameof(Combatant), typeof(ICombatant), typeof(GeneralCombatantView));
        /// <summary>
        /// DependencyProperty for <see cref="Combatant"/>
        /// </summary>
        public static readonly DependencyProperty CombatProperty = DependencyProperty.Register(nameof(Combat), typeof(Combat.ActiveCombat), typeof(GeneralCombatantView));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            ListBox? listBox = Template.FindName("PART_SourceList", this) as ListBox;
            Style? style = listBox?.GetOrCreateItemContainerStyle<ListBoxItem>();
            if (style != null)
                style.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(Effect_DoubleClick)));

            listBox = Template.FindName("PART_TargetList", this) as ListBox;
            style = listBox?.GetOrCreateItemContainerStyle<ListBoxItem>();
            if (style != null)
                style.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(Effect_DoubleClick)));
        }

        private void Effect_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is ListBoxItem lbi && lbi.DataContext is Effect effect)
                {
                    ObjectViewerWindow window = new ObjectViewerWindow(effect);
                    window.Owner = Window.GetWindow(this);
                    window.ShowDialog();
                }
            });
        }
        #endregion
    }
}
