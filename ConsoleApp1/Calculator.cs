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
            //�I�s�ۤv��@�����O�w�B�̦r��
            //�Y���\�Y�^�ǡA���ѫh�ߥX�ҥ~
            if (new Arithmetic().tryCalculate(input, out decimal result))
                return result;
            throw new ArgumentException("�榡���~�A���� [�Ʀr] [�B��Ÿ�(+-*/)] [�Ʀr]");
        }
    }

    #region Note
    //�]���@�P�ݨD�ܤơA�{����פw�L�k��������ݨD�A�G���c�Ӥ��
    //�Ӻt��k�Ѧۤv�c��B��@����
    #endregion

    internal class Arithmetic
    {
        private Queue<char> numbers;
        internal bool tryCalculate(string text, out decimal result)
        {
            result = 0M;
            decimal temp = 0M;

            //�B�z�Ӧr�꧹���L+-�����p
            if (!hasPlusOrMinus(text))
                return false;

            text = text.Trim();
            char[] inputArray = text.ToCharArray();
            this.numbers = new Queue<char>();
            bool isPlusOrMinus = true;
            bool mode_PlusAndMinus = false;
            bool mode_NumberIntactly = false;

            //�}�l�v�@���r��C�Ӥ���
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

            //�����̫�@�줸���A�Y���Q�i��Ʀr�h�ǤJ�B��
            if (!decimal.TryParse(popOutNumber(), out temp))
                return false;
            result += isPlusOrMinus ? temp : -temp;

            return true;
        }
        private bool hasPlusOrMinus(string text)
        {
            //�^�Ǧr��O�_�]�t+-��
            return text.Contains("+") || text.Contains("-");
        }
        private string popOutNumber()
        {
            //�N�r����C�̧Ǽu�X�A�ïǤJ�r���^��
            int count = this.numbers.Count;
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
                sb.Append(this.numbers.Dequeue());

            return sb.ToString();
        }
    }
}
