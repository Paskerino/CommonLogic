using CommonLogic.Core.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Services.Interfaces
{
    public interface IReportService
    {
        Task CreateReportFileAsync(GenericReportData reportData, string templateReport,string pathReport);
    }
}
