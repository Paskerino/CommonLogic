using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models.Reports
{
    public class ReportRow
    {
        public List<ReportCell> Cells { get; set; } = new List<ReportCell>();
    }
}
