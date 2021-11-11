using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System;
using System.Collections.Generic;

namespace BePe.MathParser
{
    class RpnInterpreter
    {
        public IReadOnlyDictionary<string, Operator> Operators { get; }

        public IReadOnlyDictionary<string, Function> Functions { get; }

        public Func<string, int>? VarFunction { get; set; }

        public RpnInterpreter(IReadOnlyDictionary<string, Operator> operators, IReadOnlyDictionary<string, Function> functions)
        {
            Operators = operators;
            Functions = functions;
        }

        public RpnInterpreter(IReadOnlyDictionary<string, Operator> operators, IReadOnlyDictionary<string, Function> functions, Func<string, int>? varFunction) :
            this(operators, functions)
        {
            VarFunction = varFunction;
        }

        public int Interpret(IEnumerable<Token> RPNTokens)
        {
            Stack<int> stack = new Stack<int>();
            foreach (Token token in RPNTokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(int.Parse(token.Value));
                        break;
                    case TokenType.Variable:
                        stack.Push(GetVariable(token.Value));
                        break;
                    case TokenType.Function:
                        try
                        {
                            Function function = Functions[token.Value];
                            int[] parameters = new int[function.ParameterCount];
                            for (int i = function.ParameterCount - 1; i >= 0; i--)
                                parameters[i] = stack.Pop();
                            stack.Push(function.Func(parameters));
                        }
                        catch (KeyNotFoundException e)
                        {
                            throw new InvalidTokenException($"Unknown function : {token.Value}", e);
                        }
                        catch (InvalidOperationException e) when (e.Message == "Stack empty.")
                        {
                            throw new InvalidExpressionException("Expression invalid : not enough tokens.", e);
                        }
                        break;
                    case TokenType.Operator:
                        try
                        {
                            Operator op = Operators[token.Value];
                            int b = stack.Pop();
                            int a = stack.Pop();
                            int result = op.Operation(a, b);
                            stack.Push(result);
                        }
                        catch (KeyNotFoundException e)
                        {
                            throw new InvalidTokenException($"Unknown operator : {token.Value}", e);
                        }
                        catch (InvalidOperationException e) when (e.Message == "Stack empty.")
                        {
                            throw new InvalidExpressionException("Expression invalid : not enough tokens.", e);
                        }
                        break;
                    default:
                        throw new UnexpectedTokenException(token.ToString());
                }
            }
            if (stack.Count != 1)
                throw new InvalidExpressionException("Wrong number of tokens.");

            return stack.Pop();
        }

        private int GetVariable(string variableName)
        {
            if (VarFunction is null)
                throw new InvalidOperationException("Can't use variables without providing a function.");

            return VarFunction(variableName);
        }
    }
}
