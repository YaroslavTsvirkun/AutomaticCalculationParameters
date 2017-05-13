using System;
using System.Linq;
using static System.Math;
using System.ComponentModel;

namespace Expansion
{
    class LogisticGradient : Component
    {
        /// <summary>
        /// Метод MakeAllData генерирует случайные весовые значения, 
        /// а затем итеративно генерирует случайные входные значения, 
        /// комбинирует весовые и входные значения, используя функцию логистического сигмоида, 
        /// и вычисляет соответствующее выходное значение
        /// </summary>
        /// <param name="numFeatures">Количество весов</param>
        /// <param name="numRows">Входные значения</param>
        /// <param name="seed">Начальное значение генератора случайных чисел</param>
        /// <returns></returns>
        internal static Double[][] MakeAllData(Int32 numFeatures, Int32 numRows, Int32 seed)
        {
            Random rnd = new Random(seed);
            Double[] weights = new Double[numFeatures + 1];
            for (Int32 i = 0; i < weights.Length; ++i)
            {
                weights[i] = 20.0 * rnd.NextDouble() - 10.0; // [10;-10], можно задать вручную!
            }
            Double[][] result = new Double[numRows][];
            for (Int32 i = 0; i < numRows; ++i)
            {
                result[i] = new Double[numFeatures + 1];
            }

            for (Int32 i = 0; i < numRows; ++i)
            {
                Double z = weights[0]; 
                for (Int32 j = 0; j < numFeatures; ++j)
                {
                    Double x = 20.0 * rnd.NextDouble() - 10.0; // [10;-10]
                    result[i][j] = x;
                    Double wx = x * weights[j + 1];
                    z += wx;
                }
                Double y = 1.0 / (1.0 + Exp(-z));
                if (y > 0.55)
                {
                    result[i][numFeatures] = 1.0;
                }
                else
                {
                    result[i][numFeatures] = 0.0;
                }
            }
            ShowVector(weights, 4, true);
            return result;
        }

