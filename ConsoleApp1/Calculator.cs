using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class Calculator
    {
        public decimal Calculate(string input)
        {
            //呼叫自己實作之類別庫處裡字串
            //若成功即回傳，失敗則拋出例外
            if (new Arithmetic().tryCalculate(input, out decimal result))
                return result;
            throw new ArgumentException("格式錯誤，應為 [數字] [運算符號(+-*/)] [數字]");
        }
    }

    #region Note
    //因應劇烈需求變化，現有方案已無法完善應對需求，故重構該方案
    //該演算法由自己構思、實作完成
    #endregion

    internal class Arithmetic
    {
        private Queue<char> numbers;
        internal bool tryCalculate(string text, out decimal result)
        {
            result = 0M;
            decimal temp = 0M;

            //處理該字串完全無+-號情況
            if (!hasPlusOrMinus(text))
                return false;

            text = text.Trim();
            char[] inputArray = text.ToCharArray();
            this.numbers = new Queue<char>();
            bool isPlusOrMinus = true;
            bool mode_PlusAndMinus = false;
            bool mode_NumberIntactly = false;

            //開始逐一比對字串每個元素
            for (int i = 0; i < text.Length; i++)
            {
                switch (inputArray[i])
                {
                    case ' ':
                        this.numbers.Enqueue(' ');
                        break;

                    case '.':
                        if (!mode_NumberIntactly || mode_PlusAndMinus)
                            return false;
                        this.numbers.Enqueue('.');
                        break;

                    case '+':
                        if (!mode_NumberIntactly)
                            return false;
                        if (!decimal.TryParse(popOutNumber(), out temp))
                            return false;
                        result += isPlusOrMinus ? temp : -temp;

                        isPlusOrMinus = true;
                        mode_PlusAndMinus = true;
                        mode_NumberIntactly = false;
                        break;

                    case '-':
                        if (!mode_NumberIntactly)
                            return false;
                        if (!decimal.TryParse(popOutNumber(), out temp))
                            return false;
                        result += isPlusOrMinus ? temp : -temp;

                        isPlusOrMinus = false;
                        mode_PlusAndMinus = true;
                        mode_NumberIntactly = false;
                        break;

                    case char c when char.IsDigit(c):
                        this.numbers.Enqueue(c);
                        mode_PlusAndMinus = false;
                        mode_NumberIntactly = true;
                        break;

                    default:
                        return false;
                }
            } 

            //捕捉最後一位元素，若為十進位數字則納入運算
            if (!decimal.TryParse(popOutNumber(), out temp))
                return false;
            result += isPlusOrMinus ? temp : -temp;

            return true;
        }
        private bool hasPlusOrMinus(string text)
        {
            //回傳字串是否包含+-號
            return text.Contains("+") || text.Contains("-");
        }
        private string popOutNumber()
        {
            //將字元佇列依序彈出，並納入字串後回傳
            int count = this.numbers.Count;
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
                sb.Append(this.numbers.Dequeue());

            return sb.ToString();
        }
    }
}
