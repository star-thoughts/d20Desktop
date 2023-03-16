using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fiction.Windows
{
	/// <summary>
	/// OkCancel bar
	/// </summary>
	[TemplatePart(Name = "PART_UpperBorder", Type = typeof(Border))]
	[TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
	public class OkCancel : Control
	{
		static OkCancel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OkCancel), new FrameworkPropertyMetadata(typeof(OkCancel)));
		}
	}
}
