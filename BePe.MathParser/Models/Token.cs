using System;
using Xunit.Abstractions;

namespace BePe.MathParser.Models
{
    public enum TokenType { Number, Variable, Function, Parenthesis, Operator, Comma, WhiteSpace };

    public sealed class Token : IEquatable<Token>, IXunitSerializable
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set; }

        public override string ToString() => $"{Type}: {Value}";

        public Token() { }
        public Token(TokenType type, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "A token must have a non-empty representation.");

            Type = type;
            Value = value;
        }

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
