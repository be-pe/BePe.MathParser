using BePe.MathParser;
using BePe.MathParser.Exceptions;
using BePe.MathParser.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BePe.MathParserTests
{
    [Collection("RollService")]
    public class ShuntingYardTests
    {
        readonly ShuntingYard parser = new(Operator.DefaultOperators);

        [Fact]
        void ShuntingYardThrowsOnWhitespace()
        {
            Assert.Throws<UnexpectedTokenException>(() =>
                parser.Convert(new Token[]{
                    new Token(TokenType.WhiteSpace, " ")
                }).ToList()
            );
        }

        [Fact]
        void ShuntingYardThrowsOnUnmatchedLeftParenthesis()
        {
            Assert.Throws<InvalidExpressionException>(() =>
                parser.Convert(new Token[]{
                    new Token(TokenType.Parenthesis, "("),
                    new Token(TokenType.Number, "6"),
                    new Token(TokenType.Operator, "+"),
                    new Token(TokenType.Number, "1"),
                }).ToList()
            );
        }

        [Fact]
        void ShuntingYardThrowsOnUnmatchedRightParenthesis()
        {
            Assert.Throws<InvalidExpressionException>(() =>
                parser.Convert(new Token[]{
                    new Token(TokenType.Number, "6"),
                    new Token(TokenType.Operator, "+"),
                    new Token(TokenType.Number, "1"),
                    new Token(TokenType.Parenthesis, ")"),
                }).ToList()
            );
        }

        [Theory(DisplayName = "Convert using Shunting-Yard")]
        [MemberData(nameof(TokenInput))]
        void ShuntingYardTest(IEnumerable<Token> input, IEnumerable<Token> expected)
        {
            IEnumerable<Token> output = parser.Convert(input);
            Assert.Equal(expected, output);
        }

        public static TheoryData<Token[], Token[]> TokenInput()
        {
            TheoryData<Token[], Token[]> td = new();
            td.Add(new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "6"),
                }, new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "+"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "6"),
            }, new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "-"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Number, "6"),
                }, new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "/"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "6"),
                }, new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "*"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "6"),
                }, new Token[] {
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "3"),
                }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Operator, "+"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "1"),
                }, new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "+"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Variable, "STR"),
                }, new Token[] {
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Variable, "STR"),
                new Token(TokenType.Operator, "*"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Function, "sin"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Parenthesis, ")"),
                }, new Token[] {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Function, "sin"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Parenthesis, ")"),
                }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Function, "max"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Parenthesis, ")"),
                }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Function, "max"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
                }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Operator, "+"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "5"),
                }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Operator, "^"),
            });
            td.Add(new Token[] {
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
            }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
            });
            td.Add(new Token[] {
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
            }, new Token[] {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Variable, "val"),
                new Token(TokenType.Function, "max"),
            });
            return td;
        }
    }
}
