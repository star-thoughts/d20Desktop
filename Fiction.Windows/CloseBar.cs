using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.Windows
{
    public class CloseBar : Control
    {
		static CloseBar()
		{
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseBar), new FrameworkPropertyMetadata(typeof(CloseBar)));
		}
    }
}
