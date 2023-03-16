using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiction.Windows
{
	/// <summary>
	/// Data types allowed for filtering keystrokes in a text box
	/// </summary>
	public enum TextBoxDataType
	{
		/// <summary>
		/// Text box is unformatted
		/// </summary>
		Unknown,
		/// <summary>
		/// Text box should only accept integers
		/// </summary>
		Integer,
		/// <summary>
		/// Text box should only accept a non-negative integer
		/// </summary>
		NonNegativeInteger,
		/// <summary>
		/// Text box should only accept a currency value
		/// </summary>
		Currency,
		/// <summary>
		/// Text box should only accept a float value
		/// </summary>
		Float,
	}
}
