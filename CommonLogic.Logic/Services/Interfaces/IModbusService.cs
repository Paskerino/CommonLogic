using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Services.Interfaces
{
    /// <summary>
    /// Надає контракт для сервісу, що виконує разові
    /// операції читання даних по протоколу Modbus.
    /// </summary>
    public interface IModbusService
    {
        /// <summary>
        /// Асинхронно читає поточне значення з вказаного датчика.
        /// </summary>
        /// <param name="sensor">Об'єкт датчика, з якого потрібно прочитати дані.</param>
        /// <returns>
        /// Об'єкт SensorReading з отриманими даними або null, якщо сталася помилка.
        /// </returns>
        Task<List<SensorReading>> ReadRegistersAsync(Device device);
        Task<bool> WriteRegisterAsync(Device device, ushort registerAddress, ushort value);
    }
}
