using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.DAL.Interfaces
{
    public interface IReadingRepository
    {
        // CREATE
        /// <summary>
        /// Зберігає новий запис про показання.
        /// </summary>
        Task SaveReadingsToFileAsync(IEnumerable<SensorReading> readings);

        // READ
        /// <summary>
        /// Отримує історію показань для певного датчика за період.
        /// </summary>
        Task<IEnumerable<SensorReading>> GetTodaysReadingsAsync();

        // UPDATE (опціонально, якщо потрібно)
        /// <summary>
        /// Оновлює існуючий запис.
        /// </summary>
        void Update(SensorReading readingToUpdate);

        // DELETE (опціонально, якщо потрібно)
        /// <summary>
        /// Видаляє конкретний запис.
        /// </summary>
        void Delete(int readingId);
    }
}
