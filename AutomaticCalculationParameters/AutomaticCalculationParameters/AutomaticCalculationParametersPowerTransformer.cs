using System;
using System.ComponentModel;
using static System.Math;

namespace AutomaticCalculationParameters
{
    class AutomaticCalculationParametersPowerTransformer : Component, IDisposable
    {
        #region Fields
        /// ИСХОДНЫЕ ДАННЫЕ

        /// <summary>
        /// Номинальная мощность, VA
        /// </summary>
        private Double RatedPower { get; set; }
        /// <summary>
        /// Номинальное первичное напряжение, V
        /// </summary>
        private Double RatedPrimaryVoltage { get; set; }
        /// <summary>
        /// Номинальное вторичное напряжение, V
        /// </summary>
        private Double RatedSecondaryVoltage { get; set; }
        /// <summary>
        /// Номинальная частота, Hz
        /// </summary>
        private Double RatedFrequancy { get; set; }
        /// <summary>
        /// Напряжение короткого замыкания, %
        /// </summary>
        private Double ShortCircuitVoltage { get; set; }
        /// <summary>
        /// Активная мощность короткого замыкания, W
        /// </summary>
        private Double ShortCircuiteActivePower { get; set; }
        /// <summary>
        /// Ток холостого хода, %
        /// </summary>
        private Double IddleModeCurrent { get; set; }
        /// <summary>
        /// Активная мощность холостого хода, W
        /// </summary>
        private Double IddleModeActivePower { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор класса - по умолчанию
        /// </summary>
        public AutomaticCalculationParametersPowerTransformer() => this.isDisposed = false;
        
        /// <summary>
        /// Конструктор класса для инициализации переменных
        /// </summary>
        /// <param name="RatedPower">Номинальная мощность, VA</param>
        /// <param name="RatedPrimaryVoltage">Номинальное первичное напряжение, V</param>
        /// <param name="RatedSecondaryVoltage">Номинальное вторичное напряжение, V</param>
        /// <param name="RatedFrequancy">Номинальная частота, Hz</param>
        /// <param name="ShortCircuitVoltage">Напряжение короткого замыкания, %</param>
        /// <param name="ShortCircuiteActivePower">Активная мощность короткого замыкания, W</param>
        /// <param name="IddleModeCurrent">Ток холостого хода, %</param>
        /// <param name="IddleModeActivePower">Активная мощность холостого хода, W</param>
        public AutomaticCalculationParametersPowerTransformer(Double RatedPower, Double RatedPrimaryVoltage, 
            Double RatedSecondaryVoltage, Double RatedFrequancy, Double ShortCircuitVoltage, 
            Double ShortCircuiteActivePower, Double IddleModeCurrent, Double IddleModeActivePower)
        {
            this.RatedPower = RatedPower;
            this.RatedPrimaryVoltage = RatedPrimaryVoltage;
            this.RatedSecondaryVoltage = RatedSecondaryVoltage;
            this.RatedFrequancy = RatedFrequancy;
            this.ShortCircuitVoltage = ShortCircuitVoltage;
            this.ShortCircuiteActivePower = ShortCircuiteActivePower;
            this.IddleModeCurrent = IddleModeCurrent;
            this.IddleModeActivePower = IddleModeActivePower;
            isDisposed = false;
        }
        #endregion
        #region Destructor
        /// <summary>
        /// Деструктор класса
        /// </summary>
        ~AutomaticCalculationParametersPowerTransformer() => Dispose(true);
        
        #endregion
        #region Methods for calculating the characteristics of the transformer
        /// НОМИНАЛЬНЫЕ ДАННЫЕ

        /// <summary>
        /// Метод OnePhasePower расчитывает мощность одной фазы, ВА
        /// </summary>
        /// <returns>Возращает мощность одной фазы, результат в вольт Амперах</returns>
        private Double OnePhasePower() => RatedPower / 3;
        /// <summary>
        /// Метод PhasePrimaryVoltage расчитывает первичное напряжение фазы, В
        /// </summary>
        /// <returns>Возращает первичное напряжение фазы, результат в Вольтах</returns>
        private Double PhasePrimaryVoltage() => RatedPrimaryVoltage / Sqrt(3);

        /// <summary>
        /// Метод PhaseSecondaryVoltage расчитывает вторичное напряжение фазы, В
        /// </summary>
        /// <returns>Возращает вторичное напряжение фазы, результат в Вольтах</returns>
        private Double PhaseSecondaryVoltage() => RatedSecondaryVoltage / Sqrt(3);
        /// <summary>
        /// Метод PhasePrimaryCurrent расчитывает первичный ток фазы, А
        /// </summary>
        /// <returns>Возращает первичный ток фазы, результат в Амперах</returns>
        public Double PhasePrimaryCurrent() => OnePhasePower() / RatedPrimaryVoltage;
        /// <summary>
        /// Метод PhasePrimaryCurrent расчитывает вторичный ток фазы, А
        /// </summary>
        /// <returns>Возращает вторичный ток фазы, результат в Амперах</returns>
        public Double PhaseSecondaryCurrent() => TransformationRatio() * RatedPrimaryVoltage;
        /// <summary>
        /// Метод TransformationRatio расчитывает коефициент трансформации
        /// </summary>
        /// <returns>Возращает результат в относительных единицах</returns>
        public Double TransformationRatio() => RatedPrimaryVoltage / RatedSecondaryVoltage;
        /// <summary>
        /// Метод PrimaryWindingImpedance расчитывает первичное сопротивление обмотки, Ом
        /// </summary>
        /// <returns>Возращает сопротивление первичной обмотки, результат в Омах</returns>
        public Double PrimaryWindingImpedance() => PhasePrimaryCurrent() / PhasePrimaryCurrent();
        /// <summary>
        /// Метод SecondaryWindingImpedacne расчитывает вторичное сопротивление обмотки, Ом
        /// </summary>
        /// <returns>>Возращает сопротивление вторичной обмотки, результат в Омах</returns>
        public Double SecondaryWindingImpedacne() => PhaseSecondaryVoltage() / PhaseSecondaryCurrent();
        /// <summary>
        /// Метод SecondaryWindingReducedImpedance расчитывает пониженное сопротивление вторичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает пониженное сопротивление вторичной обмотки, результат в Омах</returns>
        public Double SecondaryWindingReducedImpedance() => SecondaryWindingImpedacne() *
            Pow(TransformationRatio(), 2);

        /// РЕЖИМ ХОЛОСТОГО ХОДА
        ///
        /// <summary>
        /// Метод IddleModePhaseCurrent расчитывает ток по одной фазе в режиме холостого хода, А
        /// </summary>
        /// <returns>Возращаает ток по одной фазе в режиме холостого хода, результат в Амперах</returns>
        public Double IddleModePhaseCurrent() => IddleModeCurrent * (PhasePrimaryCurrent() / 100);
        /// <summary>
        ///  Метод IddleModeTotalImpedance расчитывает общее сопротивление в режиме холостого хода, Ом
        /// </summary>
        /// <returns>Возращает общее сопротивление в режиме холостого хода, результат в Омах</returns>
        public Double IddleModeTotalImpedance() => PhasePrimaryVoltage() / IddleModePhaseCurrent();
        /// <summary>
        /// Метод IddleModePhaseActivePower расчитывает активную мощность фазы в режиме холостого хода, А
        /// </summary>
        /// <returns>Возращает активную мощность фазы в режиме холостого хода, результат в Амперах</returns>
        public Double IddleModePhaseActivePower() => IddleModeActivePower / 3;
        /// <summary>
        /// Метод IddleModePowerFactor расчитывает коефициент мощности в режиме холостого хода
        /// </summary>
        /// <returns>Возращает результат в относительных единицах</returns>
        public Double IddleModePowerFactor() => IddleModePhaseActivePower() /
            (PhasePrimaryVoltage() * IddleModePhaseCurrent());
        /// <summary>
        /// Метод MagneticCircuitActiveResistance расчитывает активное сопротивление магнитной цепи в режиме холостого хода, Ом
        /// </summary>
        /// <returns>Возращает активное сопротивление магнитной цепи, результат в Омах</returns>
        public Double MagneticCircuitActiveResistance() => IddleModeTotalImpedance() *
            IddleModePowerFactor();
        /// <summary>
        /// Метод MagneticCircuiteInductiveReactance расчитывает индуктивное сопротивление магнитной цепи в режиме холостого хода, Ом
        /// </summary>
        /// <returns>Возращает индуктивное сопротивление магнитной цепи, результат в Омах</returns>
        public Double MagneticCircuiteInductiveReactance() => Sqrt(Pow(IddleModeTotalImpedance(), 2) -
            Pow(MagneticCircuitActiveResistance(), 2));

        /// РЕЖИМ КОРОТКОГО ЗАМЫКАНИЯ
        /// 
        /// <summary>
        /// Метод ShortCircuitePhaseVoltage расчитывает фазное напряжение короткого замыкания, В
        /// </summary>
        /// <returns>Возращает фазное напряжение короткого замыкания, результат в Вольтах</returns> 
        public Double ShortCircuitePhaseVoltage() => PhaseSecondaryVoltage() * (ShortCircuitVoltage / 100);
        /// <summary>
        /// Метод ShortCircuiteTotalImpedance расчитывает общее сопротивление короткого замыкания, Ом
        /// </summary>
        /// <returns>Возращает общее сопротивление короткого замыкания, результат в Омах</returns>
        public Double ShortCircuiteTotalImpedance() => ShortCircuitePhaseVoltage() / PhasePrimaryCurrent();
        /// <summary>
        /// Метод ShortCircuitePhaseActivePower расчитывает активную мощность фазы короткого замыкания, Вт
        /// </summary>
        /// <returns>Возращает активную мощность фазы короткого замыкания, результат в Ватах</returns>
        public Double ShortCircuitePhaseActivePower() => ShortCircuiteActivePower / 3;
        /// <summary>
        /// Метод ShortCircuitePowerFactor расчитывает коефициент мощности короткого замыкания
        /// </summary>
        /// <returns>Возращает результат в относительных единицах</returns>
        public Double ShortCircuitePowerFactor() => ShortCircuitePhaseActivePower() /
            (ShortCircuitePhaseVoltage() * PhasePrimaryCurrent());
        /// <summary>
        /// Метод ShortCircuiteActiveResistance расчитывает активное сопротивление короткого замыкания, Ом
        /// </summary>
        /// <returns>Возращает активное сопротивление короткого замыкания, результат в Омах</returns>
        public Double ShortCircuiteActiveResistance() => ShortCircuiteTotalImpedance() * ShortCircuitePowerFactor();
        /// <summary>
        /// Метод ShortCircuiteInductiveReactance расчитывает индуктивное сопротивление короткого замыкания, Ом
        /// </summary>
        /// <returns>Возращает индуктивное сопротивление короткого замыкания, результат в Омах</returns>
        public Double ShortCircuiteInductiveReactance() => Sqrt(Pow(ShortCircuiteTotalImpedance(), 2) -
            Pow(ShortCircuiteActiveResistance(), 2));

        /// ПАРАМЕТРЫ ОБМОТКИ ТРАНСФОРМАТОРА

        /// <summary>
        /// Метод SecondaryWindingReducedActiveResistance расчитывает пониженное активное 
        /// сопротивление вторичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает пониженное активное сопротивление вторичной обмотки, результат в Омах</returns>
        public Double SecondaryWindingReducedActiveResistance() => 0.5 * ShortCircuiteActiveResistance();
        /// <summary>
        /// Метод PrimaryWindingActiveResistance расчитывает активное сопротивление первичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает активное сопротивление первичной обмотки, результат в Омах</returns>
        public Double PrimaryWindingActiveResistance() => SecondaryWindingReducedActiveResistance();
        /// <summary>
        /// Метод SecondaryWindingActiveResistance расчитывает активное сопротивление вторичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает активное сопротивление вторичной обмотки, результат в Омах</returns>
        public Double SecondaryWindingActiveResistance() => SecondaryWindingReducedActiveResistance() /
            Pow(TransformationRatio(), 2);
        /// <summary>
        /// Метод SecondaryWindingReducedInductiveReactance расчитывает пониженное индуктивное 
        /// сопротивление вторичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает пониженное индуктивное сопротивление вторичной обмотки, результат в Омах</returns>
        public Double SecondaryWindingReducedInductiveReactance() => 0.5 * ShortCircuiteInductiveReactance();
        /// <summary>
        /// Метод PrimaryWindingInductiveReactance расчитывает индуктивное сопротивление первичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает индуктивное сопротивление первичной обмотки, результат а Омах</returns>
        public Double PrimaryWindingInductiveReactance() => SecondaryWindingReducedInductiveReactance();
        /// <summary>
        /// Метод SecondaryWindingInductiveReactance расчитывает индуктивное сопротивление вторичной обмотки, Ом
        /// </summary>
        /// <returns>Возращает индуктивное сопротивление вторичной обмотки, результат а Омах</returns>
        public Double SecondaryWindingInductiveReactance() => SecondaryWindingReducedInductiveReactance() /
            Pow(TransformationRatio(), 2);

        /// ПАРАМЕТРЫ ИНДУКТИВНОСТЕЙ
        /// 
        /// <summary>
        /// Метод PrimaryWindingInductance расчитывает индуктивность первичной обмотки, Гн
        /// </summary>
        /// <returns>Возращает индуктивность первичной обмотки, результат в Генри</returns>
        public Double PrimaryWindingInductance() => PrimaryWindingInductiveReactance() / (2 * PI * RatedFrequancy);

        /// <summary>
        /// Метод PrimaryWindingInductance расчитывает индуктивность первичной обмотки, Гн
        /// </summary>
        /// <returns>Возращает индуктивность первичной обмотки, результат в Генри</returns>
        public Double SecondaryWindingInductance() => SecondaryWindingInductiveReactance() / (2 * PI * RatedFrequancy);
        /// <summary>
        /// Метод MagneticCircuiteInductance расчитывает индуктивность магнитной цепи, Гн
        /// </summary>
        /// <returns>Возращает индуктивность магнитной цепи, результат в Генри</returns>
        public Double MagneticCircuiteInductance() => MagneticCircuiteInductiveReactance() / (2 * PI * RatedFrequancy);
        #endregion
        #region Properties for reading results
        /// <summary>
        /// Свойство PropertyOnePhasePower позволяющее получить результат вычисления метода 
        /// OnePhasePower, который вычисляет мощность одной фазы, ВА
        /// </summary>
        public Double PropertyOnePhasePower => OnePhasePower();
        /// <summary>
        /// Свойство PropertyPhasePrimaryVoltage позволяющее получить результат вычисления метода 
        /// PhasePrimaryVoltage, который вычисляет первичное напряжение фазы, В
        /// </summary>
        public Double PropertyPhasePrimaryVoltage => PhasePrimaryVoltage();
        /// <summary>
        /// Свойство PropertyPhaseSecondaryVoltage позволяющее получить результат вычисления метода 
        /// PhaseSecondaryVoltage, который вычисляет вторичное напряжение фазы, В
        /// </summary>
        public Double PropertyPhaseSecondaryVoltage => PhaseSecondaryVoltage();
        #endregion

        #region Removing resource
        /// <summary>
        /// Переменная указывает какие ресурсы освобождать, по умолчанию false
        /// </summary>
        bool isDisposed;
        /// <summary>
        /// Переопределение метода Dispose для освобождения управляимых и не управляемых ресурсов
        /// </summary>
        /// <param name="dispAll">Параметр указывает какие ресурсы освобождать,
        /// true - указывает на освобождение управляемых и неуправляемых ресурсов,
        /// false - указывает на освобождение неуправляемых ресурсов
        ///</param>
        protected override void Dispose(bool dispAll)
        {
            if (!isDisposed)
            {
                // Освобождае управляемы ресурсы.
                if (dispAll) isDisposed = true; // Устанавливае компонен в состояния освобождения.
                base.Dispose(dispAll); // Освобождае неуправляемы ресурсы.
            }
        }
        #endregion
    }
}