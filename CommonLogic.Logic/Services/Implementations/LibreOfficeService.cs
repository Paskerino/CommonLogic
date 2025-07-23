using CommonLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.sdbc;
using unoidl.com.sun.star.sheet;
using unoidl.com.sun.star.table;
using unoidl.com.sun.star.uno;
using unoidl.com.sun.star.view;

namespace CommonLogic.Services.Implementations
{
    public class LibreOfficeService: ILibreOffice
    {

    //"file:///E:/WORK/CommonSoft/LibreOfficeoutput.ods";
    //wmic printer get name
        Process sofficeProcess = null;

        XSpreadsheetDocument xSpreadsheetDoc;
        XSpreadsheets xSpreadsheets;
        XIndexAccess xSheetsIndex;
        XSpreadsheet xSheet;
        XComponent xSpreadsheetComponent;
        public LibreOfficeService() { }
        /// <summary>
        /// Відкриває файл LibreOffice Calc або створює новий документ, якщо шлях не вказано.
        /// Запускає сервер LibreOffice в headless-режимі, підключається до нього та завантажує файл або створює новий.
        /// </summary>
        /// <param name="filePath">Шлях до файлу для відкриття. Якщо не вказано, створюється новий документ.</param>
        public void Open(string filePath = "")
        {
            try
            {
                // Запуск LibreOffice у headless-режимі (без графічного інтерфейсу)
                sofficeProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\LibreOffice\program\soffice.exe",
                    Arguments = "--headless --accept=\"socket,host=localhost,port=8100;urp;\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                // Чекаємо, щоб сервер LibreOffice запустився
                System.Threading.Thread.Sleep(2000);

                // Підключаємося до LibreOffice
                XComponentContext xLocalContext = uno.util.Bootstrap.bootstrap();
                XMultiComponentFactory xMultiComponentFactory = xLocalContext.getServiceManager();
                XComponentLoader xComponentLoader = (XComponentLoader)xMultiComponentFactory.createInstanceWithContext(
                    "com.sun.star.frame.Desktop", xLocalContext);

                // Якщо вказано шлях, відкриваємо файл
                if (filePath.Length > 0)
                {
                    string fileUrl = new Uri(filePath).AbsoluteUri;
                    xSpreadsheetComponent = xComponentLoader.loadComponentFromURL(
                        fileUrl, "_blank", 0, new PropertyValue[0]);
                }
                else
                {
                    // Відкриваємо нову таблицю Calc
                    xSpreadsheetComponent = xComponentLoader.loadComponentFromURL(
                        "private:factory/scalc", "_blank", 0, new PropertyValue[0]);
                }

                // Отримуємо доступ до першого листа у таблиці
                xSpreadsheetDoc = (XSpreadsheetDocument)xSpreadsheetComponent;
                xSpreadsheets = xSpreadsheetDoc.getSheets();
                xSheetsIndex = (XIndexAccess)xSpreadsheets;
                xSheet = (XSpreadsheet)xSheetsIndex.getByIndex(0).Value;
            }
            catch
            {
                // Обробка помилок (можна додати логування)
            }
        }
        /// <summary>
        /// Встановлює рамку (бордер) для вказаної комірки в LibreOffice Calc.
        /// </summary>
        /// <param name="_borderWidth">Товщина лінії рамки (у пунктах).</param>
        /// <param name="_cellCoord">Координати комірки у форматі (X, Y).</param>
        public void SetBorder(int _borderWidth, System.Drawing.Point _cellCoord)
        {
            // Get cell A2 (position (0,1))
            if (xSheet == null)
            {
                throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before setting a border.");
            }
            XCell xCellPlace = xSheet.getCellByPosition(_cellCoord.X, _cellCoord.Y);
            XPropertySet xCellRangeProps = (XPropertySet)xCellPlace;
            // Access cell border properties through XPropertySet
            var tt = xCellRangeProps.getPropertyValue("TableBorder");
            // Create a TableBorder for the cell
            TableBorder tableBorder = new TableBorder();
            // Top border
            unoidl.com.sun.star.table.BorderLine topBorder = new unoidl.com.sun.star.table.BorderLine(0, 0, (short)_borderWidth, 0);
            // Right border
            unoidl.com.sun.star.table.BorderLine rightBorder = new unoidl.com.sun.star.table.BorderLine(0, 0, (short)_borderWidth, 0);
            // Bottom border
            unoidl.com.sun.star.table.BorderLine bottomBorder = new unoidl.com.sun.star.table.BorderLine(0, 0, (short)_borderWidth, 0);
            // Left border
            unoidl.com.sun.star.table.BorderLine leftBorder = new unoidl.com.sun.star.table.BorderLine(0, 0, (short)_borderWidth, 0);
            // Assign the BorderLine objects to the TableBorder
            tableBorder.TopLine = topBorder;
            tableBorder.RightLine = rightBorder;
            tableBorder.BottomLine = bottomBorder;
            tableBorder.LeftLine = leftBorder;
            tableBorder.IsBottomLineValid = true;
            tableBorder.IsTopLineValid = true;
            tableBorder.IsRightLineValid = true;
            tableBorder.IsLeftLineValid = true;
            // Apply the border properties to the cell
            xCellRangeProps.setPropertyValue("TableBorder", new uno.Any(typeof(TableBorder), tableBorder));
            Console.WriteLine("Borders for cell A2 have been set.");

        }
        /// <summary>
        /// Встановлює значення у вказану комірку LibreOffice Calc.
        /// </summary>
        /// <param name="_cellValue">Текстове або числове значення для вставки.</param>
        /// <param name="_cellCoord">Координати комірки у форматі (X, Y).</param>
        public void SetValue(string _cellValue, System.Drawing.Point _cellCoord)
        {
            if(xSheet == null)
            {
                throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before setting a value.");
            }
            XCell xCellA2 = xSheet.getCellByPosition(_cellCoord.X, _cellCoord.Y); // A2 -> (0,1)
            xCellA2.setFormula(_cellValue);
        }
        /// <summary>
        /// Зберігає поточний документ LibreOffice Calc за вказаним шляхом.
        /// </summary>
        /// <param name="filePath">Шлях до файлу для збереження (наприклад, "C:\\Users\\User\\Documents\\output.ods").</param>
        public void Save(string filePath)
        {
            XStorable xStorable = xSpreadsheetDoc as XStorable;
            if (xStorable == null)
            {
                throw new InvalidOperationException("The spreadsheet document is not initialized. Ensure that the document is opened before saving.");
            }
            // Збереження файлу
            PropertyValue[] storeProps = new PropertyValue[1];
            storeProps[0] = new PropertyValue { Name = "FilterName", Value = new uno.Any("calc8") };           
            string saveString = new Uri(filePath).AbsoluteUri;
            xStorable.storeAsURL(saveString, storeProps); 
        }
        /// <summary>
        /// Закриває поточний документ LibreOffice Calc, зберігаючи його за необхідності.
        /// </summary>
        /// <param name="filePath">Необов'язковий параметр. Шлях до файлу для збереження перед закриттям.</param>
        public void Close(string filePath = "")
        {
            try
            {
                // Якщо передано шлях, зберігаємо файл перед закриттям
                if (!string.IsNullOrEmpty(filePath))
                {
                    Save(filePath);
                }

                // Отримуємо інтерфейс XCloseable для закриття документа
                unoidl.com.sun.star.util.XCloseable xCloseable = xSpreadsheetComponent as unoidl.com.sun.star.util.XCloseable;
                if (xCloseable != null)
                {
                    xCloseable.close(true);
                }
                else
                {
                    if(xSpreadsheetComponent != null)
                    {
                        xSpreadsheetComponent.dispose();
                    }
                }

                Console.WriteLine($"Документ успішно закрито{(string.IsNullOrEmpty(filePath) ? "." : $" і збережено за шляхом: {filePath}")}.");
            }
            catch (DisposedException ex)
            {
                Console.WriteLine($"Документ вже був закритий: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Помилка під час закриття документа: {ex.Message}");
            }

            finally
            {
                // Закриваємо процес LibreOffice, якщо він ще активний
                if (sofficeProcess != null && !sofficeProcess.HasExited)
                {
                    sofficeProcess.Kill();
                    sofficeProcess.WaitForExit();
                    Console.WriteLine("Процес LibreOffice закрито.");
                }

                // Примусове звільнення пам'яті
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        /// <summary>
        /// Встановлює числове значення у вказану комірку LibreOffice Calc.
        /// </summary>
        /// <param name="_cellValue">Числове значення для вставки.</param>
        /// <param name="_cellCoord">Координати комірки у форматі (X, Y).</param>
        public void SetValue(double _cellValue, System.Drawing.Point _cellCoord)
        {
            if (xSheet == null)
            {
                throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before setting a value.");
            }

            xSheet.getCellByPosition(_cellCoord.X, _cellCoord.Y).setValue(_cellValue);
        }
        /// <summary>
        /// Друкує документ LibreOffice Calc на вказаному принтері.
        /// </summary>
        /// <param name="filePath">Шлях до файлу, який потрібно відкрити перед друком.</param>
        /// <param name="printerName">Назва принтера, на який буде відправлено документ.</param>
        public void Print(string filePath, string printerName)
        {
            try
            {
                // Відкриваємо файл
                Open(filePath);

                // Налаштування принтера
                PropertyValue[] printProps = new PropertyValue[1];
                printProps[0] = new PropertyValue { Name = "Name", Value = new uno.Any(printerName) };

                // Відправлення на друк
                XPrintable xPrintable = xSpreadsheetComponent as XPrintable;
                if (xPrintable==null)
                {
                    throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before setting a value.");

                }
                xPrintable.setPrinter(printProps);
                xPrintable.print(printProps);

                Console.WriteLine($"Файл '{filePath}' надіслано на друк на принтер '{printerName}'.");

                // Закриття файлу після друку
                Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Помилка під час друку: {ex.Message}");
            }
        }
        /// <summary>
        /// Встановлює колір фону комірки LibreOffice Calc за вказаними координатами.
        /// </summary>
        /// <param name="_color">Колір у форматі ARGB (наприклад, жовтий — 0xFFFF00).</param>
        /// <param name="_cellCoord">Координати комірки у форматі (X, Y).</param>
        public void SetColor(int _color, System.Drawing.Point _cellCoord)
        {
            // Отримуємо комірку за вказаними координатами
            if (xSheet == null)
            {
                throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before setting a color.");
            }
            XCell xRange = xSheet.getCellByPosition(_cellCoord.X, _cellCoord.Y); // Користувач визначає координати (X, Y)

            // Отримуємо властивості комірки
            XPropertySet xPropSet = (XPropertySet)xRange;

            // Встановлюємо колір фону комірки
            xPropSet.setPropertyValue("CellBackColor", new uno.Any(_color)); // Задаємо колір фону
        }
        public List<string> Read(string filePath = "", int maxRows = 1, int maxCols = 1)
        {
            List<string> rows = new List<string>();

            // Зчитуємо дані з файлу
            maxRows = 10;  // Ліміт рядків (можна збільшити, якщо треба)
            maxCols = 10;   // Ліміт колонок

            for (int row = 0; row < maxRows; row++)
            {
                List<string> rowData = new List<string>();

                for (int col = 0; col < maxCols; col++)
                {
                    if(xSheet == null)
                    {
                        throw new InvalidOperationException("The spreadsheet is not initialized. Ensure that the document is opened before reading data.");
                    }
                    XCell xCell = xSheet.getCellByPosition(col, row);
                    string cellValue = GetCellValue(xCell);
                    rowData.Add(cellValue);
                }

                string rowString = string.Join(";", rowData);
                rows.Add(rowString);
            }
            return rows;
        }
        // Функція отримання значення з комірки
        static string GetCellValue(XCell xCell)
        {
            switch (xCell.getType())
            {
                case unoidl.com.sun.star.table.CellContentType.TEXT:
                    return xCell.getFormula();
                case unoidl.com.sun.star.table.CellContentType.VALUE:
                    return xCell.getValue().ToString();
                case unoidl.com.sun.star.table.CellContentType.EMPTY:
                    return "";
                default:
                    return "";
            }
        }
        public void ExportToPDF(string pdfPath)
        {
            // Встановлюємо параметри для експорту в PDF
            PropertyValue[] exportProps = new PropertyValue[1];
            exportProps[0] = new PropertyValue { Name = "FilterName", Value = new uno.Any("calc_pdf_Export") };

            // Зберігаємо файл у PDF
            XStorable xStorable = xSpreadsheetComponent as XStorable;
            if (xStorable != null)
            {
                xStorable.storeToURL(pdfPath, exportProps);
            }
            Console.WriteLine("Файл успішно збережено в PDF: " + pdfPath);
        }
    }

}
