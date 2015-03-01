using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunctions
{
    public abstract class OneArgument
    {
        /// <summary>
        /// Тип функции
        /// </summary>
        public string name
        {
            protected set;
            get;
        }

        /// <summary>
        /// Аргумент функции
        /// </summary>
        public double argument
        {
            protected set;
            get;
        }

        /// <summary>
        /// Значение функции
        /// </summary>
        public double value
        {
            protected set;
            get;
        }
        
        /// <summary>
        /// Получить параметр
        /// </summary>
        /// <param name="x"></param>
        /// <returns>возвращает значение параметра</returns>
        public abstract double GetParam(int order);

        /// <summary>
        /// Вычисление значения функции
        /// </summary>
        /// <param name="x"></param>
        public abstract void Calculate(double x);

        /// <summary>
        /// Задание параметров
        /// </summary>
        /// <param name="coefficients"></param>
        public abstract void Parameters(params double[] coefficients);

        /// <summary>
        /// Представление записи функции
        /// </summary>
        /// <returns></returns>
        public abstract string Show();
    }
}
