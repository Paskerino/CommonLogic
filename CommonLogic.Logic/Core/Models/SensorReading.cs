using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{

    using System;

    public class SensorReading
    {
        // Ідентифікатор датчика, з якого отримано це показання
        public int SensorId { get; set; }

        // Виміряне значення
        public double Value { get; set; }

        // Час, коли було отримано це показання
        public DateTime Timestamp { get; set; }
    }
}
