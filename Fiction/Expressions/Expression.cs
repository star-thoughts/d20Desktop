using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Fiction.Expressions
{
    /// <summary>
    /// Class that handles information about an expression
    /// </summary>
    public abstract class Expression
    {
        #region Constructors
        internal Expression(string expressionName, string expression, params ExpressionParameter[] parameterInfo)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(expressionName, nameof(expressionName));
            Exceptions.ThrowIfArgumentNullOrEmpty(expression, nameof(expression));

            Name = expressionName;
            ExpressionText = expression;
            ParameterInfo = new ReadOnlyCollection<Expressions.ExpressionParameter>(parameterInfo);
            SingleLineExpression = true;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the expression
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the expression text
        /// </summary>
        public string ExpressionText { get; private set; }
        /// <summary>
        /// Gets the actual compiled source code
        /// </summary>
        public string? CompiledText { get; protected set; }
        /// <summary>
        /// Gets whether or not this has been compiled
        /// </summary>
        public bool IsCompiled { get; private set; }
        /// <summary>
        /// Gets or sets the parameter information for this expression
        /// </summary>
        public ReadOnlyCollection<ExpressionParameter> ParameterInfo { get; private set; }
        /// <summary>
        /// Gets or sets the MethodInfo of the created expression
        /// </summary>
        public MethodInfo? MethodInfo { get; internal set; }
        /// <summary>
        /// Gets or sets whether or not the expression is a single line
        /// </summary>
        public bool SingleLineExpression { get; set; }
        /// <summary>
        /// Gets or sets a list of assemblies to include
        /// </summary>
        protected ReadOnlyCollection<AssemblyName>? Assemblies { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the assemblies to reference in the compiled code
        /// </summary>
        /// <param name="assemblies">Assemblies to reference</param>
        [MemberNotNull(nameof(Assemblies))]
        public void SetAssemblies(IEnumerable<AssemblyName> assemblies)
        {
            Assemblies = new ReadOnlyCollection<AssemblyName>(assemblies.ToArray());
        }
        /// <summary>
        /// Sets the assemblies to reference in the compiled code to the assemblies used by the calling method
        /// </summary>
        public void SetAssemblies()
        {
            SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies().ToArray());
        }
        /// <summary>
        /// Invokes the given expression as a method call
        /// </summary>
        /// <param name="parms">Parameters to pass to the expression</param>
        /// <returns>Return value of the expression</returns>
        public object? Invoke(params object?[]? parms)
        {
            //  If not compiled already, compile it
            if (MethodInfo == null)
            {
                //  If no assemblies assigned, just use the calling method's assemblies
                if (Assemblies == null)
                    SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());
                Compile();
            }

            return MethodInfo.Invoke(null, parms);
        }
        /// <summary>
        /// Compiles the expression into a single assembly so that it can be called
        /// </summary>
        [MemberNotNull(nameof(MethodInfo))]
        public abstract void Compile();
        /// <summary>
        /// Creates a method signature to run the expression
        /// </summary>
        /// <returns>String representation of the method</returns>
        internal abstract string BuildMethodSignature();
        /// <summary>
        /// Compiles the expression into a single assembly so that it can be called
        /// </summary>
        /// <param name="returnType">Return type of the method being created</param>
        /// <param name="parameters">Parameter information</param>
        [MemberNotNull(nameof(MethodInfo))]
        internal void Compile(Type returnType, params ExpressionParameter[] parameters)
        {
            //  If no assemblies assigned, just use the calling method's assemblies
            if (Assemblies == null)
                SetAssemblies(Assembly.GetCallingAssembly().GetReferencedAssemblies());

            StringBuilder code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("");
            code.AppendLine("namespace Fiction.CodeGen.Dynamic");
            code.AppendLine("{");
            code.AppendLine("public static class CompiledClass");
            code.AppendLine("{");
            code.AppendLine(BuildMethodSignature(returnType, parameters));
            code.AppendLine("}");
            code.AppendLine("}");

            string source = code.ToString();

            Type type = ExpressionBlock.CompileSource(source, Assemblies.ToArray());

            CompiledText = source;
            MethodInfo = type.GetMethod(Name);

            if (MethodInfo is null)
                throw new InvalidOperationException("Could not compile method.");

            IsCompiled = true;
        }
        /// <summary>
        /// Creates a method signature to run the expression
        /// </summary>
        /// <param name="returnType">Return type of the compiled method</param>
        /// <param name="parameters">Parameter information</param>
        internal string BuildMethodSignature(Type returnType, params ExpressionParameter[] parameters)
        {
            string parameterText = string.Join(", ", parameters.Select(p => p.ParameterType?.ToString() + " " + p.Name));
            string returnString = returnType.ToString();
            if (object.ReferenceEquals(typeof(void), returnType))
                returnString = "void";

            StringBuilder code = new StringBuilder();
            code.AppendFormat(CultureInfo.InvariantCulture, "public static {0} {1}({2})", returnString, Name, parameterText);
            code.AppendLine("{");
            if (SingleLineExpression)
                code.AppendFormat(CultureInfo.InvariantCulture, "return {0};", ExpressionText);
            else
                code.AppendFormat(CultureInfo.InvariantCulture, "{0}", ExpressionText);
            code.AppendLine("}");

            CompiledText = code.ToString();

            return CompiledText;
        }
        #endregion
    }
}
