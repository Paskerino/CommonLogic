using CommonLogic.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{
    // MySuperApp.Core/Models/Sensor.cs

    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SensorType Type { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;

        // Залишаємо тільки те, що унікальне для датчика
        public ushort RegisterAddress { get; set; } = 0;

        // Посилання на ID пристрою, якому він належить
        public int DeviceId { get; set; }
    }
}
