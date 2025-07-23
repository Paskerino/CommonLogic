using CommonLogic.Core.Models;
using CommonLogic.Services.Interfaces;
using CommonLogic.Services.Workers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLogic.Services.Implementations
{
    public class ModbusPollingService : IModbusPollingService
    {
        private readonly IModbusService _modbusService;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true); // true = не на паузі
        private Task _pollingTask;

        public event Action<List<SensorReading>> DataReceived;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public ModbusPollingService(IModbusService modbusService)
        {
            _modbusService = modbusService;
        }

        public void StartPolling(Device device, int intervalMilliseconds)
        {
            if (_pollingTask != null && !_pollingTask.IsCompleted) return;

            _cancellationTokenSource = new CancellationTokenSource();
            Logger.Info($"Запуск опитування для пристроя '{device.Name}' з інтервалом {intervalMilliseconds} мс.");
            // Задаємо ім'я лог-файлу для цього пристрою
            string customLogName = "Stand"; // Це ім'я
            if (LogManager.Configuration != null)
            {
                LogManager.Configuration.Variables["logFileName"] = customLogName;
            }
            else
            {
                Logger.Warn("LogManager.Configuration is null. Unable to set custom log file name.");
            }
            var worker = new PollingLoopWorker(
                _modbusService,
                device,
                intervalMilliseconds,
                _cancellationTokenSource.Token,
                _pauseEvent);

            worker.DataReceived += (readings) => DataReceived?.Invoke(readings);
            _pollingTask = Task.Run(() => worker.ExecuteAsync());
        }

        public void StopPolling()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _pollingTask = null;
        }

        public void Pause()
        {
            _pauseEvent.Reset();
            Logger.Debug("Опитування призупинено.");
        }

        public void Resume()
        {
            _pauseEvent.Set();
            Logger.Debug("Опитування відновлено.");
        }

        public Task WriteRegisterAsync(Device targetDevice, ushort registerAddress, ushort valueToWrite)
        {
            return _modbusService.WriteRegisterAsync(targetDevice, registerAddress, valueToWrite);

        }
    }
}