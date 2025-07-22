using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Enum
{
    
        public enum SensorType
        {
            Pressure,       // Тиск
            Temperature,    // Температура
            Speed,          // Швидкість
            Force,          // Сила
            Distance,       // Відстань
            Resistance,     // Опір (для мегомметра)
                            // Можна додати інші, якщо потрібно
        }
    
}
