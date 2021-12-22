using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RegistrationForm
{
    public class CaptchaCode
    {
        int number1;
        int number2;
        char[] operators = new char[] { '+', '-', '*', '/' };
        char sign;
        Random random;

        public CaptchaCode()
        {
            random = new Random();
        }

        public override string ToString()
        {
            sign = operators[random.Next(0, 4)];
            number1 = random.Next(1, 10);
            number2 = random.Next(1, 10);
            if (sign == operators[3])
            {
                while (number1 % number2 != 0)
                {
                    number1++;
                }
            }
            return number1.ToString() + sign.ToString() + number2.ToString();
        }

    }
}
