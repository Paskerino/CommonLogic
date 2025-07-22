using CommonLogic.Core.DAL.Interfaces;
using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.DAL.Implementation
{
    public class FileReadingRepository : IReadingRepository
    {
        private readonly string _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Readings");
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Delete(int readingId)
        {
            throw new NotImplementedException();
        }

        
           public async Task<IEnumerable<SensorReading>> GetTodaysReadingsAsync()
        {
            string filePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.csv");
            var readings = new List<SensorReading>();

            if (!File.Exists(filePath))
            {
                Logger.Warn($"Файл логів за сьогодні ({filePath}) не знайдено.");
                return readings; // Повертаємо порожній список
            }

            try
            {
                // Читаємо всі рядки з файлу
                string[] lines =  File.ReadAllLines(filePath);

                // Пропускаємо перший рядок (заголовок) і парсимо решту
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 3)
                    {
                        readings.Add(new SensorReading
                        {
                            Timestamp = DateTime.Parse(parts[0]),
                            SensorId = int.Parse(parts[1]),
                            Value = double.Parse(parts[2])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Помилка при читанні файлу логів.");
            }

            return readings;
        }

        public FileReadingRepository()
        {
            Directory.CreateDirectory(_logDirectory);
        }


        public async Task SaveReadingsToFileAsync(IEnumerable<SensorReading> readings)
        {
            if (readings == null || !readings.Any())
            {
                return; // Fix: Return a completed Task when there is nothing to process
            }
            try
            {
                // Визначаємо ім'я файлу на основі сьогоднішньої дати
                string filePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.csv");

                // Перевіряємо, чи потрібно додати заголовок (якщо файл новий)
                bool fileExists = File.Exists(filePath);

                // Використовуємо StringBuilder для ефективного формування рядків
                var csvBuilder = new StringBuilder();

                if (!fileExists)
                {
                    csvBuilder.AppendLine("Timestamp;SensorId;Value");
                }

                // Додаємо всі показання в StringBuilder
                foreach (var reading in readings)
                {
                    // Форматуємо рядок: час у форматі ISO 8601; ID датчика; значення
                    csvBuilder.AppendLine($"{reading.Timestamp:o};{reading.SensorId};{reading.Value}");
                }

                // Записуємо все в кінець файлу за одну операцію
                 File.AppendAllText(filePath, csvBuilder.ToString());
            }
            catch (Exception ex)
            {
                // Повертаємо Task з помилкою
                Logger.Error(ex, "Помилка при збереженні показань у CSV-файл.");
            }
        }


        public void Update(SensorReading readingToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
