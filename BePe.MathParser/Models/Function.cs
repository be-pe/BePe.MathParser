using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BePe.MathParser.Models
{
    public sealed class Function
    {
        /// <summary>
        /// The string representation of the function without the parentheses.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The function that is called when the function is encountered.
        /// </summary>
        public Func<double[], double> Func { get; }

        /// <summary>
        /// The number of parameter the function takes in. Must be greater than 0.
        /// </summary>
        public int ParameterCount { get; }

        /// <summary>
        /// Initialize a new function with the given parameter.
        /// </summary>
        /// <param name="name">The string representation of the function without the parentheses.</param>
        /// <param name="function">The function that is called when the function is encountered.</param>
        /// <param name="parameterCount">The number of parameter the function takes in. Must be greater than 0.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Function(string name, Func<double[], double> function, int parameterCount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "A function must have a non-empty representation.");
            if (function is null)
                throw new ArgumentNullException(nameof(function), "A function must have a non-null function.");
            if (parameterCount < 1)
                throw new ArgumentOutOfRangeException(nameof(parameterCount), "A function must have at least a parameter.");

            Name = name;
            Func = function;
            ParameterCount = parameterCount;
        }

        /// <summary>
        /// Copy constructor. Performs a deep copy.
        /// </summary>
        /// <param name="other">The instance to copy the values from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Function(Function other) : this(other.Name, other.Func, other.ParameterCount) { }

        static Function()
        {
            Dictionary<string, Function> defaultFunctions = new Dictionary<string, Function>
            {
                ["sin"] = new Function("sin", (d) => Math.Sin(d[0]), 1),
                ["cos"] = new Function("cos", (d) => Math.Cos(d[0]), 1),
                ["abs"] = new Function("abs", (d) => Math.Abs(d[0]), 1),
                ["min"] = new Function("min", (d) => Math.Min(d[0], d[1]), 2),
                ["max"] = new Function("max", (d) => Math.Max(d[0], d[1]), 2)
            };
            DefaultFunctions = new ReadOnlyDictionary<string, Function>(defaultFunctions);
        }
        /// <summary>
        /// Contains 5 default functions : sin, cos, abs, min, max. Each one is implemented using <see cref="Math"/>.
        /// </summary>
        public static IReadOnlyDictionary<string, Function> DefaultFunctions { get; }
    }
}
