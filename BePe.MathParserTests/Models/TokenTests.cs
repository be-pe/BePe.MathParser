using BePe.MathParser.Models;
using BePe.MathParserTests.TestCaseOrdering;
using System;
using Xunit;

namespace BePe.MathParserTests.Models
{
    [TestCaseOrderer("BePe.MathParserTests.TestCaseOrdering.PriorityOrderer", "BePe.MathParserTests")]
    public class TokenTests
    {
        [Fact]
        [Priority(1)]
        public void ValueCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Token(TokenType.Number, null);
            });
        }

        [Fact]
        [Priority(2)]
        public void PropertiesDontChangeValues()
        {
            Token t = new(TokenType.Number, "+");
            Assert.Equal(TokenType.Number, t.Type);
            Assert.Equal("+", t.Value);
        }

        [Fact]
        [Priority(2)]
        public void ToStringReturnsTemplate()
        {
            TokenType tokenType = TokenType.Variable;
            string value = "+";
            Token t = new(tokenType, value);
            Assert.Equal($"{tokenType}: {value}", t.ToString());
        }

        [Fact]
        [Priority(3)]
        public void EqualIfTokenTypeAndStringAreEqual()
        {
            Token token = new(TokenType.Parenthesis, "(");
            Token goodToken = new(TokenType.Parenthesis, "(");

            Assert.False(token != goodToken);
            Assert.True(token == goodToken);

            Assert.True(token.Equals(goodToken), "IEquatable<Token>");

            Assert.True(token.Equals((object)goodToken), "object.Equals()");
        }

        [Fact]
        [Priority(4)]
        public void HashCodeIsSameIfEqual()
        {
            Token token = new(TokenType.Number, "+");
            Token goodToken = new(TokenType.Number, "+");

            Assert.NotSame(token, goodToken);
            Assert.Equal(token.GetHashCode(), goodToken.GetHashCode());
        }

        [Fact]
        [Priority(5)]
        public void NotEqualIfTokenTypeIsDifferent()
        {
            Token token = new(TokenType.Number, "+");
            Token badToken = new(TokenType.Variable, "+");

            Assert.True(token != badToken);
            Assert.False(token == badToken);

            Assert.False(token.Equals(badToken), "IEquatable<Token>");

            Assert.False(token.Equals((object)badToken), "object.Equals()");
        }

        [Fact]
        [Priority(5)]
        public void NotEqualIfValueIsDifferent()
        {
            Token token = new(TokenType.Number, "+");
            Token badToken = new(TokenType.Number, "-");

            Assert.True(token != badToken);
            Assert.False(token == badToken);

            Assert.False(token.Equals(badToken), "IEquatable<Token>");

            Assert.False(token.Equals((object)badToken), "object.Equals()");
        }

        [Fact]
        [Priority(5)]
        public void NotEqualIfTokenTypeAndValueAreDifferent()
        {
            Token token = new(TokenType.Number, "+");
            Token badToken = new(TokenType.Variable, "*");

            Assert.True(token != badToken);
            Assert.False(token == badToken);

            Assert.False(token.Equals(badToken), "IEquatable<Token>");

            Assert.False(token.Equals((object)badToken), "object.Equals()");
        }

        [Fact]
        [Priority(5)]
        public void NotEqualIfTypeIsDifferent()
        {
            Token token = new(TokenType.Number, "+");
            string notAToken = "I'm not a token";

            Assert.False(token.Equals(notAToken), "object.Equals()");
        }

        [Fact, Priority(6)]
        public void CopyConstructorTest()
        {
            Token expected = new(TokenType.Number, "+");
            Token actual = new(expected);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Value, actual.Value);
        }
    }
}
