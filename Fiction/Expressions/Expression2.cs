using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Class that represents an expression with a single parameter
	/// </summary>
	/// <typeparam name="TResult">Return type of the expression</typeparam>
	/// <typeparam name="T1">Type of the first parameter for the expression</typeparam>
	public class Expression<TResult, T1> : ParameterExpression<TResult>
	{
		#region Constructors
		/// <summary>
		/// Constructs an expression object
		/// </summary>
		/// <param name="expressionName">Name of the expression, must be unique for each expression if used in an ExpressionBlock</param>
		/// <param name="expression">Expression text</param>
		/// <param name="parameterInfo">Information about the first parameter</param>
		public Expression(string expressionName, string expression, ExpressionParameter parameterInfo)
			: base(expressionName, expression, parameterInfo)
		{
		}
		#endregion
		#region Properties
		#endregion
		#region Methods
		/// <summary>
		/// Calls the expression and return the result
		/// </summary>
		/// <returns>Result of the expression</returns>
		public TResult? Call(T1 param1)
		{
			//  If no assemblies assigned, just use the calling method's assemblies
			if (Assemblies == null)
				SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());
			return (TResult?)Invoke(new object?[] { param1 });
		}
		#endregion
	}
}
