using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Class that represents an expression with three parameters
	/// </summary>
	/// <typeparam name="TResult">Result type of the expression</typeparam>
	/// <typeparam name="T1">Type of the first parameter for the expression</typeparam>
	/// <typeparam name="T2">Type of the second parameter for the expression</typeparam>
	/// <typeparam name="T3">Type of the third parameter for the expression</typeparam>
	/// <typeparam name="T4">Type of the fourth parameter for the expression</typeparam>
	/// <typeparam name="T5">Type of the fifth parameter for the expression</typeparam>
	public class Expression<TResult, T1, T2, T3, T4, T5> : ParameterExpression<TResult>
	{
		#region Constructors
		/// <summary>
		/// Constructs an expression object
		/// </summary>
		/// <param name="expressionName">Name of the expression, must be unique for each expression if used in an ExpressionBlock</param>
		/// <param name="expression">Expression text</param>
		/// <param name="parameterInfo">Information about the first parameter</param>
		/// <param name="parameter2Info">Information about the second parameter</param>
		/// <param name="parameter3Info">Information about the third parameter</param>
		/// <param name="parameter4Info">Information about the fourth parameter</param>
		/// <param name="parameter5Info">Information about the fifth parameter</param>
		public Expression(string expressionName,
			string expression,
			ExpressionParameter parameterInfo,
			ExpressionParameter parameter2Info,
			ExpressionParameter parameter3Info,
			ExpressionParameter parameter4Info,
			ExpressionParameter parameter5Info)
			: base(expressionName, expression, parameterInfo, parameter2Info, parameter3Info, parameter4Info, parameter5Info)
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
		/// <param name="param3">Third parameter for the expression</param>
		/// <param name="param4">Fourth parameter for the expression</param>
		/// <param name="param5">Fifth parameter for the expression</param>
		/// <returns>Result of the expression</returns>
		public TResult Call(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
		{
			//  If no assemblies assigned, just use the calling method's assemblies
			if (Assemblies == null)
				SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());
			return (TResult)Invoke(new object[] { param1, param2, param3, param4, param5 });
		}
		#endregion
	}
}
