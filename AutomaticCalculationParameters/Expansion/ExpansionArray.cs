using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

namespace Expansion
{
    /// <summary>
    /// Класс ExpansionArray предоставляет методы расширения для работы с массивами
    /// </summary>
    public static class ExpansionArray
    {
        /// <summary>
        /// Метод IntputArray позволяет ввести целочисленные элементы массива Int32 с клавиатуры
        /// </summary>
        /// <param name="intArray">Ссылка на экземпляр массива</param>
        /// <returns>Возращает заполненный массив целыми числами Int32</returns>
        public static Int32[] IntputArray(this Int32[] intArray)
        {
            Console.Write("Укажите размер массива: ");
            Int32 n = Convert.ToInt32(Console.ReadLine());
            intArray = new Int32[n];
            for (Int32 i = 0; i < intArray.Count(); i++)
            {
                Console.Write($"IntArray [{i}]: ");
                intArray[i] = Convert.ToInt32(Console.ReadLine());
            }
            return intArray;
        }

        /// <summary>
        /// Метод IntputArray позволяет ввести целочисленные элементы массива Int64 с клавиатуры
        /// </summary>
        /// <param name="intArray">Ссылка на экземпляр массива</param>
        /// <returns>Возращает заполненный массив целыми числами Int64</returns>
        public static Int64[] IntputArray(this Int64[] intArray)
        {
            Console.Write("Укажите размер массива: ");
            Int32 n = Convert.ToInt32(Console.ReadLine());
            intArray = new Int64[n];
            for (Int32 i = 0; i < intArray.Count(); i++)
            {
                Console.Write($"IntArray [{i}]: ");
                intArray[i] = Convert.ToInt64(Console.ReadLine());
            }
            return intArray;
        }

        /// <summary>
        /// Метод IntputArray позволяет ввести вещественные элементы массива Double с клавиатуры
        /// </summary>
        /// <param name="doubleArray">Ссылка на экземпляр массива</param>
        /// <returns>Возращает заполненный массив целыми числами Double</returns>
        public static Double[] IntputArray(this Double[] doubleArray)
        {
            Console.Write("Укажите размер массива: ");
            Int32 n = Convert.ToInt32(Console.ReadLine());
            doubleArray = new Double[n];
            for (Int32 i = 0; i < doubleArray.Count(); i++)
            {
                Console.Write($"IntArray [{i}]: ");
                doubleArray[i] = Convert.ToDouble(Console.ReadLine());
            }
            return doubleArray;
        }

        /// <summary>
        /// Метод IntputArray позволяет ввести вещественные элементы массива Single с клавиатуры
        /// </summary>
        /// <param name="singleArray">Ссылка на экземпляр массива</param>
        /// <returns>Возращает заполненный массив целыми числами Single</returns>
        public static Single[] IntputArray(this Single[] singleArray)
        {
            Console.Write("Укажите размер массива: ");
            Int32 n = Convert.ToInt32(Console.ReadLine());
            singleArray = new Single[n];
            for (Int32 i = 0; i < singleArray.Count(); i++)
            {
                Console.Write($"IntArray [{i}]: ");
                singleArray[i] = Convert.ToSingle(Console.ReadLine());
            }
            return singleArray;
        }

        /// <summary>
        /// Метод Print<T> выводит элементы массива на экран
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="array">Ссылка на экземпляр массива</param>
        public static void Print<T>(this T[] array)
        {
            foreach (T ar in array)
            {
                Console.Write($"{ar}" + " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Метод Print<T> выводит элементы массива на экран
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="array">Ссылка на экземпляр массива</param>
        public static void Print<T>(this IEnumerable<T> array)
        {
            foreach (var ar in array)
            {
                Console.Write($"{ar}" + " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Метод ShortQuickSort<T> выполняет сортировку по алгоритму быстрой сортировки
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="list">Ссылка на экземпляр массива</param>
        /// <returns>Возращает отсортированный массив элементов</returns>
        public static IEnumerable<T> ShortQuickSort<T>(this IEnumerable<T> array) where T : IComparable<T> =>
            !array.Any() ? Empty<T>() : array.Skip(1).Where(item => item.CompareTo(array.First()) <= 0).
                ShortQuickSort().Concat(new[] { array.First() }).Concat(array.Skip(1).Where(item => item.
                    CompareTo(array.First()) > 0).ShortQuickSort());
        
    }
}