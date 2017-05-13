using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ListEquipment
{
    /// <summary>
    /// Класс Transformer предоставляет список трансформаторов
    /// </summary>
    public class Transformer
    {
        /// <summary>
        /// Позиция трансформаторов в списке
        /// </summary>
        private Int32 PositionTransformer { get; }

        /// <summary>
        /// Тип трансформатора
        /// </summary>
        private String TypeTransformer { get; }
        /// <summary>
        /// Номинальное напряжение ВН, кВ
        /// </summary>
        private Double NominalVoltageHV { get; }

        /// <summary>
        /// Номинальное напряжение НН, кВ
        /// </summary>
        private Double NominalVoltageLV { get; }

        /// <summary>
        /// Схема и группа соединения обмоток
        /// </summary>
        private String DrivingWindingConnectionGroup { get; }

        /// <summary>
        /// Коэффициент трансформации
        /// </summary>
        private Double TransformationRatio { get; }

        /// <summary>
        /// Потери холостого хода, Вт
        /// </summary>
        private Double NoLoadLosses { get; }

        /// <summary>
        /// Потери короткого замыкания, Вт
        /// </summary>
        private Double LossShortCircuit { get; }

        /// <summary>
        /// Суммарные потери, Вт
        /// </summary>
        private Double TotalLosses { get; }

        /// <summary>
        /// Напряжение короткого замыкания, %
        /// </summary>
        private Single VoltageShortCircuit { get; }

        /// <summary>
        /// Ток холостого хода, %
        /// </summary>
        private Single NoLoadCurrent { get; }

        /// <summary>
        /// Масса масла, кг
        /// </summary>
        private Single MassOil { get; }

        /// <summary>
        /// Полная масса, кг
        /// </summary>
        private Single FullMass { get; }

        /// <summary>
        /// Удельная масса, кг/кВА
        /// </summary>
        private Single SpecificGravity { get; }

        /// <summary>
        /// Конструктор для вызова методов генерации XML файлов
        /// </summary>
        public Transformer() { }

        /// <summary>
        /// Конструктор для инициализации параметров трансформатора в списке
        /// </summary>
        /// <param name="PositionTransformer">Позиция трансформаторов в списке</param>
        /// <param name="TypeTransformer">Тип трансформатора</param>
        /// <param name="NominalVoltageHV">Номинальное напряжение ВН, кВ</param>
        /// <param name="NominalVoltageLV">Номинальное напряжение НН, кВ</param>
        /// <param name="DrivingWindingConnectionGroup">Схема и группа соединения обмоток</param>
        /// <param name="TransformationRatio">Коэффициент трансформации</param>
        /// <param name="NoLoadLosses">Потери холостого хода, Вт</param>
        /// <param name="LossShortCircuit">Потери короткого замыкания, Вт</param>
        /// <param name="TotalLosses">Суммарные потери, Вт</param>
        /// <param name="VoltageShortCircuit">Напряжение короткого замыкания, %</param>
        /// <param name="NoLoadCurrent">Ток холостого хода, %</param>
        /// <param name="MassOil"> Масса масла, кг</param>
        /// <param name="FullMass">Полная масса, кг</param>
        /// <param name="SpecificGravity">Удельная масса, кг/кВА</param>
        private Transformer(Int32 PositionTransformer,String TypeTransformer, Double NominalVoltageHV, 
            Double NominalVoltageLV, String DrivingWindingConnectionGroup, Double TransformationRatio, 
                Double NoLoadLosses, Double LossShortCircuit, Single TotalLosses, Single VoltageShortCircuit,
                    Single NoLoadCurrent, Single MassOil, Single FullMass, Single SpecificGravity)
        {
            this.PositionTransformer = PositionTransformer;
            this.TypeTransformer = TypeTransformer;
            this.NominalVoltageHV = NominalVoltageHV;
            this.NominalVoltageLV = NominalVoltageLV;
            this.DrivingWindingConnectionGroup = DrivingWindingConnectionGroup;
            this.TransformationRatio = TransformationRatio;
            this.NoLoadLosses = NoLoadLosses;
            this.LossShortCircuit = LossShortCircuit;
            this.TotalLosses = TotalLosses;
            this.VoltageShortCircuit = VoltageShortCircuit;
            this.NoLoadCurrent = NoLoadCurrent;
            this.MassOil = MassOil;
            this.FullMass = FullMass;
            this.SpecificGravity = SpecificGravity;
        }

        /// <summary>
        /// Метод GetAllTransformer предоставляет каталог трансформаторов
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Transformer> GetAllTransformer()
        {
            return new List<Transformer>
            {
                new Transformer(1, "ТМ-63", 6.0, 0.4, "Y/Yн-0", 15, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(2, "ТМ-63", 6.0, 0.4, "Y/∆-11", 15, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(3, "ТМ-63", 6.0, 0.4, "∆/Y-11", 15, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(4, "ТМ-63", 6.0, 0.4, "Y/Zн-11", 15, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),

                new Transformer(5, "ТМ-63", 10.0, 0.4, "Y/Yн-0", 25, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(6, "ТМ-63", 10.0, 0.4, "Y/∆-11", 25, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(7, "ТМ-63", 10.0, 0.4, "∆/Y-11", 25, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(8, "ТМ-63", 10.0, 0.4, "Y/Zн-11", 25, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),

                new Transformer(9, "ТМ-63", 20.0, 0.4, "Y/Yн-0", 50, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(10, "ТМ-63", 20.0, 0.4, "Y/∆-11", 50, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(11, "ТМ-63", 20.0, 0.4, "∆/Y-11", 50, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),
                new Transformer(12, "ТМ-63", 20.0, 0.4, "Y/Zн-11", 50, 255, 1450, 1705, 4.5f, 4.5f, 81, 385, 6.1f),

                new Transformer(13, "ТМ-63", 27.5, 0.4, "Y/Yн-0", 68.75, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(14, "ТМ-63", 27.5, 0.4, "Y/∆-11", 68.75, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(15, "ТМ-63", 27.5, 0.4, "∆/Y-11", 68.75, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(16, "ТМ-63", 27.5, 0.4, "Y/Zн-11", 68.75, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),

                new Transformer(17, "ТМ-63", 35, 0.4, "Y/Yн-0", 87.5, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(18, "ТМ-63", 35, 0.4, "Y/∆-11", 87.5, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(19, "ТМ-63", 35, 0.4, "∆/Y-11", 87.5, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
                new Transformer(20, "ТМ-63", 35, 0.4, "Y/Zн-11", 87.5, 265, 1400, 1665, 5f, 4.5f, 122, 474, 7.52f),
            };
        }
        /// <summary>
        /// Метод XmlListTransforme с генерирует XML файл со списком трансформаторов
        /// </summary>
        public void XmlListTransformer()
        {
            XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("ListEquipments",
                            GetAllTransformer().Select(item => new XElement("ListEquipmentsTransformer",
                                    new XAttribute("TypeTransformer", item.TypeTransformer),
                                    new XElement("PositionTransformer", item.PositionTransformer),
                                    new XElement("NominalVoltageHV", item.NominalVoltageHV),
                                    new XElement("NominalVoltageLV", item.NominalVoltageLV),
                                    new XElement("DrivingWindingConnectionGroup", item.DrivingWindingConnectionGroup),
                                    new XElement("TransformationRatio", item.TransformationRatio),
                                    new XElement("NoLoadLosses", item.NoLoadLosses),
                                    new XElement("LossShortCircuit", item.LossShortCircuit),
                                    new XElement("TotalLosses", item.TotalLosses),
                                    new XElement("VoltageShortCircuit", item.VoltageShortCircuit),
                                    new XElement("NoLoadCurrent", item.NoLoadCurrent),
                                    new XElement("MassOil", item.MassOil),
                                    new XElement("FullMass", item.FullMass),
                                    new XElement("SpecificGravity", item.SpecificGravity)))));
            xmlDoc.Save(Path.Combine(Environment.CurrentDirectory, "xmlTransformer.xml"));
        }
    }
}