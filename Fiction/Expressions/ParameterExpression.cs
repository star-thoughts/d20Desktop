using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Class that handles expressions that allow parameters
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public abstract class ParameterExpression<TResult> : Expression
	{
		#region Constructors
		/// <summary>
		/// Constructs a ParameterExpression object
		/// </summary>
		/// <param name="expressionName">Name of the expression, must be unique amongst all expressions in an ExpressionBlock</param>
		/// <param name="expression">Expression to compile</param>
		/// <param name="parameterInfo">Information about parameters used by the expression</param>
		protected ParameterExpression(string expressionName, string expression, params ExpressionParameter[] parameterInfo)
			: base(expressionName, expression, parameterInfo)
		{
		}
		#endregion
		#region Methods
		/// <summary>
		/// Compiles the expression into a single assembly so that it can be called
		/// </summary>
		public override void Compile()
		{
			//  If no assemblies assigned, just use the calling method's assemblies
			if (Assemblies == null)
				SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies()); 
			Compile(typeof(TResult), ParameterInfo.ToArray());
		}
		/// <summary>
		/// Creates a method signature to run the expression
		/// </summary>
		/// <returns>String representation of the method</returns>
		internal override string BuildMethodSignature()
		{
			return BuildMethodSignature(typeof(TResult), ParameterInfo.ToArray());
		}
		#endregion
	}
}
