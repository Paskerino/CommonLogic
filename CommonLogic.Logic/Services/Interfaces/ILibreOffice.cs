using System;
using System.Collections.Generic;
using System.Drawing;

namespace CommonLogic.Services.Interfaces
{
    /// <summary>
    /// Надає контракт для сервісу, що керує документами LibreOffice Calc.
    /// </summary>
    public interface ILibreOffice
    {
        /// <summary>
        /// Відкриває файл LibreOffice Calc або створює новий документ, якщо шлях не вказано.
        /// </summary>
        void Open(string filePath = "");

        /// <summary>
        /// Встановлює рамку (бордер) для вказаної комірки.
        /// </summary>
        void SetBorder(int _borderWidth, Point _cellCoord);

        /// <summary>
        /// Встановлює текстове значення у вказану комірку.
        /// </summary>
        void SetValue(string _cellValue, Point _cellCoord);

        /// <summary>
        /// Встановлює числове значення у вказану комірку.
        /// </summary>
        void SetValue(double _cellValue, Point _cellCoord);

        /// <summary>
        /// Встановлює колір фону комірки за вказаними координатами.
        /// </summary>
        void SetColor(int _color, Point _cellCoord);

        /// <summary>
        /// Зберігає поточний документ за вказаним шляхом.
        /// </summary>
        void Save(string filePath);

        /// <summary>
        /// Закриває поточний документ, зберігаючи його за необхідності.
        /// </summary>
        void Close(string filePath = "");

        /// <summary>
        /// Друкує документ на вказаному принтері.
        /// </summary>
        void Print(string filePath, string printerName);

        /// <summary>
        /// Зчитує дані з аркуша.
        /// </summary>
        List<string> Read(string filePath = "", int maxRows = 1, int maxCols = 1);

        /// <summary>
        /// Експортує поточний документ у PDF.
        /// </summary>
        void ExportToPDF(string pdfPath);
    }
}
