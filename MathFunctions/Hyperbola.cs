using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunctions
{
    /// <summary>
    /// Общий вид: F(x) = k / x
    /// </summary>
    public class Hyperbola : OneArgument
    {
        /// <summary>
        /// Коэффициенты
        /// </summary>
        private double k;

        public Hyperbola()
        {
            base.name = "Гиперболическая функция";
        }

        public override double GetParam(int order)
        {
            if(order > 0)
            {
                throw new Exception("Такого параметра нет");
            }
            return k;
        }

        public override void Calculate(double x)
        {
            if (x == 0) 
            {
                throw new Exception("Делить на ноль нельзя");
            }
            base.argument = x;
            base.value = k / x;
        }

        public override void Parameters(params double[] coefficients)
        {
            int length = coefficients.Length;

            if (length > 3 || length <= 0)
            {
                throw new Exception("Неверное число параметров!");
            }

            if(coefficients[0] == 0)
            {
                throw new Exception("Функция не является гиперболой при k = 0");
            }

            k = coefficients[0];
        }

        public override string Show()
        {
            return "F(x) = " + k + "/x";
        }
    }
}
