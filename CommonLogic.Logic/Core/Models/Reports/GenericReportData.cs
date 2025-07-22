using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models.Reports
{
    public class GenericReportData
    {
        public string Title { get; set; } = "Звіт";

        // Заголовки стовпців (це теж рядок)
        public ReportRow Headers { get; set; } = new ReportRow();

        // Рядки з даними
        public List<ReportRow> DataRows { get; set; } = new List<ReportRow>();
    }
}
