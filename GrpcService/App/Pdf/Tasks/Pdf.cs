using GrpcService1.App.Core.Tasks;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;

namespace GrpcService1.App.Pdf.Tasks;

public class Pdf : IPdf
{
    public class PdfFile : IPdfBase.IPdfFile
    {
        public MemoryStream Stream;

        public PdfFile()
        {
            Stream = new MemoryStream();
        }

        public byte[] GetPdfBytes()
        {
            return Stream.GetBuffer();
        }
    }

    public IPdfBase.IPdfFile NewPdfFile(List<Domain.Entities.Task> tasks, IPdf.ReportInfo reportInfo)
    {
        var File = new PdfFile();
        var report = new StiReport();
        report.Load("./ReportTemplates/tasks_report.mrt");

        report.RegData("dt", tasks);
        report.RegData("ReportInfo", reportInfo);
        report.Dictionary.Synchronize();
        Stimulsoft.Base.StiFontCollection.AddFontFile("./ReportTemplates/Bahij Nazanin-Regular.ttf","Bahij Nazanin");
        Stimulsoft.Base.StiFontCollection.AddFontFile("./ReportTemplates/B-NAZANIN.TTF","B Nazanin");
        report.Render();
        report.ExportDocument(StiExportFormat.Pdf, File.Stream);
        return File;
    }
}