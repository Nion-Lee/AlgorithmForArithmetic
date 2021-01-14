using System;
using ConsoleApp1;
using Shouldly;
using Xunit;

namespace TestProject1
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData("3+1", 4)]
        [InlineData("5 + 2", 7)]
        [InlineData("5 +2", 7)]
        [InlineData("5+ 2", 7)]
        [InlineData("    5+2    ", 7)]
        [InlineData("20000+13413", 33413)]
        public void 整數加法_應正確計算(string formula, decimal expected)
        {
            var calculator = new Calculator();
            calculator.Calculate(formula).ShouldBe(expected);
        }

        [Theory]
        [InlineData("3-1", 2)]
        [InlineData("5 - 2", 3)]
        [InlineData("20000-13413", 6587)]
        public void 整數減法_應正確計算(string formula, decimal expected)
        {
            var calculator = new Calculator();
            calculator.Calculate(formula).ShouldBe(expected);
        }
        
        [Theory]
        [InlineData("3-1 + 5", 7)]
        [InlineData("5 - 2 + 3 - 5 ", 1)]
        [InlineData("6 + 3 + 2 + 1 + 5 - 3 - 5", 9)]
        [InlineData("1+2+3+4+5", 15)]
        public void 多項加減法_應正確計算(string formula, decimal expected)
        {
            var calculator = new Calculator();
            calculator.Calculate(formula).ShouldBe(expected);
        }

        [Theory]
        [InlineData("3.3+1.8", 5.1)]
        [InlineData("5.2 + 2.2", 7.4)]
        [InlineData("5.1 +2.9 - 3.1 + 5.2", 10.1)]
        public void 含小數數加法_應正確計算(string formula, decimal expected)
        {
            var calculator = new Calculator();
            calculator.Calculate(formula).ShouldBe(expected);
        }

        [Theory]
        [InlineData("3.9-1.1", 2.8)]
        [InlineData("5.2 - 2.3", 2.9)]
        [InlineData("3.5 - 2.3 + 1.1-2.5-3.9-1.3", -5.4)]
        public void 含小數數減法_應正確計算(string formula, decimal expected)
        {
            var calculator = new Calculator();
            calculator.Calculate(formula).ShouldBe(expected);
        }

        [Theory]
        [InlineData("a3-1")]
        [InlineData("5 +- 2")]
        [InlineData("2 2 3 - 3 2 3 1")]
        [InlineData("12345")]
        [InlineData("1+2a")]
        [InlineData("1+3 2")]
        [InlineData("12 3 - 5")]
        [InlineData("+12")]
        [InlineData("12--2")]
        [InlineData("12--2+2 - 5 --3")]
        [InlineData("12-2+2 +- 5 -3")]
        [InlineData("12-2-5.2.3 -3")]
        [InlineData("12-2-5..012 -3")]
        public void 輸入格式錯誤_應拋出例外(string formula)
        {
            var calculator = new Calculator();
            Should.Throw<ArgumentException>(() => { calculator.Calculate(formula); })
                .Message.ShouldContain("格式錯誤，應為 [數字] [運算符號(+-*/)] [數字]");
        }
    }
}
