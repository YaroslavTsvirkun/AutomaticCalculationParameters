using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Expansion
{
    delegate bool deg(Int32 x);

    /// <summary>
    /// Класс ExpansionInt32 предоставляет методы расширения для работы с Int32
    /// </summary>
    public static class ExpansionInt32
    {
        /// <summary>
        /// Метод Generic генерирует последовательность целых чисел в заданном диапазоне и заносит их в массив
        /// </summary>
        /// <param name="start">Значение первого числа последовательности</param>
        /// <param name="end">Значение последнего числа последовательности</param>
        /// <returns></returns>
        public static Int32[] Generic(Int32 start, Int32 end) => Enumerable.Range(start, end).ToArray();

        /// <summary>
        /// Метод AnalysisNumbersComponents раскладывает число на составляющие
        /// </summary>
        /// <param name="number">Ссылка на экземпляр числа</param>
        public static void AnalysisNumbersComponents(this Int32 number)
        {
            String numbers = number.ToString();
            Char[] num = numbers.ToCharArray();
            for (Int32 i = 0; i < num.Count(); i++)
            {
                Console.Write($"{num[i]}" + " ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод IntSearchNumbers ищет в числе искомые цифры
        /// </summary>
        /// <param name="number">Ссылка на экземпляр числа</param>
        /// <param name="seeking">Ссылка на экземпляр искомых значений</param>
        /// <returns>Возращает лист найденных значений</returns>
        public static List<Int32> IntSearchNumbers(this Int32 number, Int32[] seeking)
        {
            List<Int32> arr = new List<Int32>();
            String numbers = number.ToString();
            Char[] num = numbers.ToCharArray();
            for (Int32 i = 0; i < num.Count(); i++)
            {
                for (Int32 j = 0; j < seeking.Count(); j++)
                {
                    if (num[i] == seeking[j])
                    {
                        arr.Add(num[i]);
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// Метод Sum вычисляет сумму элементов массива
        /// </summary>
        /// <param name="array">Ссылка на экземпляр массива</param>
        /// <returns></returns>
        public static Int32 Sum(this Int32[] array)
        {
            var sum = 0;
            for (Int32 i = 0; i < array.Count(); i++) sum += array[i];
            return sum;
        }

        /// <summary>
        /// Метод EvenNumbers определяет все ли четные элементы массива
        /// </summary>
        /// <param name="array">Ссылка на экземпляр</param>
        /// <returns>Возращает все четные элементы массива</returns>
        public static List<Int32> EvenNumbers(this Int32[] array)
        {
            deg t = n => n % 2 == 0;
            List<Int32> arr = new List<Int32>();
            for (Int32 i = 0; i < array.Count(); i++)
            {
                if (t(array[i]) && array[i] > 0) arr.Add(array[i]);
            }
            return arr;
        }

        /// <summary>
        /// Метод Composition вычисляет произведение элементов массива
        /// </summary>
        /// <param name="array">Исходный массив</param>
        /// <returns>Возращает произведение чисел массива</returns>
        public static BigInteger Composition(this Int32[] array)
        {
            BigInteger big = new BigInteger(1);
            for (Int32 i = 0; i < array.Count(); i++) big *= array[i];
            return big;
        }
    }
}