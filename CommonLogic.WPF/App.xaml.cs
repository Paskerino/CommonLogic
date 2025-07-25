using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.BusinessLogic.Managers;
using CommonLogic.Core.DAL.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.DAL.Implementation;
using CommonLogic.Services.Implementations;
using CommonLogic.Services.Interfaces;
using CommonLogic.Services.Workers;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace CommonLogic.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // --- Крок 0: Завантаження конфігурації (так само, як було) ---
            var configLoader = new ConfigurationLoader();
            List<Device> allDevices = configLoader.LoadDevices();
            List<TriggerRule> allRules = configLoader.LoadTriggerRules();

            // --- Крок 1: Створюємо сервіси (так само, як було) ---
            IReadingRepository readingRepo = new FileReadingRepository();
            IModbusService singleShotService = new ModbusService();
            IModbusPollingService pollingService = new ModbusPollingService(singleShotService);
            IReportService reportService = new ReportService();

            // --- Крок 2: Створюємо бізнес-логіку (так само, як було) ---
            IDataManager dataManager = new DataManager(singleShotService, readingRepo, allDevices, allRules);

            // --- Крок 3: Створюємо ViewModel ---
            var viewModel = new MainViewModel(dataManager, pollingService, allDevices);

            // --- Крок 4: Створюємо View і передаємо їй ViewModel ---
            var mainWindow = new MainWindow
            {
                DataContext = viewModel // Це ключовий рядок, що все пов'язує!
            };

            mainWindow.Show();
        }
    }
}