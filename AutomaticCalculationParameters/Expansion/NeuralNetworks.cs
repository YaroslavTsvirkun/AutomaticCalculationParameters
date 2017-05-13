using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.ComponentModel;

namespace Expansion
{
    /// <summary>
    /// Класс NeuralNT предоставляет инфраструктуру для построения нейронных сетей
    /// </summary>
    public class NeuralNW : Component
    {
        /// <summary>
        /// Полносвязанная нейронная сеть
        /// </summary>
        LayerNW[] Layers;

        /// <summary>
        /// Количество слоев нейронной сети
        /// </summary>
        private Int32 CountLayers { get; set; } = 0;

        /// <summary>
        /// Хранит число входов в нейроную сеть
        /// </summary>
        Int32 CountX { get; set; }

        /// <summary>
        /// Хранит число выходов с нейроной сети
        /// </summary>
        Int32 CountY { get; set; }

        /// <summary>
        /// Хранит чило выходов нейронной сети
        /// </summary>
        Double[][] NETOUT;  // NETOUT[countLayers + 1][]

        /// <summary>
        /// 
        /// </summary>
        Double[][] DELTA;   // NETOUT[countLayers][]

        /// <summary>
        /// Конструктор NeuralNW создает полносвязанную сеть из 1 слоя
        /// </summary>
        /// <param name="sizeX">Размерность вектора входных параметров</param>
        /// <param name="sizeY">Размерность вектора выходных параметров</param>
        public NeuralNW(Int32 sizeX, Int32 sizeY)
        {
            Layers = new LayerNW[CountLayers = 1];
            Layers[0] = new LayerNW(sizeX, sizeY);
            Layers[0].GenerateWeights();
        }

        /// <summary>
        /// Конструктор NeuralNW создает полносвязанную сеть из n слоев
        /// </summary>
        /// <param name="sizeX">Размерность вектора входных параметров</param>
        /// <param name="layers">Массив слоев, Значение элементов массива это 
        /// количество нейронов в слое</param>
        public NeuralNW(Int32 sizeX, params Int32[] layers)
        {
            CountLayers = layers.Length;
            CountX = sizeX;
            CountY = layers[layers.Length - 1];
            // Размерность выходов нейронов и Дельты
            NETOUT = new Double[CountLayers + 1][];
            NETOUT[0] = new Double[sizeX];
            DELTA = new Double[CountLayers][];

            this.Layers = new LayerNW[CountLayers];
            Int32 countY1;
            Int32 countX1 = sizeX;
            // Устанавливаем размерность слоям и заполняем слоя случайнымичислами
            for (int i = 0; i < CountLayers; i++)
            {
                countY1 = layers[i];
                NETOUT[i + 1] = new Double[countY1];
                DELTA[i] = new Double[countY1];
                this.Layers[i] = new LayerNW(countX1, countY1);
                this.Layers[i].GenerateWeights();
                countX1 = countY1;
            }
        }

        /// <summary>
        /// Конструктор NeuralNW открывает полносвязанную нейроную сеть из двоичного файла
        /// </summary>
        /// <param name="fileName">Имя нейроной сети</param>
        public NeuralNW(String fileName)
        {
            OpenNW(fileName);
        }

        /// <summary>
        /// Метод OpenNW открывает полносвязанную нейроную сеть
        /// </summary>
        /// <param name="fileName">Имя нейроной сети</param>
        public void OpenNW(String fileName)
        {
            byte[] binNW = File.ReadAllBytes(fileName);
            Int32 k = 0;
            // Извлекаем количество слоев из массива
            CountLayers = ReadFromArrayInt(binNW, ref k);
            Layers = new LayerNW[CountLayers];
            // Извлекаем размерность слоев
            Int32 CountY1 = 0;
            Int32 CountX1 = ReadFromArrayInt(binNW, ref k);
            // Размерность входа
            CountX = CountX1;
            // Выделяемпамять под выходы нейронов и дельта
            NETOUT = new Double[CountLayers + 1][];
            NETOUT[0] = new Double[CountX1];
            DELTA = new Double[CountLayers][];
            for (Int32 i = 0; i < CountLayers; i++)
            {
                CountY1 = ReadFromArrayInt(binNW, ref k);
                Layers[i] = new LayerNW(CountX1, CountY1);
                CountX1 = CountY1;
                // Выделяем память
                NETOUT[i + 1] = new Double[CountY1];
                DELTA[i] = new Double[CountY1];
            }
            // Размерность выхода
            CountY = CountY1;
            // Извлекаем и записываем сами веса
            for (Int32 r = 0; r < CountLayers; r++)
                for (Int32 p = 0; p < Layers[r].GetCountX; p++)
                    for (Int32 q = 0; q < Layers[r].GetCountY; q++)
                        Layers[r][p, q] = ReadFromArrayDouble(binNW, ref k);
        }

