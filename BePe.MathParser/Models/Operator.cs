using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BePe.MathParser.Models
{
    public sealed class Operator
    {
        /// <summary>
        /// The display name if the operator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The precedence of the operator.
        /// By default, the addition has a precedence of 1 and the multiplication has a precedence of 2.
        /// </summary>
        public int Precedence { get; }

        /// <summary>
        /// Default is false. See <see href="https://en.wikipedia.org/wiki/Operator_associativity">Operator associativity on Wikipedia</see>
        /// </summary>
        public bool RightAssociative { get; } = false;

        /// <summary>
        /// The function called when the operator is encountered.
        /// </summary>
        public Func<int, int, int> Operation { get; }

        /// <summary>
        /// Initialize a left-associative operator.
        /// </summary>
        /// <param name="name">The display name if the operator.</param>
        /// <param name="precedence">The precedence of the operator.</param>
        /// <param name="operation">The function called when the operator is encountered.</param>
        public Operator(string name, int precedence, Func<int, int, int> operation)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "An operator must have a non-empty representation.");
            if (precedence <= 0)
                throw new ArgumentOutOfRangeException(nameof(precedence), "An operator must have a precedence greater than 0.");
            if (operation is null)
                throw new ArgumentNullException(nameof(operation), "An operator must have an operation.");

            Name = name;
            Precedence = precedence;
            Operation = operation;
        }

        /// <summary>
        /// Initialize an operator.
        /// </summary>
        /// <param name="rightAssociative">Wheter the operator is right associative or not.</param>
        /// <inheritdoc cref="Operator(string, int, Func{int, int, int})"/>
        public Operator(string name, int precedence, Func<int, int, int> operation, bool rightAssociative) : this(name, precedence, operation)
        {
            RightAssociative = rightAssociative;
        }

        /// <summary>
        /// Copy constructor. Performs a deep copy.
        /// </summary>
        /// <param name="other">The instance to copy the values from.</param>
        public Operator(Operator other) : this(other.Name, other.Precedence, other.Operation, other.RightAssociative) { }

        public bool HasLowerPrecedenceThan(Operator other)
        {
            return RightAssociative ? Precedence < other.Precedence : Precedence <= other.Precedence;
        }

        static Operator()
        {
            Dictionary<string, Operator> defaultOperators = new Dictionary<string, Operator>
            {
                ["+"] = new Operator("+", 1, (a, b) => a + b),
                ["-"] = new Operator("-", 1, (a, b) => a - b),
                ["*"] = new Operator("*", 2, (a, b) => a * b),
                ["/"] = new Operator("/", 2, (a, b) => a / b),
                ["^"] = new Operator("^", 3, (a, b) => (int) Math.Pow(a, b), true)
            };
            DefaultOperators = new ReadOnlyDictionary<string, Operator>(defaultOperators);
        }

        /// <summary>
        /// Contains 5 operations : 
        /// <list type="table">
        ///     <listheader> 
        ///         <term>Operation</term>
        ///         <description>Precedence</description>
        ///     </listheader>
        ///     <item>
        ///         <term>Addition</term>
        ///         <description>1</description>
        ///     </item>
        ///     <item>
        ///         <term>Substraction</term>
        ///         <description>1</description>
        ///     </item>
        ///     <item>
        ///         <term>Multiplication</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <term>Division</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <term>Exponentiation</term>
        ///         <description>3</description>
        ///     </item>
        /// </list>
        /// </summary>
        public static IReadOnlyDictionary<string, Operator> DefaultOperators { get; }
    }
}
