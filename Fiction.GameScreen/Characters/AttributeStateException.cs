using System;
using System.Runtime.Serialization;

namespace Fiction.GameScreen.Characters
{
    [Serializable]
    public class AttributeStateException : Exception
    {
        public AttributeStateException()
        {
        }

        public AttributeStateException(string message) 
            : base(message)
        {
        }

        public AttributeStateException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected AttributeStateException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}