using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BePe.MathParser
{
    class ShuntingYard
    {
        public IReadOnlyDictionary<string, Operator> Operators { get; }

        public ShuntingYard(IReadOnlyDictionary<string, Operator> operators)
        {
            Operators = operators;
        }

        public IEnumerable<Token> Convert(IEnumerable<Token> tokens)
        {
            Stack<Token> stack = new Stack<Token>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                    case TokenType.Variable:
                        yield return token;
                        break;
                    case TokenType.Function:
                        stack.Push(token);
                        break;
                    case TokenType.Comma:
                        while (stack.Peek().Value != "(")
                            yield return stack.Pop();
                        break;
                    case TokenType.Operator:
                        while (stack.Any() && stack.Peek().Type == TokenType.Operator && CompareOperators(token.Value, stack.Peek().Value))
                            yield return stack.Pop();
                        stack.Push(token);
                        break;
                    case TokenType.Parenthesis:
                        if ("(".Equals(token.Value))
                            stack.Push(token);
                        else
                        {
                            while (stack.TryPeek(out Token? value) && value.Value != "(")
                                yield return stack.Pop();
                            try
                            {
                                stack.Pop();
                            }
                            catch (InvalidOperationException e) when (e.Message == "Stack empty.")
                            {
                                throw new InvalidExpressionException("Unmatched parenthesis.", e);
                            }
                            if (stack.Count > 0 && stack.Peek().Type == TokenType.Function)
                                yield return stack.Pop();
                        }
                        break;
                    default:
                        throw new UnexpectedTokenException($"Unexpected token : {token}.");
                }
            }
            while (stack.Any())
            {
                var tok = stack.Pop();
                if (tok.Type == TokenType.Parenthesis)
                    throw new InvalidExpressionException("Mismatched parentheses");
                yield return tok;
            }
        }

        private bool CompareOperators(string op1, string op2) => Operators[op1].HasLowerPrecedenceThan(Operators[op2]);
    }
}
