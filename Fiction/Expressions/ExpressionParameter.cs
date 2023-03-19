using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Class that holds information about a parameter for an expression call
	/// </summary>
	public class ExpressionParameter
	{
		#region Properties
		/// <summary>
		/// Gets or sets the name of the parameter
		/// </summary>
		/// <remarks>
		/// Must be a valid parameter name for the given language
		/// </remarks>
		public string? Name { get; set; }
		/// <summary>
		/// Gets or sets the type of the parameter
		/// </summary>
		public Type? ParameterType { get; set; }
		#endregion
	}
}
