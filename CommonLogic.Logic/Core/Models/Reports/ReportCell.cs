using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models.Reports
{
    public class ReportCell
    {
        // Значення, яке буде відображено в комірці
        public string Value { get; set; } = string.Empty;

        // Опціонально: чи потрібно виділяти цю комірку (наприклад, жирним)
        public bool IsHighlighted { get; set; } = false;
    }
}
