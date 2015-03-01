using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathFunctions;

namespace WindowsFormsApplication
{
    public class FunctionManager
    {
        /// <summary>
        /// Контейнер математических функций
        /// </summary>
        public List<OneArgument> MathFunctions = new List<OneArgument>();

        const string errorFormat = "Неправильный формат данных!";

        public void CreateFile(string filename)
        {
            MathFunctions = new List<OneArgument>();
            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            file.Close();
        }

        public void OpenFile(string filename)
        {
            if(!File.Exists(filename))
            {
                throw new Exception("Файла не существует");
            }

            MathFunctions = new List<OneArgument>();
            StreamReader reader = new StreamReader(filename, Encoding.UTF8);
            string line;
            
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split('|');
                
                if (fields.Length <= 1)
                {
                    throw new Exception("Файл имеет недопустимое содержимое!");
                }
                
                string type = fields[0];
                double argument = double.Parse(fields[1]);

                OneArgument function;

                if(type == "Параболическая функция")
                {
                    if(fields.Length < 4)
                    {
                        throw new Exception(errorFormat);
                    }

                    function = new Parabola();
                    try
                    {
                        double a = double.Parse(fields[2]);
                        double b = double.Parse(fields[3]);
                        double c = double.Parse(fields[4]);
                        function.Parameters(a, b, c);
                        function.Calculate(argument);
                    }
                    catch { throw new Exception(errorFormat); }
                } 
                else if(type == "Линейная функция")
                {
                    if (fields.Length < 3)
                    {
                        throw new Exception(errorFormat);
                    }

                    try
                    {
                        function = new Linear();
                        double k = double.Parse(fields[2]);
                        double b = double.Parse(fields[3]);
                        function.Parameters(k, b);
                        function.Calculate(argument);
                    }
                    catch { throw new Exception(errorFormat); }
                } 
                else
                {
                    if (fields.Length < 2)
                    {
                        throw new Exception(errorFormat);
                    }

                    try
                    {
                        function = new Hyperbola();
                        double k = double.Parse(fields[2]);
                        function.Parameters(k);
                        function.Calculate(argument);
                    }
                    catch { throw new Exception(errorFormat); }
                }
                MathFunctions.Add(function);
            }
            reader.Close();
        }

        public void SaveFile(string filename)
        {
            int count = MathFunctions.Count;
            
            StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8);

            for(int i = 0; i < count; i++)
            {
                OneArgument function = MathFunctions[i];
                string type = function.name;
                string argument = function.argument.ToString();

                if (function is Parabola)
                {
                    string a = function.GetParam(0).ToString();
                    string b = function.GetParam(1).ToString();
                    string c = function.GetParam(2).ToString();
                    writer.WriteLine(type + "|" + argument + "|" + a + "|" + b + "|" + c);
                }
                else if(function is Linear)
                {
                    string k = function.GetParam(0).ToString();
                    string b = function.GetParam(1).ToString();
                    writer.WriteLine(type + "|" + argument + "|" + k + "|" + b);
                }
                else
                {
                    string k = function.GetParam(0).ToString();
                    writer.WriteLine(type + "|" + argument + "|" + k);
                }
            }
            writer.Close();
        }

        public void SaveFileAs(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            file.Close();
            SaveFile(filename);
        }

        public int getNumParams(int index)
        {
            switch (index)
            {
                case 0:
                    return 2;
                case 1:
                    return 3;
                default:
                    return 1;
            }
        }

        public void SortByValue()
        {
            //Проверяем отсортирована ли коллекция
            bool isSortedAsc = MathFunctions.SequenceEqual(MathFunctions.OrderBy(x => x.value));
            if (!isSortedAsc)
                MathFunctions = MathFunctions.OrderBy(x => x.value).ToList();
            else
                MathFunctions = MathFunctions.OrderByDescending(x => x.value).ToList();
        }
    }
}
