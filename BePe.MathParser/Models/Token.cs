using System;
using Xunit.Abstractions;

namespace BePe.MathParser.Models
{
    public enum TokenType { Number, Variable, Function, Parenthesis, Operator, Comma, WhiteSpace };

    public sealed class Token : IEquatable<Token>, IXunitSerializable
    {
        /// <summary>
        /// The type of the token.
        /// </summary>
        public TokenType Type { get; private set; }

        /// <summary>
        /// The value of the token. Should be its string representation.
        /// </summary>
        public string Value { get; private set; }

        public override string ToString() => $"{Type}: {Value}";

        /// <summary>
        /// Initialize a whitespace token whose representation is a single space.
        /// </summary>
        public Token()
        {
            Type = TokenType.WhiteSpace;
            Value = " ";
        }

        /// <summary>
        /// Initialize a token.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="value">The string representation of the token.</param>
        public Token(TokenType type, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "A token must have a non-empty representation.");

            Type = type;
            Value = value;
        }

        /// <summary>
        /// Copy constructor. Performs a deep copy.
        /// </summary>
        /// <param name="other">The token to copy from.</param>
        public Token(Token other) : this(other.Type, other.Value) { }

        public bool Equals(Token other) => Type == other.Type && Value.Equals(other.Value);

        public override bool Equals(object obj) => obj is Token token && Equals(token);

        public override int GetHashCode() => HashCode.Combine(Type, Value);

        public void Deserialize(IXunitSerializationInfo info)
        {
            Type = info.GetValue<TokenType>(nameof(Type));
            Value = info.GetValue<string>(nameof(Value));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Type), Type, typeof(TokenType));
            info.AddValue(nameof(Value), Value, typeof(string));
        }

        public static bool operator ==(Token left, Token right) => left.Equals(right);

        public static bool operator !=(Token left, Token right) => !(left == right);
    }
}