        internal static Double[][] MakeAllData(Double[] numFeatures, Int32 numRows, Int32 seed)
        {
            Double maxValue = numFeatures.Cast<Double>().Max();
            Random rnd = new Random(seed);
            Double[] weights = new Double[numFeatures.Count() + 1];
            for (Int32 i = 0; i < weights.Length; ++i)
            {
                weights[i] = 20 * rnd.NextDouble() - 10; // [maxValue; minValue]
            }
            Double[][] result = new Double[numRows][];
            for (Int32 i = 0; i < numRows; ++i)
            {
                result[i] = new Double[numFeatures.Count()];
            }
            for (Int32 i = 0; i < numRows; ++i)
            {
                Double z = weights[0];
                try
                {
                    for (Int32 j = 0; j < numFeatures.Count(); ++j)
                    {
                        Double x = 20 * rnd.NextDouble() - 10; // [maxValue; minValue]
                        result[i][j] = x;
                        Double wx = x * weights[j + 1];
                        z += wx;
                    }
                }
                catch(IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
                Double y = 1 / (1 + Exp(-z));
                if (y > 0.55)
                {
                    result[i][numFeatures.Count() - 1] = 1.0;
                }
                else
                {
                    result[i][numFeatures.Count() - 1] = 0.0;
                }
            }
            ShowVector(weights, 4, true);
            return result;
        }

        /// <summary>
        /// Метод MakeTrainTest разбивает данных на обучающий и 
        /// тестовый наборы в соотношении 80% к 20%
        /// </summary>
        /// <param name="allData"></param>
        /// <param name="seed"></param>
        /// <param name="trainData">Обучающие данные</param>
        /// <param name="testData"></param>
        public static void MakeTrainTest(Double[][] allData, Int32 seed, out Double[][] trainData, out Double[][] testData)
        {
            Random rnd = new Random(seed);
            Int32 totRows = allData.Length;
            Int32 numTrainRows = (Int32)(totRows * 0.80);
            Int32 numTestRows = totRows - numTrainRows;
            trainData = new Double[numTrainRows][];
            testData = new Double[numTestRows][];
            Double[][] copy = new Double[allData.Length][];
            for (Int32 i = 0; i < copy.Length; ++i)
            {
                copy[i] = allData[i];
            }
            for (Int32 i = 0; i < copy.Length; ++i)
            {
                Int32 r = rnd.Next(i, copy.Length);
                Double[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }
            for (Int32 i = 0; i < numTrainRows; ++i)
            {
                trainData[i] = copy[i];
            }
            for (Int32 i = 0; i < numTestRows; ++i)
            {
                testData[i] = copy[i + numTrainRows];
            }
        }

        /// <summary>
        /// Метод ShowData выводит входные данные на экран
        /// </summary>
        /// <param name="data">Входные данные</param>
        /// <param name="numRows"></param>
        /// <param name="decimals"></param>
        /// <param name="indices"></param>
        public static void ShowData(Double[][] data, Int32 numRows, Int32 decimals, Boolean indices)
        {
            int len = data.Length.ToString().Length;
            for (Int32 i = 0; i < numRows; ++i)
            {
                if (indices == true)
                {
                    Console.Write("[" + i.ToString().PadLeft(len) + "]  ");
                }    
                for (Int32 j = 0; j < data[i].Length; ++j)
                {
                    Double v = data[i][j];
                    if (v >= 0.0) Console.Write(" ");
                    Console.Write(v.ToString("F" + decimals) + "  ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine(". . .");
            int lastRow = data.Length - 1;
            if (indices == true)
            {
                Console.Write("[" + lastRow.ToString().PadLeft(len) + "]  ");
            }   
            for (Int32 j = 0; j < data[lastRow].Length; ++j)
            {
                Double v = data[lastRow][j];
                if (v >= 0.0)
                {
                    Console.Write(" ");
                }    
                Console.Write(v.ToString("F" + decimals) + "  ");
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Метод ShowVector выводит выходные данные на экран
        /// </summary>
        /// <param name="vector">Выходные данные</param>
        /// <param name="decimals"></param>
        /// <param name="newLine"></param>
        public static void ShowVector(Double[] vector, Int32 decimals, Boolean newLine)
        {
            for (Int32 i = 0; i < vector.Length; ++i)
            {
                Console.Write(vector[i].ToString("F" + decimals) + " ");
            }
            Console.WriteLine("");
            if (newLine == true)
            {
                Console.WriteLine("");
            }
        }

    }
    /// <summary>
    /// Класс LogisticClassifier предоставляет алгоритм для обучения 
    /// по методу градиентного спуска
    /// </summary>
    public class LogisticClassifier : Component
    {
        /// <summary>
        /// Переменная задающая число переменных для обучения
        /// </summary>
        private Int32 numFeaturesInt32;
        /// <summary>
        /// Массив задающий количество переменных для обучения
        /// </summary>
        private Double[] numFeaturesDouble;
        /// <summary>
        /// Массив - константа для записи случайнных весовых коефициентов
        /// </summary>
        private Double[] weights;
        /// <summary>
        /// Генератор случайнных чисел
        /// </summary>
        private Random rnd;

        /// <summary>
        /// Конструктор класса LogisticClassifier для инициализации числа переменных
        /// </summary>
        /// <param name="numFeaturesInt32">Переменная задающая число переменных</param>
        public LogisticClassifier(Int32 numFeaturesInt32)
        {
            this.numFeaturesInt32 = numFeaturesInt32;
            weights = new Double[numFeaturesInt32 + 1];
            rnd = new Random(0);
            for (Int32 i = 0; i < weights.Count(); ++i)
            {
                weights[i] = 0.01 * rnd.NextDouble();
            }
        }
        /// <summary>
        /// Конструктор класса LogisticClassifier для инициализации числа переменных
        /// </summary>
        /// <param name="numFeaturesDouble">Массив задающий число переменных</param>
        public LogisticClassifier(Double[] numFeaturesDouble)
        {
            this.numFeaturesDouble = numFeaturesDouble;
            weights = numFeaturesDouble;
            rnd = new Random(0);
            for (Int32 i = 0; i < weights.Count(); ++i)
            {
                weights[i] = 0.01 * rnd.NextDouble();
            }
        }

        /// <summary>
        /// Метод Train реализует обучения по методу градиентного спуска
        /// </summary>
        /// <param name="trainData">Выборка данных для обучения модели</param>
        /// <param name="maxEpochs">Максимальная ошибка</param>
        /// <param name="alpha">Скорость обучения</param>
        /// <returns></returns>
        public Double[] Train(Double[][] trainData, Int32 maxEpochs, Double alpha)
        {
            // скорость обучения
            Int32 epoch = 0;
            // случайнный порядок
            Int32[] sequence = new Int32[trainData.Count()]; 
            for (Int32 i = 0; i < sequence.Count(); ++i) sequence[i] = i;
            while (epoch < maxEpochs)
            {
                ++epoch;
                if (epoch % 100 == 0 && epoch != maxEpochs)
                {
                    Double mse = Error(trainData, weights);
                }
                Shuffle(sequence); // обработка данных в случайном порядке
                // Стохастический или поэтапный подход
                for (Int32 ti = 0; ti < trainData.Count(); ++ti)
                {
                    Int32 i = sequence[ti];
                    Double computed = ComputeOutput(trainData[i], weights);
                    Int32 targetIndex = trainData[i].Count() - 1;
                    Double target = trainData[i][targetIndex];
                    // вес b0 имеет фиктивный 1 вход
                    weights[0] += alpha * (target - computed) * 1; 
                    for (Int32 j = 1; j < weights.Count(); ++j)
                    {
                        weights[j] += alpha * (target - computed) * trainData[i][j - 1];
                    }   
                }
                // один счет для каждого веса
                Double[] accumulatedGradients = new Double[weights.Length]; 
                for (Int32 i = 0; i < trainData.Count(); ++i)  // накопление
                {
                    // Нет необходимости, чтобы перетасовать данные
                    double computed = ComputeOutput(trainData[i], weights); 
                    Int32 targetIndex = trainData[i].Length - 1;
                    Double target = trainData[i][targetIndex];
                    accumulatedGradients[0] += (target - computed) * 1;
                    for (Int32 j = 1; j < weights.Count(); ++j)
                    {
                        accumulatedGradients[j] += (target - computed) * trainData[i][j - 1];
                    }   
                }
                for (Int32 j = 0; j < weights.Count(); ++j)
                {
                    weights[j] += alpha * accumulatedGradients[j];
                }
            } 
            return weights;
        }
        /// <summary>
        /// Метод Shuffle обрабатывает данные в случайном порядке
        /// </summary>
        /// <param name="sequence">Входныые данные</param>
        private void Shuffle<T>(T[] sequence)
        {
            for (Int32 i = 0; i < sequence.Count(); ++i)
            {
                var r = rnd.Next(i, sequence.Length);
                T tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }
        /// <summary>
        /// Метод Error вычисляет среднеквадратичную ошибку
        /// </summary>
        /// <param name="trainData">Выборка данных для обучения модели</param>
        /// <param name="weights">Выборка весов</param>
        /// <returns>Возвращает среднеквадратичную ошибку</returns>
        private Double Error(Double[][] trainData, Double[] weights)
        {
            // среднеквадратической ошибки с помощью поставляемых весов
            Int32 yIndex = trainData[0].Count() - 1; // y-value (0/1) является последней колонкой
            Double sumSquaredError = 0.0;
            for (Int32 i = 0; i < trainData.Count(); ++i)
            {
                Double computed = ComputeOutput(trainData[i], weights);
                Double desired = trainData[i][yIndex];
                sumSquaredError += (computed - desired) * (computed - desired);
            }
            return sumSquaredError / trainData.Length;
        }
        /// <summary>
        /// Метод ComputeOutput вычисляет выходы обученной модели
        /// </summary>
        /// <param name="dataItem">Точки по которым производится вычисления выходов</param>
        /// <param name="weights">Выборка весов</param>
        /// <returns>Возращает выходы модели</returns>
        private Double ComputeOutput(Double[] dataItem, Double[] weights)
        {
            Double z = 0.0;
            z += weights[0];
            for (Int32 i = 0; i < weights.Count() - 1; ++i)
            {
                z += (weights[i + 1] * dataItem[i]);
            }
            return 1.0 / (1.0 + Exp(-z));
        }
        /// <summary>
        /// Метод ComputeDependent вычисляет зависимость
        /// </summary>
        /// <param name="dataItem">Точки по которым производится вычисления зависимостей</param>
        /// <param name="weights">Выборка весов</param>
        /// <returns>Возращает зависимости</returns>
        private Int32 ComputeDependent(Double[] dataItem, Double[] weights)
        {
            if (ComputeOutput(dataItem, weights) <= 0.5) return 0;
            else return 1;
        }
        /// <summary>
        /// Метод Accuracy вычисляет точность обучения модели
        /// </summary>
        /// <param name="trainData">Выборка данных для обучения модели</param>
        /// <param name="weights">Выборка весов</param>
        /// <returns>Возращает точность обучения модели</returns>
        public Double Accuracy(Double[][] trainData, Double[] weights)
        {
            Int32 numCorrect = 0;
            Int32 numWrong = 0;
            Int32 yIndex = trainData[0].Count() - 1;
            for (Int32 i = 0; i < trainData.Count(); ++i)
            {
                Int32 computed = ComputeDependent(trainData[i], weights);
                Int32 target = (Int32)trainData[i][yIndex];
                if (computed == target) ++numCorrect;
                else ++numWrong;
            }
            return (numCorrect * 1.0) / (numWrong + numCorrect);
        }
    }
    public class Print
    {
        /// <summary>
        /// Метод NeuralNWGradientDescentRealData реализует обучение на реальнных данных
        /// нейроной сети методом градиентного спуска
        /// </summary>
        public static void NeuralNWGradientDescentRealData(Double[] numFeatures, Int32 numRows, Int32 seed)
        {
            Console.WriteLine("Логическая регресия");
            Console.WriteLine("Цель заключается в демонстрации обучения с использованием градиентного спуска");

            Console.WriteLine($"Генерация { numRows } искусственных элементов данных { numFeatures.Count() } функции");
            Double[][] allData;
            try
            {
                allData = LogisticGradient.MakeAllData(numFeatures, numRows, seed);
                Console.WriteLine("Разбиение данных на обучающие и проверочные наборы в соотношении с правилом Парето 80% на 20%");
                LogisticGradient.MakeTrainTest(allData, 0, out Double[][] trainData, out Double[][] testData);
                Console.WriteLine("Готово");
                Console.WriteLine("\nОбучающие данные: \n");
                LogisticGradient.ShowData(trainData, 3, 2, true);

                Console.WriteLine("\nТестовые данные: \n");
                LogisticGradient.ShowData(testData, 3, 2, true);

                Console.WriteLine("Генерация обучающих-бинарных класификаторов");
                using (LogisticClassifier lc = new LogisticClassifier(numFeatures))
                {
                    Int32 maxEpochs = 1000;
                    Console.WriteLine($"Установка maxEpochs = {maxEpochs}");
                    Double alpha = 0.05;
                    Console.WriteLine($"Установка коэфициента обучения = {alpha.ToString("F2")}");

                    Console.WriteLine("\nНачинаеться обучение с использованием (стохастического) градиентного спуска");
                    Double[] weights = lc.Train(trainData, maxEpochs, alpha);
                    Console.WriteLine("Обучение завершено");

                    Console.WriteLine("\nЛучшие найденые веса:");
                    LogisticGradient.ShowVector(weights, 4, true);

                    Double trainAcc = lc.Accuracy(trainData, weights);
                    Console.WriteLine($"Точность прогноза на обучающих данных = {trainAcc.ToString("F4")}");

                    Double testAcc = lc.Accuracy(testData, weights);
                    Console.WriteLine($"Точность прогноза на тестовых данных = {testAcc.ToString("F4")}");
                    Console.WriteLine("\nКонец генерации\n");
                    Console.ReadLine();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Метод NeuralNWGradientDescentData реализует обучение с помощью случайнных входных данных
        /// нейроной сети методом градиентного спуска
        /// </summary>
        /// <param name="numFeatures">Число входных переменных</param>
        /// <param name="numRows">Число весовых коефициентов</param>
        /// <param name="seed">Начальное значение генератора случайных чисел</param>
        public static void NeuralNWGradientDescentData(Int32 numFeatures, Int32 numRows, Int32 seed)
        {
            Console.WriteLine("Логическая регресия");
            Console.WriteLine("Цель заключается в демонстрации обучения с использованием градиентного спуска");

            Console.WriteLine($"Генерация { numRows } искусственных элементов данных { numFeatures } функции");
            Double[][] allData = LogisticGradient.MakeAllData(numFeatures, numRows, seed);

            Console.WriteLine("Разбиение данных на обучающие и проверочные наборы в соотношении с правилом Парето 80% на 20%");
            LogisticGradient.MakeTrainTest(allData, 0, out Double[][] trainData, out Double[][] testData);
            Console.WriteLine("Готово");

            Console.WriteLine("\nОбучающие данные: \n");
            LogisticGradient.ShowData(trainData, 3, 2, true);

            Console.WriteLine("\nТестовые данные: \n");
            LogisticGradient.ShowData(testData, 3, 2, true);

            Console.WriteLine("Генерация обучающих-бинарных класификаторов");
            using (LogisticClassifier lc = new LogisticClassifier(numFeatures))
            {
                Int32 maxEpochs = 1000;
                Console.WriteLine($"Установка maxEpochs = {maxEpochs}");
                Double alpha = 0.01;
                Console.WriteLine($"Установка коэфициента обучения = {alpha.ToString("F2")}");

                Console.WriteLine("\nНачинаеться обучение с использованием (стохастического) градиентного спуска");
                Double[] weights = lc.Train(trainData, maxEpochs, alpha);
                Console.WriteLine("Обучение завершено");

                Console.WriteLine("\nЛучшие найденые веса:");
                LogisticGradient.ShowVector(weights, 4, true);

                Double trainAcc = lc.Accuracy(trainData, weights);
                Console.WriteLine($"Точность прогноза на обучающих данных = {trainAcc.ToString("F4")}");

                Double testAcc = lc.Accuracy(testData, weights);
                Console.WriteLine($"Точность прогноза на тестовых данных = {testAcc.ToString("F4")}");
                Console.WriteLine("\nКонец генерации\n");
                Console.ReadLine();
            }
        }
    }
}