// Program.cs у проекті CommonLogic.WinForm

// Додаємо using для всіх шарів, які нам потрібні
using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.BusinessLogic.Managers;
using CommonLogic.Core.DAL.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.DAL.Implementation;
using CommonLogic.Presenters;
using CommonLogic.Services.Implementations;
using CommonLogic.Services.Interfaces;
using CommonLogic.Services.Workers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CommonLogic.WinForm
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // --- Крок 0: Завантаження конфігурації ---
            // Логіка завантаження конфігурації може бути винесена в окремий клас
            // або залишатися тут, якщо вона проста.
            var configLoader = new ConfigurationLoader(); // Можливо, цей клас теж варто перенести в Services
            List<Device> allDevices = configLoader.LoadDevices();
            List<TriggerRule> allRules = configLoader.LoadTriggerRules();

            // --- Крок 1: Створюємо реалізації сервісів та репозиторіїв (DAL, Services) ---
            IReadingRepository readingRepo = new FileReadingRepository(); // Приклад з шляхом
            IModbusService singleShotService = new ModbusService();
            IModbusPollingService pollingService = new ModbusPollingService(singleShotService);
            IReportService reportService = new ReportService();

            // --- Крок 2: Створюємо екземпляр бізнес-логіки, передаючи йому сервіси ---
            IDataManager dataManager = new DataManager(singleShotService, readingRepo, allDevices, allRules);

            // --- Крок 3: Створюємо UI (View) та його Presenter ---
            // Presenter є диригентом, тому ми передаємо йому всі необхідні залежності.
            MainForm mainForm = new MainForm(allDevices); // View не знає ні про що, крім себе
            MainPresenter presenter = new MainPresenter(mainForm, dataManager, pollingService, reportService, allDevices);

            // --- Крок 4: Запускаємо застосунок ---
            Application.Run(mainForm);
        }
    }
}