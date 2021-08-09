using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BePe.MathParser
{
    class Tokenizer
    {
        public IReadOnlyDictionary<string, Operator> Operators { get; }

        public Tokenizer(IReadOnlyDictionary<string, Operator> operators)
        {
            Operators = operators;
        }

        public IEnumerable<Token> Tokenize(string expression)
        {
            StringBuilder token = new StringBuilder();
            using StringReader reader = new StringReader(expression);

            int curr;
            while ((curr = reader.Read()) != -1)
            {
                var ch = (char)curr;
                var currType = DetermineType(ch);
                if (currType == TokenType.WhiteSpace)
                    continue;

                token.Append(ch);

                var next = reader.Peek();
                var nextType = next != -1 ? DetermineType((char)next) : TokenType.WhiteSpace;
                if (currType == TokenType.Parenthesis || currType == TokenType.Operator)
                {
                    yield return new Token(currType, token.ToString());
                    token.Clear();
                }
                else if (currType != nextType)
                {
                    if ('('.Equals((char)next))
                        yield return new Token(TokenType.Function, token.ToString());
                    else
                        yield return new Token(currType, token.ToString());
                    token.Clear();
                }
            }
        }

        private TokenType DetermineType(char ch)
        {
            if (Operators.ContainsKey(Convert.ToString(ch)))
                return TokenType.Operator;
            if (char.IsLetter(ch))
                return TokenType.Variable;
            if (char.IsDigit(ch))
                return TokenType.Number;
            if (char.IsWhiteSpace(ch))
                return TokenType.WhiteSpace;
            if (',' == ch)
                return TokenType.Comma;
            if ('(' == ch || ')' == ch)
                return TokenType.Parenthesis;

            throw new InvalidTokenException($"Unexpected character : '{ch}'");
        }
    }
}
