using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Class that represents an expression with two parameters
	/// </summary>
	/// <typeparam name="TResult">Result type of the expression</typeparam>
	/// <typeparam name="T1">Type of the first parameter for the expression</typeparam>
	/// <typeparam name="T2">Type of the second parameter for the expression</typeparam>
	public class Expression<TResult, T1, T2> : ParameterExpression<TResult>
	{
		#region Constructors
		/// <summary>
		/// Constructs an expression object
		/// </summary>
		/// <param name="expressionName">Name of the expression, must be unique for each expression if used in an ExpressionBlock</param>
		/// <param name="expression">Expression text</param>
		/// <param name="parameterInfo">Information about the first parameter</param>
		/// <param name="parameter2Info">Information about the second parameter</param>
		public Expression(string expressionName, string expression, ExpressionParameter parameterInfo, ExpressionParameter parameter2Info)
			: base(expressionName, expression, parameterInfo, parameter2Info)
		{
		}
		#endregion
		#region Properties
		#endregion
		#region Methods
		/// <summary>
		/// Calls the expression and returns the result
		/// </summary>
		/// <param name="param1">First parameter for the expression</param>
		/// <param name="param2">Second parameter for the expression</param>
		/// <returns>Result of the expression</returns>
		public TResult Call(T1 param1, T2 param2)
		{
			return (TResult)Invoke(new object[]{ param1, param2 });
		}
		#endregion
	}
}
