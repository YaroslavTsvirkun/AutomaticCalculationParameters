using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expansion
{
    /// <summary>
    /// Класс ExpansionString предоставляет методы расширения для работы со строками
    /// </summary>
    public static class ExpansionString
    {
        /// <summary>
        /// Метод ReversStr перестраивает строку в обратном порядке
        /// </summary>
        /// <param name="str">Ссылка на экземпляр строки</param>
        /// <returns>Возращает строку в обратном порядке</returns>
        public static String ReversStr(this String str) => new string(str.ToCharArray().Reverse().ToArray());

        /// <summary>
        /// Метод GetCharLength ищет самую длиную последовательность символов в строке
        /// </summary>
        /// <param name="line">Ссылка на экземпляр строки</param>
        /// <returns></returns>
        public static IEnumerable<Int32> GetCharLength(this String str)
        {
            for (Int32 i = 0, n = 1; i < str.Count() - 1; i++)
            {
                if (str[i] == str[i + 1]) n++;
                else
                {
                    yield return n;
                    n = 1;
                }
                if (i == str.Length - 2) yield return n;
            }
        }

        /// <summary>
        /// Метод читает по указаному пути текстовфй файл с числами в виде строки, 
        /// и преобразует его в массив чисел с плавающей точкой двойной точности
        /// </summary>
        /// <param name="address">Адресс текстового файла</param>
        /// <returns>Возращает массив чисел с плавающей точкой двойной точности</returns>
        public static Double[] GetFileStringToDouble(String address)
        {
            String text = "";
            Char[] symbol = { ' ' };
            try
            {
                using (StreamReader fs = new StreamReader(address))
                {
                    while(true)
                    {
                        String temp = fs.ReadLine();
                        if (temp == null) break;
                        text += temp;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            String[] dataString = text.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            Double[] dataDouble = new Double[dataString.Count()];
            for (Int32 i = 0; i < dataString.Count(); i++)
            {
                try
                {
                    dataDouble[i] = Convert.ToDouble(dataString[i]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return dataDouble;
        }
    }
}