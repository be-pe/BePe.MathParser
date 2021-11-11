using BePe.MathParser.Models;
using BePe.MathParserTests.TestCaseOrdering;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace BePe.MathParserTests.Models
{
    [TestCaseOrderer("BePe.MathParserTests.TestCaseOrdering.PriorityOrderer", "BePe.MathParserTests")]
    public class OperatorTests
    {
        [Fact, Priority(1)]
        public void NameCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Operator(null, 1, (a, b) => a + b);
            });
        }

        [Fact, Priority(1)]
        public void NameCantBeEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Operator(string.Empty, 1, (a, b) => a + b);
            });
        }

        [Fact, Priority(1)]
        public void PrecedenceCantBeNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Operator("+", -5, (a, b) => a + b);
            });
        }

        [Fact, Priority(1)]
        public void PrecedenceCantBeZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Operator("+", 0, (a, b) => a + b);
            });
        }

        [Fact, Priority(1)]
        public void OperationCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Operator("+", 1, null);
            });
        }

        [Fact, Priority(2)]
        public void NameReturnedAsIs()
        {
            Operator op = new("+", 1, (a, b) => a + b);
            Assert.Equal("+", op.Name);
        }

        [Fact, Priority(2)]
        public void PrecedenceReturnedAsIs()
        {
            Operator op = new("+", 1, (a, b) => a + b);
            Assert.Equal(1, op.Precedence);
        }

        [Fact, Priority(3)]
        public void LowerPrecedenceTest()
        {
            Operator add = new("+", 1, (a, b) => a + b);
            Operator multiply = new("*", 2, (a, b) => a * b);

            Assert.True(add.HasLowerPrecedenceThan(multiply));
        }

        [Fact, Priority(3)]
        public void SamePrecedenceTest()
        {
            Operator add = new("+", 1, (a, b) => a + b);
            Operator substract = new("-", 1, (a, b) => a - b);

            Assert.True(add.HasLowerPrecedenceThan(substract));
        }

        [Fact, Priority(3)]
        public void SamePrecedenceRightAssossiativeTest()
        {
            Operator power = new("^", 3, (a, b) => (int) Math.Pow(a, b), true);

            Assert.False(power.HasLowerPrecedenceThan(power));
        }

        [Fact, Priority(3)]
        public void HigherPrecedenceTest()
        {
            Operator add = new("+", 1, (a, b) => a + b);
            Operator multiply = new("*", 2, (a, b) => a * b);

            Assert.False(multiply.HasLowerPrecedenceThan(add));
        }

        [Fact, Priority(4)]
        public void CopyConstructorTest()
        {
            Operator expected = new("^", 3, (a, b) => (int) Math.Pow(a, b), true);
            Operator actual = new(expected);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Operation, actual.Operation);
            Assert.Equal(expected.Precedence, actual.Precedence);
            Assert.Equal(expected.RightAssociative, actual.RightAssociative);
        }

        [Fact, Priority(5)]
        public void DefaultDictIsReadOnly()
        {
            Assert.IsType<ReadOnlyDictionary<string, Operator>>(Operator.DefaultOperators);
        }
    }
}
