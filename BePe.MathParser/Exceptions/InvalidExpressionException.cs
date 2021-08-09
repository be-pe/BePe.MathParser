using System;
using System.Diagnostics.CodeAnalysis;

namespace BePe.MathParser.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException() { }
        public InvalidExpressionException(string message) : base(message) { }
        public InvalidExpressionException(string message, Exception innerException) : base(message, innerException) { }
        protected InvalidExpressionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
