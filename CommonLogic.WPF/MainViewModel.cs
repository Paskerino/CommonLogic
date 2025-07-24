using CommonLogic.BusinessLogic.Interfaces;
using CommonLogic.Core.Models;
using CommonLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Windows.Input; // Важливо!

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

        public ICommand StartPollingCommand { get; }

        public MainViewModel(IDataManager dataManager, IModbusPollingService modbusPolling, List<Device> devices)
        {
            _dataManager = dataManager;
            _modbusPolling = modbusPolling;
            _availableDevices = devices;

            _modbusPolling.DataReceived += OnDataReceived;
            StartPollingCommand = new RelayCommand(StartPolling);
        }

        private void OnDataReceived(List<SensorReading> readings)
        {
            // Для прикладу беремо лише перше значення
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