using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benzi_v2
{
    class Utils
    {
        public static int Power(int indexNumber, int exponentNumber)
        {
            int result = 1;
            for (int i = 0; i < exponentNumber; i++)
            {
                result = result * indexNumber;
            }

            return result;
        }

        public static int[] decimalToBits(int number)
        {
            int numberBits = (int)Math.Truncate(Math.Log(number, 2)) + 1;

            int[] bits = new int[16];

            for (int i = 0; i < numberBits; i++)
            {
                bits[15 - i] = number % 2;
                number = number / 2;
            }

            return bits;
        }

        public static int bitsToDecimal(int[] bits)
        {
            int number = 0;
            for (int j = 0; j <= 15; j++)
            {
                number += bits[j] * Utils.Power(2, (15 - j));
            }

            return number;
        }
    }
}
