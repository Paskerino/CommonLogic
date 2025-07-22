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
        private async Task ExecuteWriteCommand(List<WriteCommand> command)
        {
            if (command == null || !command.Any())
            {
                _view.ShowStatus("Немає команд для виконання.");
                return;
            }
            foreach (var cmd in command)
            {
                _view.ShowStatus($"Виконується команда запису в регістр {cmd.RegisterAddress}...");
                _modbusPolling.Pause();
                try
                {
                    await _modbusPolling.WriteRegisterAsync(cmd.TargetDevice, cmd.RegisterAddress, cmd.ValueToWrite);
                }
                finally
                {
                    _modbusPolling.Resume();
                    _view.ShowStatus("Команду запису виконано.");
                }
            }
            
        }
    }
}
