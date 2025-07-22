using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{

    using System.Collections.Generic;

    public class TestStand
    {
        // Унікальний ідентифікатор стенда
        public int Id { get; set; }

        // Назва стенда
        public string Name { get; set; } = string.Empty;

        // Опис, для чого цей стенд призначений
        public string Description { get; set; } = string.Empty;

        // Список датчиків, які встановлені на цьому стенді.
        // Це створює зв'язок між сутностями.
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();
    }
}
