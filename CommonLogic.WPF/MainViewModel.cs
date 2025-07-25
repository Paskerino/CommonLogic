using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading; // Важливо!

namespace CommonLogic.WPF
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataManager _dataManager;
        private readonly IModbusPollingService _modbusPolling;
        private readonly List<Device> _availableDevices;

        private string _sensorValue;
        public string SensorValue
        {
            get => _sensorValue;
            set
            {
                _sensorValue = value;
                OnPropertyChanged(); // Повідомляємо UI, що дані змінилися
            }
        }
        private string _pressureValue;
        public string PressureValue
        {
            get => _pressureValue;
            set { _pressureValue = value; OnPropertyChanged(); }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set { _currentTime = value; OnPropertyChanged(); }
        }
        private string _targetValue = "123.4"; // Значення, яке ми будемо редагувати
        public string TargetValue
        {
            get => _targetValue;
            set { _targetValue = value; OnPropertyChanged(); }
        }
        private string _serialNumber = "Введіть серійний номер";
        public string SerialNumber
        {
            get => _serialNumber;
            set { _serialNumber = value; OnPropertyChanged(); }
        }
        private string _operatorCode = "Введіть код оператора";
        public string OperatorCode
        {
            get => _operatorCode;
            set { _operatorCode = value; OnPropertyChanged(); }
        }

        
        private string _activePropertyName;
        private Action<string> _activeInputSetter;
        public ICommand StartPollingCommand { get; }
        public ICommand StopPollingCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ShowInputCommand { get; }//
        public ICommand AcceptInputCommand { get; }//
        public ICommand CancelInputCommand { get; }//
        public ICommand OpenKeypadCommand { get; } 


        public MainViewModel(IDataManager dataManager, IModbusPollingService modbusPolling, List<Device> devices)
        {
            _dataManager = dataManager;
            _modbusPolling = modbusPolling;
            _availableDevices = devices;

            _modbusPolling.DataReceived += OnDataReceived;
            StartPollingCommand = new RelayCommand(StartPolling);
            ExitCommand = new RelayCommand(ExitApplication);
            ShowInputCommand = new RelayCommand(ShowInput);

            // Таймер для годинника на екрані
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => { CurrentTime = DateTime.Now.ToString("HH:mm:ss"); };
            timer.Start();
            PressureValue = "0.0"; // Ініціалізуємо значення тиску
        }


        private void ShowInput(object parameter)
        {
            if (parameter is string propertyName)
            {
                // Отримуємо поточне значення властивості за допомогою рефлексії
                PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName);
                string initialValue = propertyInfo?.GetValue(this)?.ToString() ?? "";

                // Створюємо і показуємо наше нове вікно
                var dialog = new InputDialog(initialValue);

                // dialog.ShowDialog() - показує вікно модально (блокує основне)
                if (dialog.ShowDialog() == true)
                {
                    // Якщо користувач натиснув OK, оновлюємо властивість
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(this, dialog.ResultText);
                    }
                }
            }
        }


        private void ExitApplication(object parameter)
        {
            Application.Current.Shutdown();
        }
        private void OnDataReceived(List<SensorReading> readings)
        {
            var pressureSensor = readings.FirstOrDefault(r => r.SensorId == 1); // Шукаємо наш датчик тиску
            if (pressureSensor != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PressureValue = pressureSensor.Value.ToString("F1");
                });
            }
            var firstReading = readings[0];
            // Оновлюємо властивість. UI оновить себе автоматично!
            SensorValue = firstReading.Value.ToString("F2");
        }

        private void StartPolling(object parameter)
        {
            Device plc1 = _availableDevices[0];
            int interval = 7;
            _modbusPolling.StartPolling(plc1, interval);
        }
    }
}