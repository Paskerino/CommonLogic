using CommonLogic.Core.Models;
using CommonLogic.Presenters;
using CommonLogic.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLogic.WinForm
{
    public partial class MainForm : Form, IMainView
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly List<Device> _allDevices;
        private Dictionary<int, Action<string>> _registerLabels;
        public MainForm(List<Device> allDevices)
        {
            InitializeComponent();
            InitializeSensorMapping();
            _allDevices = allDevices;
        }
        private void InitializeSensorMapping()
        {
            _registerLabels = new Dictionary<int, Action<string>>
            {
                { 1, (value) => {
                sensor1ValueLabel.Text = value;
                button1.BackColor = Color.Red;
              }
            }
            };
        }

        public event EventHandler StartPollingClicked;
        public event EventHandler<GenerateReportEventArgs> GenerateReport;
        private MainPresenter _presenter;


        public void ShowStatus(string message)
        {
           // throw new NotImplementedException();
        }
        Timer timer = new Timer();
        private void button1_Click(object sender, EventArgs e)
        {
            Logger.Info($"Запуск опитування для пристроя  з інтервалом ___ мс.");

            StartPollingClicked?.Invoke(this, EventArgs.Empty);

            timer.Interval = 1000; // 1000 мс = 1 секунда
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            string reportName = Path.Combine(Application.StartupPath, "Report\\ReportForStand" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".ods");

            // Створюємо екземпляр нашого класу з даними
            var eventArgs = new GenerateReportEventArgs(reportName);

            GenerateReport?.Invoke(this, eventArgs);

        }
        public void SetPresenter(MainPresenter presenter)
        {
            _presenter = presenter;
        }

        public void DisplaySensorValue(int Id, string value)
        {
            // Знаходимо потрібний Label у словнику
            if (_registerLabels != null && _registerLabels.TryGetValue(Id, out Action<string> updateAction))
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => updateAction(value)));
                }
                else
                {
                    updateAction(value);
                }
            }
        }
    }
}
