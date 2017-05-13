using System;
using Expansion;


namespace AutomaticCalculationParameters
{
    /// <summary>
    /// Класс Program предназначен для вызова основных функций программы
    /// </summary>
    class Program
    {
        /// <summary>
        /// Метод Main начинает выполнение программы
        /// </summary>
        /// <param name="args">Входящие и выходящие параметры</param>
        static void Main(String[] args)
        {
            Double[] numFeatures = AreaFeatures.Data();
            NeuralNWGradientDescentReal(numFeatures);
            Console.WriteLine("Нажмите любую клавишу для завершения работы программы . . . ");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Метод NeuralNWGradientDescentRandom реализует обучение с помощью случайнных входных данных
        /// нейроной сети методом градиентного спуска
        /// </summary>
        static void NeuralNWGradientDescentRandom() => Print.NeuralNWGradientDescentData(30, 100000, 1);

        /// <summary>
        /// Метод NeuralNWGradientDescentRandom реализует обучение с помощью реальнных входных данных
        /// нейроной сети методом градиентного спуска
        /// </summary>
        /// <param name="numFeatures"></param>
        static void NeuralNWGradientDescentReal(Double[] numFeatures) => Print.NeuralNWGradientDescentRealData(numFeatures, 1000, 1);
    }
}