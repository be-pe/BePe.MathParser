using BePe.MathParser.Models;
using BePe.MathParserTests.TestCaseOrdering;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace BePe.MathParserTests.Models
{
    [TestCaseOrderer("BePe.MathParserTests.TestCaseOrdering.PriorityOrderer", "BePe.MathParserTests")]
    public class FunctionTests
    {
        [Fact, Priority(1)]
        public void NameCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Function(null, (p) => p[0] + p[1], 2);
            });
        }

        [Fact, Priority(1)]
        public void NameCantBeEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Function(string.Empty, (p) => p[0] + p[1], 2);
            });
        }

        [Fact, Priority(1)]
        public void NameCantBeWhitespace()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Function(" ", (p) => p[0] + p[1], 2);
            });
        }

        [Fact, Priority(1)]
        public void FunctionCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Function("add", null, 2);
            });
        }

        [Fact, Priority(1)]
        public void ParameterCountCantBeLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Function("add", (p) => p[0] + p[1], 0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Function("add", (p) => p[0] + p[1], -1);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Function("add", (p) => p[0] + p[1], int.MinValue);
            });
        }

        [Fact, Priority(2)]
        public void NameReturnedAsIs()
        {
            Function func = new("add", (p) => p[0] + p[1], 2);
            Assert.Equal("add", func.Name);
        }

        [Fact, Priority(2)]
        public void ParameterCountIsReturnedAsIs()
        {
            Function func = new("add", (p) => p[0] + p[1], 2);
            Assert.Equal(2, func.ParameterCount);
        }

        [Fact, Priority(3)]
        public void CopyConstructorTest()
        {
            Function expected = new("add", (p) => p[0] + p[1], 2);
            Function actual = new(expected);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Func, actual.Func);
            Assert.Equal(expected.ParameterCount, actual.ParameterCount);
        }

        [Fact, Priority(4)]
        public void DefaultDictIsReadOnly()
        {
            Assert.IsType<ReadOnlyDictionary<string, Function>>(Function.DefaultFunctions);
        }
    }
}