        /// <summary>
        /// Метод OpenNW сохраняет полносвязанную нейроную сеть
        /// </summary>
        /// <param name="FileName">Имя нейроной сети</param>
        public void SaveNW(String FileName)
        {
            // размер сети в байтах
            Int32 sizeNW = GetSizeNW();
            Byte[] binNW = new Byte[sizeNW];
            Int32 k = 0;
            // Записываем размерности слоев в массив байтов
            WriteInArray(binNW, ref k, CountLayers);
            if (CountLayers <= 0) return;
            WriteInArray(binNW, ref k, Layers[0].GetCountX);
            for (Int32 i = 0; i < CountLayers; i++) WriteInArray(binNW, ref k, Layers[i].GetCountY);
            // Записываем сами веса
            for (Int32 r = 0; r < CountLayers; r++)
                for (Int32 p = 0; p < Layers[r].GetCountX; p++)
                    for (Int32 q = 0; q < Layers[r].GetCountY; q++)
                        WriteInArray(binNW, ref k, Layers[r][p, q]);       
            File.WriteAllBytes(FileName, binNW);
        }

        // 
        /// <summary>
        /// Метод NetOUT возвращает значение j-го слоя нейроной сети
        /// </summary>
        /// <param name="inX">Входное значение j-го слоя</param>
        /// <param name="outY">Вызодное значение j-го слоя</param>
        /// <param name="jLayer">Позиция j-го слоя</param>
        public void NetOUT(Double[] inX, out Double[] outY, Int32 jLayer)
        {
            GetOUT(inX, jLayer);
            outY = new Double[NETOUT[jLayer].Count()];
            for (Int32 i = 0; i < NETOUT[jLayer].Count(); i++) outY[i] = NETOUT[jLayer][i];
        }

        /// <summary>
        /// Метод NetOUT возвращает значения нейроной сети
        /// </summary>
        /// <param name="inX">Входное значение слоя</param>
        /// <param name="outY">Вызодное значение слоя</param>
        public void NetOUT(Double[] inX, out Double[] outY) => NetOUT(inX, out outY, CountLayers);

        /// <summary>
        /// Метод CalcError возвращает ошибку (метод наименьших квадратов)
        /// </summary>
        /// <param name="inX">Входное значение слоя</param>
        /// <param name="outY">Вызодное значение слоя</param>
        /// <returns></returns>
        public Double CalcError(Double[] inX, Double[] outY)
        {
            Double kErr = 0;
            for (Int32 i = 0; i < outY.Length; i++)
            {
                kErr += Math.Pow(outY[i] - NETOUT[CountLayers][i], 2);
            }
            return 0.5 * kErr;
        }

        /// <summary>
        /// Метод LernNW обучает нейроную сеть, изменяя ее весовые коэффициэнты
        /// </summary>
        /// <param name="inX">Входное значение слоя</param>
        /// <param name="outY">Вызодное значение слоя</param>
        /// <param name="kLern">Коэффициэнт обучения/изменения значения</param>
        /// <returns>Метод возвращает ошибку 0.5(Y-outY)^2</returns>
        public Double LernNW(Double[] inX, Double[] outY, Double kLern)
        {
            Double O; // Вход нейрона
            Double s;
            // Вычисляем выход сети
            GetOUT(inX);
            // Заполняем дельта последнего слоя
            for (Int32 j = 0; j < Layers[CountLayers - 1].GetCountY; j++)
            {
                O = NETOUT[CountLayers][j];
                DELTA[CountLayers - 1][j] = (outY[j] - O) * O * (1 - O);
            }
            // Перебираем все слои начиная с последнего 
            // изменяя веса и вычисляя дельта для скрытого слоя
            for (Int32 k = CountLayers - 1; k >= 0; k--)
            {
                // Изменяем веса выходного слоя
                for (Int32 j = 0; j < Layers[k].GetCountY; j++)
                    for (Int32 i = 0; i < Layers[k].GetCountX; i++)
                        Layers[k][i, j] += kLern * DELTA[k][j] * NETOUT[k][i];
                if (k > 0)
                {
                    // Вычисляем дельта слоя к-1
                    for (Int32 j = 0; j < Layers[k - 1].GetCountY; j++)
                    {
                        s = 0;
                        for (Int32 i = 0; i < Layers[k].GetCountY; i++) s += Layers[k][j, i] * DELTA[k][i];
                        DELTA[k - 1][j] = NETOUT[k][j] * (1 - NETOUT[k][j]) * s;
                    }
                }
            }
            return CalcError(inX, outY);
        }


