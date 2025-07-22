using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{
    /// <summary>
    /// Описує команду на запис одного значення в регістр.
    /// </summary>
    public class WriteCommand
    {
        public Device TargetDevice { get; set; }
        public ushort RegisterAddress { get; set; }
        public ushort ValueToWrite { get; set; }

        // Створюємо конструктор, який вимагає всі потрібні дані
        public WriteCommand(Device targetDevice, ushort registerAddress, ushort valueToWrite)
        {
            TargetDevice = targetDevice;
            RegisterAddress = registerAddress;
            ValueToWrite = valueToWrite;
        }
    }
}
