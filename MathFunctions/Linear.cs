using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunctions
{
    /// <summary>
    /// Общий вид: F(x) = kx + b
    /// </summary>
    public class Linear : OneArgument
    {
        /// <summary>
        /// коэффициенты
        /// </summary>
        private double k;
        private double b;

        public Linear()
        {
            base.name = "Линейная функция";
        }

        public override double GetParam(int order)
        {
            if (order > 1)
            {
                throw new Exception("Такого параметра нет");
            }

            if(order == 0)
            {
                return k;
            }
            else
            {
                return b;
            }
        }

        public override void Calculate(double x)
        {
            base.argument = x;
            base.value = k * x + b;
        }

        public override void Parameters(params double[] coefficients)
        {
            int length = coefficients.Length;
            
            if (length > 3 || length <= 0)
            {
                throw new Exception("Неверное число параметров!");
            }

            k = coefficients[0];
            b = length == 2 ? coefficients[1] : 0;
        }

        public override string Show()
        {
            if(k == 0 && b == 0)
            {
                return "F(x) = 0";
            }
            return "F(x) = " + (k != 0 ? k + "x " : "")
                             + (b != 0 ? (b > 0 ? "+ " : "") + b.ToString().Replace("-", "- ") : "");
        }
    }
}
