using BePe.MathParser;
using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BePe.MathParserTests
{
    [Collection("RollService")]
    public class TokenizerTests
    {
        readonly Tokenizer parser = new(Operator.DefaultOperators);

        [Fact]
        void TokenizeIgnoresSingleWhitespace()
        {
            IEnumerable<Token> result = parser.Tokenize(" ");
            Assert.False(result.Any());
        }

        [Fact]
        void TokenizeIgnoresWhitespaceInExpression()
        {
            string input = "6 6";
            IEnumerable<Token> expected = new List<Token>
            {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6")
            };
            IEnumerable<Token> result = parser.Tokenize(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        void TokenizeThrowsOnUnknownToken()
        {
            string input = "=";
            Assert.Throws<InvalidTokenException>(() => parser.Tokenize(input).ToList());
        }

        [Theory(DisplayName = "Single token")]
        [MemberData(nameof(SimpleData))]
        void SingleTokenString(string expression, Token expected)
        {
            Token actual = parser.Tokenize(expression).First();
            Assert.Equal(expected, actual);
        }

        public static TheoryData<string, Token> SimpleData()
        {
            TheoryData<string, Token> td = new();

            td.Add("6", new Token(TokenType.Number, "6"));
            td.Add("+", new Token(TokenType.Operator, "+"));
            td.Add("*", new Token(TokenType.Operator, "*"));
            td.Add("a", new Token(TokenType.Variable, "a"));
            td.Add("aa", new Token(TokenType.Variable, "aa"));
            td.Add(",", new Token(TokenType.Comma, ","));
            td.Add("(", new Token(TokenType.Parenthesis, "("));
            td.Add(")", new Token(TokenType.Parenthesis, ")"));
            td.Add("sin()", new Token(TokenType.Function, "sin"));

            return td;
        }

        [Theory(DisplayName = "Multiple tokens")]
        [MemberData(nameof(ComplexData))]
        void MultipleTokenString(string expression, Token[] expected)
        {
            IEnumerable<Token> result = parser.Tokenize(expression);
            Assert.Equal(expected, result.ToArray());
        }

        public static TheoryData<string, Token[]> ComplexData()
        {
            TheoryData<string, Token[]> td = new();

            td.Add("6+6", new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "6"),
            });
            td.Add("6-6", new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "6"),
            });
            td.Add("6/6", new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Number, "6"),
            });
            td.Add("6*6", new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "6"),
            });
            td.Add("6^6", new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "6"),
            });
            td.Add("val+1", new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "1"),
            });
            td.Add("val*STR", new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Variable, "STR"),
            });
            td.Add("sin(1)", new Token[] {
                new Token(TokenType.Function, "sin"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Parenthesis, ")"),
            });
            td.Add("max(2,1)", new Token[] {
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Parenthesis, ")"),
            });
            td.Add("max(2,val)", new Token[] {
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Parenthesis, ")"),
            });
            td.Add("2+5*5", new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
            });
            td.Add("2^5^5", new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "5"),
            });
            td.Add("(2+5)*5", new Token[] {
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
            });
            td.Add("max((2+5)*5,val)", new Token[] {
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Parenthesis, ")"),
            });

            return td;
        }
    }
}
