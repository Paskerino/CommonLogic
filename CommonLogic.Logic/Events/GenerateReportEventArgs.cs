using System;

public class GenerateReportEventArgs : EventArgs
{
    public string ReportName { get; }

    public GenerateReportEventArgs(string reportName)
    {
        ReportName = reportName;
    }
}