using System;
using Expansion;

namespace AutomaticCalculationParameters
{
    /// <summary>
    /// Класс AreaFeatures хранит даные о потребителях моделируемого города(микрорайона)
    /// </summary>
    internal static class AreaFeatures
    {
        /// <summary>
        /// Адресс расположения проекта на компьютере (при переносе проекта он меняеться)
        /// </summary>
        private static String addProject = @"D:\CDImagest\ХНУМГ ім.О.М.Бекетова\Диплом";

        /// <summary>
        /// Адресс расположения исходных данных в проекте
        /// </summary>
        private static String internalAddProject = @"\AutomaticCalculationParameters\AutomaticCalculationParameters\InitialData\";

        /// <summary>
        /// Метод Data читает выборку данных из файла
        /// </summary>
        internal static Double[] Data()
        {
            String address = addProject + internalAddProject + "Data.txt";
            return ExpansionString.GetFileStringToDouble(address);
        }
    }
}