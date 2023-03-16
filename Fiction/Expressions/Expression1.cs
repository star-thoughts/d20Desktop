using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Expressions
{
	/// <summary>
	/// Expression that takes no parameters
	/// </summary>
	/// <typeparam name="TResult">Result type of the expression</typeparam>
	public class Expression<TResult> : Expression
	{
		#region Constructors
		/// <summary>
		/// Constructs an Expression object
		/// </summary>
		/// <param name="expressionName">Name of the expression, must be unique if using an ExpressionBlock</param>
		/// <param name="expression">Expression to compile</param>
		public Expression(string expressionName, string expression)
			: base(expressionName, expression)
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
		public TResult Call()
		{
			//  If no assemblies assigned, just use the calling method's assemblies
			if (Assemblies == null)
				SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());
			return (TResult)Invoke(null);
		}
		/// <summary>
		/// Compiles the expression into a single assembly so that it can be called
		/// </summary>
		public override void Compile()
		{
			//  If no assemblies assigned, just use the calling method's assemblies
			if (Assemblies == null)
				SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());
			Compile(typeof(TResult));
		}
		/// <summary>
		/// Creates a method signature to run the expression
		/// </summary>
		/// <returns>String representation of the method</returns>
		internal override string BuildMethodSignature()
		{
			return BuildMethodSignature(typeof(TResult));
		}
		#endregion
	}
}
