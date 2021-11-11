using BePe.MathParser;
using BePe.MathParser.Exceptions;
using Xunit;

namespace BePe.MathParserTests
{
    public class ShuntingYardParserTests
    {
        [Theory]
        [MemberData(nameof(ValidExpressions))]
        public void DefaultParser(string input, int expected)
        {
            ShuntingYardParser parser = new();
            int actual = parser.Parse(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<string, int> ValidExpressions()
        {
            TheoryData<string, int> td = new();

            td.Add("1+2", 1 + 2);
            td.Add("1-2", 1 - 2);
            td.Add("5*2", 5 * 2);
            td.Add("4/2", 4 / 2);
            td.Add("5/2", 5 / 2);
            td.Add("(5+2)*5", (5 + 2) * 5);
            td.Add("2^2", 4);
            td.Add("2^2^2^2", 65536);
            td.Add("2^(2^(2^2))", 65536);
            td.Add("1+2^2^2^2", 65537);
            td.Add("max(10,5)", 10);
            td.Add("max(10,5*5)", 25);
            td.Add("abs(5-7)", 2);

            return td;
        }

        [Theory]
        [MemberData(nameof(InvalidExpressions))]
        public void AssertThrows(string input, Type expected)
        {
            ShuntingYardParser parser = new();
            Assert.Throws(expected, () => parser.Parse(input));
        }

        public static TheoryData<string, Type> InvalidExpressions()
        {
            TheoryData<string, Type> td = new();

            td.Add("(6+1", typeof(InvalidExpressionException));
            td.Add("6+1)", typeof(InvalidExpressionException));
            td.Add("5/0", typeof(DivideByZeroException));
            td.Add("-5", typeof(InvalidExpressionException));
            td.Add("max(2,5,7)", typeof(InvalidExpressionException));
            td.Add("test(6)", typeof(InvalidTokenException));
            td.Add("6=6", typeof(InvalidTokenException));

            return td;
        }

        [Fact]
        public void CantUseVariableWithoutSettingAFunction()
        {
            ShuntingYardParser parser = new();
            Assert.Throws<InvalidOperationException>(() => parser.Parse("WIS"));
        }

        [Fact]
        public void VariableThrowIsntCaught()
        {
            static int GetVariable(string name)
            {
                string upperName = name.ToUpper();
                return upperName switch
                {
                    "STR" => 10,
                    "DEX" => 12,
                    "INT" => 16,
                    _ => throw new KeyNotFoundException($"{name} is not set."),
                };
            }
            ShuntingYardParser parser = new(GetVariable);
            Assert.Throws<KeyNotFoundException>(() => parser.Parse("WIS"));
        }

        [Theory]
        [MemberData(nameof(VariableExpressions))]
        public void VariableTests(string input, int expected)
        {
            static int GetVariable(string name)
            {
                string upperName = name.ToUpper();
                return upperName switch
                {
                    "STR" => 10,
                    "DEX" => 12,
                    "INT" => 16,
                    _ => throw new KeyNotFoundException($"{name} is not set."),
                };
            }
            ShuntingYardParser parser = new(GetVariable);
            Assert.Equal(expected, parser.Parse(input));
        }

        public static TheoryData<string, int> VariableExpressions()
        {
            TheoryData<string, int> td = new();

            td.Add("STR*2", 20);
            td.Add("max(STR, INT)", 16);

            return td;
        }
    }
}