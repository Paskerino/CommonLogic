using CommonLogic.Core.Models;
using CommonLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLogic.Services.Workers
{
    public class PollingLoopWorker
    {
        private readonly IModbusService _modbusService;
        private readonly Device _device;
        private readonly int _interval;
        private readonly CancellationToken _token;

        public event Action<List<SensorReading>> DataReceived;
        private readonly ManualResetEventSlim _pauseEvent; // "Прапорець" для паузи
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public PollingLoopWorker(
            IModbusService modbusService,
            Device device,
            int interval,
            CancellationToken token,
            ManualResetEventSlim pauseEvent)
        {
            _modbusService = modbusService;
            _device = device;
            _interval = interval;
            _token = token;
            _pauseEvent = pauseEvent;
        }

        public async Task ExecuteAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Run(() => _pauseEvent.Wait(_token));
                try
                {
                    var readings = await _modbusService.ReadRegistersAsync(_device);
                    if (readings != null)
                    {
                        foreach (var reading in readings)
                        {
                            // Створюємо структурований лог
                            Logger.Trace("Sensor reading received. SensorId={SensorId}, Value={SensorValue}, Timestamp={Timestamp}",
                                        reading.SensorId,
                                        reading.Value,
                                        reading.Timestamp);
                        }
                        DataReceived?.Invoke(readings);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PollingLoopWorker Error] {ex.Message}");
                }

                try
                {
                    await Task.Delay(_interval, _token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
}
