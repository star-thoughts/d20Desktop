using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fiction.Windows
{
    public static class TextBoxHelper
    {
        #region Constructors
        static TextBoxHelper()
        {
            DataTypeProperty = DependencyProperty.RegisterAttached("DataType", typeof(TextBoxDataType), typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(TextBoxDataType.Unknown, DataTypeChanged));
            AllowableCharactersProperty = DependencyProperty.RegisterAttached("AllowableCharacters", typeof(string), typeof(TextBoxHelper),
                new FrameworkPropertyMetadata("", AllowableCharactersChanged));
            RegexProperty = DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(TextBoxHelper),
                new FrameworkPropertyMetadata("", RegexChanged));
        }
        #endregion
        #region Support Methods
        private static string GetEditedText(System.Windows.Input.TextCompositionEventArgs e, TextBox textBox)
        {
            string text = textBox.Text;
            if (textBox.SelectionLength > 0)
                text = text.Substring(0, textBox.SelectionStart) + text.Substring(textBox.SelectionStart + textBox.SelectionLength);
            text = text.Insert(textBox.CaretIndex, e.Text);
            return text;
        }
        #endregion
        #region DataType
        public static readonly DependencyProperty DataTypeProperty;

        /// <summary>
        /// Gets the type of data to be entered into a textbox
        /// </summary>
        /// <param name="textBox">Textbox to test</param>
        /// <returns>Type of data to be entered into a TextBox</returns>
        public static TextBoxDataType GetDataType(TextBox textBox)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            return (TextBoxDataType)textBox.GetValue(DataTypeProperty);
        }
        /// <summary>
        /// Sets the type of data to be entered into a TextBox
        /// </summary>
        /// <param name="textBox">Textbox to set</param>
        /// <param name="value">Type of data to be entered into the TextBox</param>
        public static void SetDataType(TextBox textBox, TextBoxDataType value)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            textBox.SetValue(DataTypeProperty, value);
        }

        private static void DataTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox? textBox = o as TextBox;

            if (!(e.NewValue is TextBoxDataType))
                throw new InvalidOperationException("NewValue must be a TextBoxDataType for IsNumeric");

            if (textBox != null)
            {
                TextBoxDataType type = (TextBoxDataType)e.NewValue;
                if (type != TextBoxDataType.Unknown)
                    textBox.PreviewTextInput += TextBoxDataType_PreviewTextInput;
                else
                    textBox.PreviewTextInput -= TextBoxDataType_PreviewTextInput;
            }
        }

        static void TextBoxDataType_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            int output = 0;
            float outfloat = 0.0F;

            TextBox? textBox = sender as TextBox;
            if (sender != null && textBox != null)
            {
                TextBoxDataType type = GetDataType(textBox);
                //  Get what the text is going to be after the edit
                string text = GetEditedText(e, textBox);
                //  See if it is an int
                if (type == TextBoxDataType.Integer)
                {
                    e.Handled = !string.IsNullOrEmpty(text) && (text != "-") && !int.TryParse(text, out output);
                }
                else if (type == TextBoxDataType.NonNegativeInteger)
                {
                    e.Handled = !string.IsNullOrEmpty(text) && !int.TryParse(text, out output);
                    if (output < 0)
                        e.Handled = true;
                }
                else if (type == TextBoxDataType.Float)
                {
                    e.Handled = !string.IsNullOrEmpty(text) && (text != "-") && !float.TryParse(text, out outfloat);
                }
            }
        }
        #endregion
        #region AllowableCharacters
        public static readonly DependencyProperty AllowableCharactersProperty;

        /// <summary>
        /// Gets the allowable characters for the textbox
        /// </summary>
        /// <param name="textBox">Textbox to limit</param>
        /// <returns>Allowable characters in the textbox</returns>
        public static string GetAllowableCharacters(TextBox textBox)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            return (string)textBox.GetValue(AllowableCharactersProperty);
        }
        /// <summary>
        /// Sets the allowable characters for the textbox
        /// </summary>
        /// <param name="textBox">Textbox to limit</param>
        /// <param name="value">Allowable characters in the textbox</param>
        public static void SetAllowableCharacters(TextBox textBox, string value)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            textBox.SetValue(AllowableCharactersProperty, value);
        }

        private static void AllowableCharactersChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox? textBox = o as TextBox;

            Debug.Assert(e.NewValue is string, "NewValue must be a string for AllowableCharacters");

            if (textBox != null)
            {
                string value = (string)e.NewValue;
                if (!string.IsNullOrEmpty(value))
                    textBox.PreviewTextInput += AllowableCharacters_PreviewTextInput;
                else
                    textBox.PreviewTextInput -= AllowableCharacters_PreviewTextInput;
            }
        }

        static void AllowableCharacters_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                string allowed = GetAllowableCharacters(textBox);
                string text = GetEditedText(e, textBox);
                e.Handled = text.Any(p => !allowed.Contains(p));
            }
        }
        #endregion
        #region Regex
        public static readonly DependencyProperty RegexProperty;

        public static string GetRegex(TextBox textBox)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            return (string)textBox.GetValue(RegexProperty);
        }
        public static void SetRegex(TextBox textBox, string value)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            textBox.SetValue(RegexProperty, value);
        }

        private static void RegexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox? textBox = o as TextBox;

            if (textBox != null)
            {
                string? old = e.OldValue as string;
                string? regex = e.NewValue as string;

                if (textBox.IsLoaded)
                    SetRegexValidation(textBox, old, regex);
                else
                    textBox.Loaded += TextBox_SetRegexOnLoad;
            }
        }

        private static void TextBox_SetRegexOnLoad(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
                SetRegexValidation(textBox, "", GetRegex(textBox));
        }

        private static void SetRegexValidation(TextBox textBox, string? old, string? regex)
        {
            Binding binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
            if (binding != null)
            {
                //  Remove the old one if necessary
                if (!string.IsNullOrEmpty(old))
                    binding.ValidationRules.Remove(binding.ValidationRules.OfType<RegexValidationRule>().First());
                //  Add the new one if necessary
                if (!string.IsNullOrEmpty(regex))
                    binding.ValidationRules.Add(new RegexValidationRule() { Regex = regex });
            }
        }
        #endregion
        #region Required
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(false, IsRequiredPropertyChanged));

        public static bool GetIsRequired(TextBox textBox)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            return (bool)textBox.GetValue(IsRequiredProperty);
        }

        public static void SetIsRequired(TextBox textBox, bool value)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));
            textBox.SetValue(IsRequiredProperty, value);
        }

        private static void IsRequiredPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox? textBox = o as TextBox;
            if (textBox != null)
            {
                if (textBox.IsLoaded)
                    SetIsRequiredValidation(textBox, (bool)e.OldValue, (bool)e.NewValue);
                else
                    textBox.Loaded += TextBox_SetIsRequiredOnLoad;
            }
        }

        private static void SetIsRequiredValidation(TextBox textBox, bool oldValue, bool newValue)
        {
            Binding binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
            if (newValue)
                binding.ValidationRules.Add(new RequiredValidationRule());
            else if (binding.ValidationRules.OfType<RequiredValidationRule>().Count() > 0)
                binding.ValidationRules.Remove(binding.ValidationRules.OfType<RequiredValidationRule>().First());
        }

        static void TextBox_SetIsRequiredOnLoad(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
                SetIsRequiredValidation(textBox, false, GetIsRequired(textBox));
        }
        #endregion
    }
}
