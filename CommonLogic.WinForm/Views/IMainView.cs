using CommonLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Views
{
    public interface IMainView
    {
        event EventHandler StartPollingClicked;
        void DisplaySensorValue(int Id,string value);
        void ShowStatus(string message);
        event EventHandler<GenerateReportEventArgs> GenerateReport;
    }
}