        /// <summary>
        /// Метод GetOUT возвращает все значения нейронов до скрытого слоя
        /// </summary>
        /// <param name="inX">Входное значение слоя</param>
        /// <param name="lastLayer">Входное значения скрытого слоя</param>
        private void GetOUT(Double[] inX, Int32 lastLayer)
        {
            Double s = 0;
            for (Int32 j = 0; j < Layers[0].GetCountX; j++) NETOUT[0][j] = inX[j];
            for (Int32 i = 0; i < lastLayer; i++)
            {
                // размерность столбца проходящего через i-й слой
                for (Int32 j = 0; j < Layers[i].GetCountY; j++)
                {
                    for (Int32 k = 0; k < Layers[i].GetCountX; k++) s += Layers[i][k, j] * NETOUT[i][k];
                    // Вычисляем значение активационной функции
                    s = 1.0 / (1 + Math.Exp(-s));
                    NETOUT[i + 1][j] = 0.998 * s + 0.001;
                }
            }
        }

        /// <summary>
        /// Метод GetOUT возвращает все значения нейронов всех слоев
        /// </summary>
        /// <param name="inX">Массив значений нейронов всех слоев</param>
        void GetOUT(Double[] inX) => GetOUT(inX, CountLayers);

        /// <summary>
        /// Метод GetSizeNW возвращает размер нейроной сети в байтах
        /// </summary>
        /// <returns>Dозвращает размер нейроной сети в байтах</returns>
        Int32 GetSizeNW()
        {
            Int32 sizeNW = sizeof(Int32) * (CountLayers + 2);
            for (Int32 i = 0; i < CountLayers; i++)
            {
                sizeNW += sizeof(Double) * Layers[i].GetCountX * Layers[i].GetCountY;
            }
            return sizeNW;
        }

        /// <summary>
        /// Метод Layer возвращает i-й слой нейронной сети
        /// </summary>
        /// <param name="num">Номер слоя</param>
        /// <returns>Возвращает i-й слой нейронной сети</returns>
        public LayerNW Layer(Int32 num) => Layers[num];

        /// <summary>
        /// Метод WriteInArray разбивает переменную типа int на байты и записывает в массив
        /// </summary>
        /// <param name="mas">Массив байтов</param>
        /// <param name="pos">Позиция байта</param>
        /// <param name="value">Значение байта</param>
        void WriteInArray(Byte[] mas, ref Int32 pos, Int32 value)
        {
            var dtb = new DataToByte(value);
            mas[pos++] = dtb.b1;
            mas[pos++] = dtb.b2;
            mas[pos++] = dtb.b3;
            mas[pos++] = dtb.b4;
        }

        /// <summary>
        /// Метод WriteInArray разбивает переменную типа double на байты и записывает в массив
        /// </summary>
        /// <param name="mas">Массив байтов</param>
        /// <param name="pos">Позиция байта</param>
        /// <param name="value">Значение байта</param>
        void WriteInArray(Byte[] mas, ref Int32 pos, Double value)
        {
            var dtb = new DataToByte(value);
            mas[pos++] = dtb.b1;
            mas[pos++] = dtb.b2;
            mas[pos++] = dtb.b3;
            mas[pos++] = dtb.b4;
            mas[pos++] = dtb.b5;
            mas[pos++] = dtb.b6;
            mas[pos++] = dtb.b7;
            mas[pos++] = dtb.b8;
        }

        /// <summary>
        /// Метод ReadFromArrayInt извлекает переменную типа Int32 из 4-х байтов массива
        /// </summary>
        /// <param name="mas">Массив с байтами переменной типа Int32</param>
        /// <param name="pos">Позиция байта</param>
        /// <returns></returns>
        Int32 ReadFromArrayInt(Byte[] mas, ref Int32 pos) => new DataToByte(mas[pos++],
            mas[pos++], mas[pos++], mas[pos++]).vInt32;

