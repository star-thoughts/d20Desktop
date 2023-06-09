﻿using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Creates a monster view model out of a monster
    /// </summary>
    public sealed class MonsterToViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Monster monster)
                return new EditMonsterViewModel(monster, null);
            else if (value is EditMonsterViewModel vm)
                return vm;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
