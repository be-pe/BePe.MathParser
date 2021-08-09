using BePe.MathParser.Models;
using System;
using System.Collections.Generic;

namespace BePe.MathParser
{
    public class ShuntingYardParser
    {
        readonly Tokenizer tokenizer;
        readonly ShuntingYard shuntingYard;
        readonly RpnInterpreter interpreter;

        public ShuntingYardParser() : this(Operator.DefaultOperators, Function.DefaultFunctions, null) { }

        public ShuntingYardParser(Func<string, double> variableFunc) : this(Operator.DefaultOperators, Function.DefaultFunctions, variableFunc) { }

        public ShuntingYardParser(IReadOnlyDictionary<string, Operator> operators, IReadOnlyDictionary<string, Function> functions, Func<string, double> variableFunc)
        {
            tokenizer = new Tokenizer(operators);
            shuntingYard = new ShuntingYard(operators);
            interpreter = new RpnInterpreter(operators, functions, variableFunc);
        }

        public double Parse(string expression)
        {
            IEnumerable<Token> tokens = tokenizer.Tokenize(expression);
            IEnumerable<Token> rpn = shuntingYard.Convert(tokens);
            return interpreter.Interpret(rpn);
        }
    }
}
