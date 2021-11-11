using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System;
using System.Collections.Generic;

namespace BePe.MathParser
{
    /// <summary>
    /// This class is used to parse expression (formated as a string, ideally free of whitespaces). <br/>
    /// By default, it is able to parse basic expression as it provides some default operators (+,-,*,/,^) and functions (sin, cos, max, min, abs). <br/>
    /// However, you can define your own operators using the <see cref="Operator"/> class and your own functions using the <see cref="Function"/> class. <br/>
    /// They must be passed as a dictionnary where the keys are the string representation of the Operator/Function.
    /// </summary>
    public class ShuntingYardParser
    {
        readonly Tokenizer tokenizer;
        readonly ShuntingYard shuntingYard;
        readonly RpnInterpreter interpreter;

        /// <summary>
        /// Initialize a parser with the default operators and functions.
        /// It isn't able to parse expressions with variables.
        /// </summary>
        public ShuntingYardParser() : this(Operator.DefaultOperators, Function.DefaultFunctions) { }

        /// <summary>
        /// Initialize a parser with the given operators and functions.
        /// It isn't able to parse expressions with variables.
        /// </summary>
        /// <param name="operators">The operators to use. The keys must be the string representation of each operator.</param>
        /// <param name="functions">The functions to use. The keys must be the string representation of each function.</param>
        public ShuntingYardParser(IReadOnlyDictionary<string, Operator> operators, IReadOnlyDictionary<string, Function> functions) : this(operators, functions, null) { }

        /// <summary>
        /// Initialize a parser with the default operators and functions.
        /// It will try to get the variables values using the given function.
        /// </summary>
        /// <param name="variableFunc">The function to use to get the variables values. The variable strings are passed as found in the expression and the exceptions it throws aren't caught.</param>
        public ShuntingYardParser(Func<string, int> variableFunc) : this(Operator.DefaultOperators, Function.DefaultFunctions, variableFunc) { }

        /// <summary>
        /// Initialize a parser with the given operators and functions.
        /// It will try to get the variables values using the given function.
        /// </summary>
        /// <param name="operators">The operators to use. The keys must be the string representation of each operator.</param>
        /// <param name="functions">The functions to use. The keys must be the string representation of each function.</param>
        /// <param name="variableFunc">The function to use to get the variables values. The variable strings are passed as found in the expression and the exceptions it throws aren't caught.</param>
        public ShuntingYardParser(IReadOnlyDictionary<string, Operator> operators, IReadOnlyDictionary<string, Function> functions, Func<string, int>? variableFunc)
        {
            tokenizer = new Tokenizer(operators);
            shuntingYard = new ShuntingYard(operators);
            interpreter = new RpnInterpreter(operators, functions, variableFunc);
        }

        /// <summary>
        /// Parse the expression and return a double.
        /// </summary>
        /// <param name="expression">The expression to parse. For better results, it shouldn't contain any whitespaces.</param>
        /// <returns>The value computed.</returns>
        /// <exception cref="InvalidTokenException"></exception>
        /// <exception cref="InvalidExpressionException"></exception>
        /// <exception cref="UnexpectedTokenException"></exception>
        /// <exception cref="ArgumentException">The expression is either null, empty, or a whitspace.</exception>
        public int Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Can't parse null or whitespace expressions.", nameof(expression));

            IEnumerable<Token> tokens = tokenizer.Tokenize(expression);
            IEnumerable<Token> rpn = shuntingYard.Convert(tokens);
            return interpreter.Interpret(rpn);
        }
    }
}
