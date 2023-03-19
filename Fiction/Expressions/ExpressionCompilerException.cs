using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Fiction.Expressions
{
    /// <summary>
    /// Exception that represents a problem compiling an expression
    /// </summary>
    [Serializable]
    public class ExpressionCompilerException : Exception
    {
        /// <summary>
        /// Constructs a default ExpressionCompilerException
        /// </summary>
        public ExpressionCompilerException() : base("An error occurred while compiling the expression.") { }
        /// <summary>
        /// Constructs a default ExpressionCompilerException
        /// </summary>
        /// <param name="message">Exception message</param>
        public ExpressionCompilerException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Constructs a default ExpressionCompilerException
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public ExpressionCompilerException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Constructs a ExpressionCompilerException with the given error information
        /// </summary>
        /// <param name="errors">Collection of compiler errors</param>
        /// <param name="output">Text output from the compiler</param>
        public ExpressionCompilerException(CompilerErrorCollection errors, string[] output)
            : base("An error occurred while compiling the expression.")
        {
            Errors = errors;
            Output = new ReadOnlyCollection<string>(new List<string>(output));
        }
        /// <summary>
        /// Constructs an ExpressionCompilerException object from a serialization
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected ExpressionCompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Errors = info.GetValue("Errors", typeof(CompilerErrorCollection)) as CompilerErrorCollection;
            string[]? output = info.GetValue("Output", typeof(string[])) as string[]
                ?? Array.Empty<string>();

            Output = new ReadOnlyCollection<string>(new List<string>(output));
        }
        /// <summary>
        /// Get a collection of errors produced by the compiler
        /// </summary>
        public CompilerErrorCollection? Errors { get; private set; }
        /// <summary>
        /// Gets the text output produced by the compiler
        /// </summary>
        public ReadOnlyCollection<string>? Output { get; private set; }
        /// <summary>
        /// Writes the object data to the current serialization
        /// </summary>
        /// <param name="info">Information about the serialization</param>
        /// <param name="context">Streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Errors", Errors);
            info.AddValue("Output", Output?.ToArray());
        }
    }
}
