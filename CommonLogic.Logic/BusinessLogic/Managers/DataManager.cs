using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.Core.Models.Reports;
using CommonLogic.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLogic.Core.DAL.Interfaces;

namespace CommonLogic.BusinessLogic.Managers
{
    // MySuperApp.BusinessLogic/Managers/DataManager.cs
    public class DataManager : IDataManager
    {
        private readonly IModbusService _modbusService;
        private readonly IReadingRepository _readingRepository;
        private readonly List<Device> _configuredDevices;
        private readonly List<TriggerRule> _rules;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public DataManager(
        IModbusService modbusService,
        IReadingRepository readingRepository,
        List<Device> configuredDevices,
         List<TriggerRule> rules)
        {
            _modbusService = modbusService;
            _readingRepository = readingRepository;
            _configuredDevices = configuredDevices;
            _rules = rules;
        }
        public async Task<GenericReportData> CreateReportFromTodayLogAsync()
        {
            // 1. Просимо DAL дати нам всі дані за сьогодні
            var readings = await _readingRepository.GetTodaysReadingsAsync();

            // 2. Створюємо об'єкт звіту (це логіка, яку ми вже обговорювали)
            var report = new GenericReportData
            {
                Title = $"Звіт за {DateTime.Now:dd MMMM yyyy}"
            };
            report.Headers.Cells.Add(new ReportCell { Value = "Час" });
            report.Headers.Cells.Add(new ReportCell { Value = "ID Датчика" });
            report.Headers.Cells.Add(new ReportCell { Value = "Значення" });

            foreach (var reading in readings)
            {
                var row = new ReportRow();
                row.Cells.Add(new ReportCell { Value = reading.Timestamp.ToString("HH:mm:ss.fff") });
                row.Cells.Add(new ReportCell { Value = reading.SensorId.ToString() });
                row.Cells.Add(new ReportCell { Value = reading.Value.ToString("F2") });
                report.DataRows.Add(row);
            }

            return report;
        }
        public async Task ProcessSingleReadingAsync(List<SensorReading> reading)
        {
            // Тут логіка обробки і збереження
            await _readingRepository.SaveReadingsToFileAsync(reading);
        }
        public async Task<bool> WriteValueToSensorAsync(Device device, ushort register, ushort value)
        {
            bool result = await _modbusService.WriteRegisterAsync(device, register, value);
            return result;
        }
        public List<WriteCommand> ProcessAndDecideSingleReading(SensorReading reading)
        {

            // Знаходимо всі правила, що стосуються нашого датчика
            var applicableRules = _rules.Where(r => r.SourceSensorId == reading.SensorId);

            var commandsToExecute = new List<WriteCommand>();

            foreach (var rule in applicableRules)
            {
                // Перевіряємо, чи виконується умова
                bool conditionMet = false;
                if (rule.Condition == TriggerCondition.GreaterThan && reading.Value > rule.ThresholdValue)
                {
                    conditionMet = true;
                }
                if (rule.Condition == TriggerCondition.LessThan && reading.Value < rule.ThresholdValue)
                {
                    conditionMet = true;
                }
                if (rule.Condition == TriggerCondition.EqualTo && reading.Value == rule.ThresholdValue)
                {
                    conditionMet = true;
                }

                if (conditionMet)
                {
                    Logger.Info($"Спрацювало правило для датчика {reading.SensorId}.");
                    // Знаходимо пристрій, якому належить датчик
                    Device targetDevice = _configuredDevices.FirstOrDefault(d => d.Sensors.Any(s => s.Id == reading.SensorId));

                    if (targetDevice != null)
                    {
                        // Створюємо команди на запис на основі дій з правила
                        foreach (var action in rule.Actions)
                        {
                            commandsToExecute.Add(new WriteCommand(
                                 targetDevice,
                               action.RegisterAddress,
                                action.ValueToWrite
                            ));
                        }
                    }
                }
            }

            return commandsToExecute.Any() ? commandsToExecute : null;
        }


    }
}
