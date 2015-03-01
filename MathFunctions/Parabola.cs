using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunctions
{
    /// <summary>
    /// Общий вид: F(x) = ax^2 + bx + c
    /// </summary>
    public class Parabola : OneArgument
    {
        /// <summary>
        /// коэффициенты
        /// </summary>
        private double a;
        private double b;
        private double c;

        public Parabola()
        {
            base.name = "Параболическая функция";
        }

        public override double GetParam(int order)
        {
            if (order > 2)
            {
                throw new Exception("Такого параметра нет");
            }

            if (order == 0)
            {
                return a;
            }
            else if(order == 1)
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        public override void Calculate(double x)
        {
            base.argument = x;
            base.value = a * x * x + b * x + c;
        }

        public override void Parameters(params double[] coefficients)
        {
            int length = coefficients.Length;

            if (length > 3 || length <= 0)
            {
                throw new Exception("Неверное число параметров!");
            }

            a = coefficients[0];
            b = length > 1 ? coefficients[1] : 0;
            c = length > 2 ? coefficients[2] : 0;
        }

        public override string Show()
        {
            if (a == 0 && b == 0 && c == 0)
            {
                return "F(x) = 0";
            }
            return "F(x) = " + (a != 0 ? a + "x^2 " : "")
                             + (b != 0 ? (b > 0 ? "+ " : "") + b.ToString().Replace("-", "- ") + "x " : "")
                             + (c != 0 ? (c > 0 ? "+ " : "") + c.ToString().Replace("-", "- ") : "");
        }
    }
}