        /// <summary>
        /// Метод ReadFromArrayDouble извлекает переменную типа Double из 8-ми байтов массива
        /// </summary>
        /// <param name="mas">Массив с байтами переменной типа Double</param>
        /// <param name="pos">Позиция байта</param>
        /// <returns>Возращает собраную с байтов переменную типа Double</returns>
        Double ReadFromArrayDouble(Byte[] mas, ref Int32 pos) => new DataToByte(mas[pos++], 
            mas[pos++], mas[pos++], mas[pos++], mas[pos++], mas[pos++], mas[pos++], mas[pos++]).vDouble;
    }

    /// <summary>
    /// Класс дря разбиения переменных типа Int32 и Double на байты
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal class DataToByte : Component
    {
        [FieldOffset(0)]
        public Double vDouble;
        [FieldOffset(0)]
        public Int32 vInt32;
        [FieldOffset(0)]
        public Byte b1;
        [FieldOffset(1)]
        public Byte b2;
        [FieldOffset(2)]
        public Byte b3;
        [FieldOffset(3)]
        public Byte b4;
        [FieldOffset(4)]
        public Byte b5;
        [FieldOffset(5)]
        public Byte b6;
        [FieldOffset(6)]
        public Byte b7;
        [FieldOffset(7)]
        public Byte b8;

        /// <summary>
        /// Конструктор класса DataToByte для инициализации байтов под переменную типа Double
        /// </summary>
        /// <param name="b1">Позиция первого байта</param>
        /// <param name="b2">Позиция второго байта</param>
        /// <param name="b3">Позиция третьего байта</param>
        /// <param name="b4">Позиция четвертого байта</param>
        /// <param name="b5">Позиция пятого байта</param>
        /// <param name="b6">Позиция шестого байта</param>
        /// <param name="b7">Позиция седьмого байта</param>
        /// <param name="b8">Позиция восьмого байта</param>
        public DataToByte(Byte b1, Byte b2, Byte b3, Byte b4, Byte b5, Byte b6, Byte b7, Byte b8)
        {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
            this.b4 = b4;
            this.b5 = b5;
            this.b6 = b6;
            this.b7 = b7;
            this.b8 = b8;
        }

        /// <summary>
        /// Конструктор класса DataToByte для инициализации байтов под переменную типа Int32
        /// </summary>
        /// <param name="b1">Позиция первого байта</param>
        /// <param name="b2">Позиция второго байта</param>
        /// <param name="b3">Позиция третьего байта</param>
        /// <param name="b4">Позиция четвертого байта</param>
        public DataToByte(Byte b1, Byte b2, Byte b3, Byte b4)
        {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
            this.b4 = b4;
        }

        /// <summary>
        /// Конструктор класса DataToByte для инициализации переменной типа Double
        /// </summary>
        /// <param name="vDouble">Переменной типа Double</param>
        public DataToByte(Double vDouble)
        {
            this.vDouble = vDouble;
        }

        /// <summary>
        /// Конструктор класса DataToByte для инициализации переменной типа Int32
        /// </summary>
        /// <param name="vInt32">Переменной типа Int32</param>
        public DataToByte(Int32 vInt32)
        {
            this.vInt32 = vInt32;
        }
    }

    /// <summary>
    /// Класс LayerNW слой нейросети
    /// </summary>
    public class LayerNW : Component
    {
        /// <summary>
        /// Массив для записи весовых коефициентов
        /// </summary>
        double[,] Weights;

        /// <summary>
        /// Входные нейроны
        /// </summary>
        public Int32 GetCountX { get; set; }

        /// <summary>
        /// Выходные нейроны
        /// </summary>
        public Int32 GetCountY { get; set; }

        /// <summary>
        /// Метод GenerateWeights заполняем веса случайными числами
        /// </summary>
        public void GenerateWeights()
        {
            for (Int32 i = 0; i < GetCountX; i++)
                for (Int32 j = 0; j < GetCountY; j++) Weights[i, j] = new Random().NextDouble() - 0.5;
        }

        /// <summary>
        /// Метод GiveMemory выделяет память под веса
        /// </summary>
        protected void GiveMemory() => Weights = new Double[GetCountX, GetCountY];
        
        /// <summary>
        /// Конструктор LayerNW для инициализации входных и выходных нейронов
        /// </summary>
        /// <param name="countX">Входные нейроны</param>
        /// <param name="countY">Выходные нейроны</param>
        public LayerNW(Int32 countX, Int32 countY)
        {
            GetCountX = countX;
            GetCountY = countY;
            GiveMemory();
        }

        /// <summary>
        /// Индексатор для доступа к весовым значениям
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public double this[Int32 line, Int32 column]
        {
            get { return Weights[line, column]; }
            set { Weights[line, column] = value; }
        }
    }
}