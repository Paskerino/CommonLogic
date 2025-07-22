using CommonLogic.Core.Models;
using CommonLogic.Core.Models.Reports;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.BusinessLogic.Interfaces
{
    public interface IDataManager
    {
        /// <summary>
        /// Отримує поточне значення з датчика, обробляє його
        /// і зберігає в репозиторій.
        /// </summary>
        //Task ProcessSingleReadingAsync(Sensor sensor);

      //  Task<SensorReading?> GetAndStoreSensorReadingAsync(int sensorId);
       // Task ProcessSingleReadingAsync(SensorReading reading);
        /// <summary>
        /// Готує дані для звіту по тиску.
        /// </summary>
       // Task<GenericReportData> CreatePressureReportAsync(int sensorId, DateTime from, DateTime to);
        Task<bool> WriteValueToSensorAsync(Device device, ushort register,ushort value);
        Task ProcessSingleReadingAsync(List<SensorReading> reading);
        Task<GenericReportData> CreateReportFromTodayLogAsync();
        List<WriteCommand> ProcessAndDecideSingleReading(SensorReading reading);
    }
}
