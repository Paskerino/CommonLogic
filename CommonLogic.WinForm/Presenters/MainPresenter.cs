using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.Core.Models.Reports;
using CommonLogic.Services.Implementations;
using CommonLogic.Services.Interfaces;
using CommonLogic.Views;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLogic.Presenters
{
    public partial class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDataManager _dataManager;
        private readonly IModbusPollingService _modbusPolling;
        private readonly List<Device> _availableDevices;
        private readonly IReportService _reportService;


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MainPresenter(
            IMainView view,
            IDataManager dataManager,
            IModbusPollingService modbusPolling,
            IReportService reportService,
             List<Device> availableDevices)
        {
            _view = view;
            _dataManager = dataManager;
            _modbusPolling = modbusPolling;
            _availableDevices = availableDevices;
            _reportService = reportService;

            _view.StartPollingClicked += OnStartPollingClicked;//MainPresenter.Polling
            _modbusPolling.DataReceived += OnDataReceived;
            _view.GenerateReport += OnGenerateReportClicked;  // Ініціалізація кнопки генерації звіту
        }
        Stopwatch debugStopwatch = new Stopwatch();
        private async void OnDataReceived(List<Core.Models.SensorReading> readings)
        {
            // await _dataManager.ProcessSingleReadingAsync(readings);
            foreach (var reading in readings)
            {
                _view.DisplaySensorValue(reading.SensorId, reading.Value.ToString("F2"));
                // await _dataManager.ProcessSingleReadingAsync(reading);
                var writeCommand = _dataManager.ProcessAndDecideSingleReading(reading);
                if (writeCommand != null)
                {
                    await ExecuteWriteCommand(writeCommand);
                }
            }
        }
        private async void OnGenerateReportClicked(object sender, GenerateReportEventArgs e)
        {
            _view.ShowStatus("Зупинка опитування...");
            // 1. Зупиняємо логування (опитування)
            _modbusPolling.StopPolling();

            _view.ShowStatus("Генерація звіту з залогованих даних...");
            try
            {
                // 2. Даємо команду бізнес-логіці створити звіт
                var reportData = await _dataManager.CreateReportFromTodayLogAsync();

                // 3. Передаємо готовий звіт сервісу для створення файлу
                // reportData,pathToTemplate,pathToSave
                string templatePath = Path.Combine(Application.StartupPath, "ReportTempalets\\TestTemplate.ods");
                string savePath = e.ReportName;
                await _reportService.CreateReportFileAsync(reportData, templatePath,savePath);

                _view.ShowStatus("Звіт успішно створено!");
            }
            catch (Exception ex)
            {
                _view.ShowStatus("Помилка при створенні звіту.");
                Logger.Error(ex, "Не вдалося згенерувати звіт.");
            }
        }
    }
}

