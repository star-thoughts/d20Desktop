using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Fiction.Expressions
{
    /// <summary>
    /// Class that encapsulates multiple expressions together to streamline compilation into a single assembly
    /// </summary>
    public class ExpressionBlock
    {
        #region Constructors
        /// <summary>
        /// Constructs an ExpressionBlock for the given expressions
        /// </summary>
        /// <param name="expressions">Expressions to include in the expression block</param>
        public ExpressionBlock(IEnumerable<Expression> expressions)
        {
            Exceptions.ThrowIfArgumentNull(expressions, nameof(expressions));

            Expressions = new ReadOnlyCollection<Expression>(expressions.ToList());
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Sets the assemblies to reference in the compiled code
        /// </summary>
        /// <param name="assemblies">Assemblies to reference</param>
        [MemberNotNull(nameof(Assemblies))]
        public void SetAssemblies(IEnumerable<AssemblyName> assemblies)
        {
            Exceptions.ThrowIfArgumentNull(assemblies, nameof(assemblies));

            Assemblies = assemblies.ToArray();
        }
        /// <summary>
        /// Sets the assemblies to reference in the compiled code to the assemblies used by the calling method
        /// </summary>
        public void SetAssemblies()
        {
            Assemblies = Assembly.GetCallingAssembly().GetReferencedAssemblies().ToArray();
        }
        /// <summary>
        /// Gets a collection of the expressions compiled by this block
        /// </summary>
        public ReadOnlyCollection<Expression> Expressions { get; private set; }
        /// <summary>
        /// Gets or sets a list of assemblies to load
        /// </summary>
        private AssemblyName[]? Assemblies { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Compiles all of the expressions into a single assembly
        /// </summary>
        public void Compile()
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
            code.AppendLine("public class CompiledClass");
            code.AppendLine("{");
            foreach (Expression expression in Expressions)
                code.AppendLine(expression.BuildMethodSignature());
            code.AppendLine("}");
            code.AppendLine("}");

            string source = code.ToString();
            Type type = CompileSource(source, Assemblies);

            foreach (Expression expression in Expressions)
                expression.MethodInfo = type.GetMethod(expression.Name);
        }
        /// <summary>
        /// Compiles source code using the given language
        /// </summary>
        /// <param name="source">The source code to compile</param>
        /// <param name="assemblies">Additional assemblies to include in the compile</param>
        /// <returns>Type of the class that has all of the methods that represent the expressions that are compiled</returns>
        /// <remarks>
        /// This only supports CSharp at the moment
        /// </remarks>
        internal static Type CompileSource(string source, AssemblyName[] assemblies)
        {
            using (CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }))
            {
                //CodeDomProvider provider = CodeDomProvider.CreateProvider(language);
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;
                compilerParameters.GenerateExecutable = false;

                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                foreach (AssemblyName assemblyName in assemblies.Where(p => !p.Name?.Contains("mscorlib") == true))
                    compilerParameters.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);

                //compilerParameters.ReferencedAssemblies.Add("System.dll");
                //compilerParameters.ReferencedAssemblies.Add("System.Core.dll");

                CompilerResults results = provider.CompileAssemblyFromSource(compilerParameters, source);
                if (results.Errors.HasErrors)
                    throw new ExpressionCompilerException(results.Errors, results.Output.Cast<string>().ToArray());

                Assembly assembly = results.CompiledAssembly;
                Type type = assembly.GetTypes().First(p => p.Name == "CompiledClass");
                return type;
            }
        }

        #endregion
    }
}
