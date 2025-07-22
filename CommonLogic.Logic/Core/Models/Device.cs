using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{
    
        // Цей клас описує фізичний пристрій (PLC, контролер)
        public class Device
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;

            // Параметри підключення тепер тут
            public string IpAddress { get; set; } = "127.0.0.1";
            public int Port { get; set; } = 502;
            public byte SlaveId { get; set; } = 1;

            // Пристрій "знає" про свої датчики
            public List<Sensor> Sensors { get; set; } = new List<Sensor>();
        }
}

