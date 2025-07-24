using ClosedXML.Excel;
using CommonLogic.Core.Models.Reports;
using CommonLogic.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CommonLogic.Services.Implementations
{
    public class ReportService : IReportService
    {
        // Цей сервіс тепер не має зовнішніх залежностей, тому конструктор порожній
        public ReportService() { }

        public Task CreateReportFileAsync(GenericReportData reportData, string templateReport, string pathReport)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(reportData.Title);

                    // --- Створення шапки звіту ---
                    int currentRow = 1;
                    // Приклад складної шапки: об'єднаємо комірки для головного заголовка
                    worksheet.Cell(currentRow, 1).Value = reportData.Title;
                    var titleRange = worksheet.Range(currentRow, 1, currentRow, reportData.Headers.Cells.Count);
                    titleRange.Merge().Style.Font.Bold = true;
                    titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    // --- Заголовки стовпців ---
                    int currentColumn = 1;
                    foreach (var headerCell in reportData.Headers.Cells)
                    {
                        worksheet.Cell(currentRow, currentColumn).Value = headerCell.Value;
                        currentColumn++;
                    }
                    worksheet.Row(currentRow).Style.Font.Bold = true;
                    currentRow++;

                    // --- Заповнення даними ---
                    foreach (var dataRow in reportData.DataRows)
                    {
                        currentColumn = 1;
                        foreach (var cell in dataRow.Cells)
                        {
                            worksheet.Cell(currentRow, currentColumn).Value = cell.Value;
                            if (cell.IsHighlighted)
                            {
                                worksheet.Cell(currentRow, currentColumn).Style.Font.FontColor = XLColor.Red;
                            }
                            currentColumn++;
                        }
                        currentRow++;
                    }

                    // Автоматично налаштовуємо ширину всіх стовпців по вмісту
                    worksheet.Columns().AdjustToContents();

                    // Зберігаємо файл (тепер у форматі .xlsx, бо він "рідний" для ClosedXML)
                    workbook.SaveAs(pathReport.Replace(".ods", ".xlsx"));
                }
            }
            catch (Exception ex)
            {
                // Тут має бути твій логгер
                Console.WriteLine($"Помилка при створенні звіту за допомогою ClosedXML: {ex.Message}");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}