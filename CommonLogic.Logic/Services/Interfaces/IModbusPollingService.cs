using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Services.Interfaces
{

    /// <summary>
    /// Надає контракт для сервісу, що керує фоновим
    /// процесом постійного опитування пристрою.
    /// </summary>
    public interface IModbusPollingService
    {
        /// <summary>
        /// Подія, що спрацьовує кожного разу, коли отримано нове показання.
        /// </summary>
        event Action<List<Core.Models.SensorReading>> DataReceived;

        /// <summary>
        /// Запускає фоновий процес опитування для вказаного датчика
        /// з заданим інтервалом.
        /// </summary>
        /// <param name="sensor">Датчик, який потрібно опитувати.</param>
        /// <param name="intervalMilliseconds">Інтервал між запитами в мілісекундах.</param>
        void StartPolling(Device device, int intervalMilliseconds);

        /// <summary>
        /// Зупиняє фоновий процес опитування.
        /// </summary>
        void StopPolling();
        void Pause();

        void Resume();
        Task WriteRegisterAsync(Device targetDevice, ushort registerAddress, ushort valueToWrite);
    }
}
