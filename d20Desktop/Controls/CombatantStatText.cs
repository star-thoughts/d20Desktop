using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for displaying the text of a combatant's or monster's statistic
    /// </summary>
    public class CombatantStatText : Control
    {
        /// <summary>
        /// Initializes the <see cref="CombatantStatText"/> class
        /// </summary>
        static CombatantStatText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombatantStatText), new FrameworkPropertyMetadata(typeof(CombatantStatText)));
        }

        /// <summary>
        /// Gets or sets the text of the statistic
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(CombatantStatText),
            new FrameworkPropertyMetadata(null, TextChanged));

        /// <summary>
        /// Gets the parts to display
        /// </summary>
        public TextBlock Parts
        {
            get { return (TextBlock)GetValue(PartsProperty); }
            set { SetValue(PartsProperty, value); }
        }

        public static readonly DependencyProperty PartsProperty = DependencyProperty.Register(nameof(Parts), typeof(TextBlock), typeof(CombatantStatText));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CombatantStatText view)
            {
                view.UpdateText(e.NewValue as string);
            }
        }

        private void UpdateText(string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
            };
            string prefix = "";

            foreach (string part in GetLines(text))
            {
                Run run = new Run();
                if (part.StartsWith("# "))
                {
                    run.Text = prefix + part.TrimStart("# ");
                    run.FontWeight = FontWeights.Bold;
                }
                else
                    run.Text = prefix + part;

                textBlock.Inlines.Add(run);

                prefix = Environment.NewLine;
            }

            Parts = textBlock;
        }

        private IEnumerable<string> GetLines(string text)
        {
            StringBuilder builder = new StringBuilder(text);
            builder.Replace("\r\n", "\r").Replace("\r", "\n");

            return builder.ToString().Split('\n');
        }
    }
}
