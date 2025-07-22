using CommonLogic.Core.Models.Reports;
using CommonLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CommonLogic.Services.

namespace CommonLogic.Services.Implementations
{
    public class ReportService : IReportService
    {

        public Task CreateReportFileAsync(GenericReportData reportData, string templateReport, string pathReport)
        {
            LibreOfficeService libreOfficeService = new LibreOfficeService();
            libreOfficeService.Open(templateReport);
            libreOfficeService.Save(pathReport);


            return Task.CompletedTask;
        }
    }
}
