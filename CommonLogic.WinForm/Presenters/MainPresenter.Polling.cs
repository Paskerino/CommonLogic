using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Presenters
{
    public partial class MainPresenter
    {
        private void OnStartPollingClicked(object sender, EventArgs e)
        {   
            Device plc1 = _availableDevices.FirstOrDefault(d => d.Id == 1);
            if(plc1 == null)
            {
                _view.ShowStatus("Пристрій з не знайдено.");
                return;
            }
            _view.ShowStatus($"Запуск фонового опитування...{plc1.Name}");

            int interval = 7; // Опитувати приблизно 15 разів на секунду
            _modbusPolling.StartPolling(plc1, interval);
        }
    }
}
