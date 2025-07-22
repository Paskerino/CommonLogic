using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{
    public enum TriggerCondition
    {
        GreaterThan,
        LessThan,
        EqualTo
    }

    public class TriggerRule
    {
        public int SourceSensorId { get; set; }

        // Умова, яка має спрацювати
        public TriggerCondition Condition { get; set; }

        // Поріг для порівняння
        public double ThresholdValue { get; set; }

        // Список дій, які потрібно виконати, якщо умова істинна
        public List<WriteAction> Actions { get; set; } = new List<WriteAction>();
    }
}
