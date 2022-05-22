using GrpcService1.App.Core.Presentation;
using GrpcService1.Domain.Entities;
using Stimulsoft.Report;

namespace GrpcService1.App.Pdf.Presentations;

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

    public IPdfBase.IPdfFile NewPdfFile(List<Presentation> presentations, IPdf.ReportInfo reportInfo)
    {
        var File = new PdfFile();
        var report = new StiReport();
        report.Load("./ReportTemplates/presentation_report.mrt");

        report.RegData("dt", presentations);
        report.RegData("ReportInfo", reportInfo);
        report.Dictionary.Synchronize();
        report.Render();
        report.ExportDocument(StiExportFormat.Pdf, File.Stream);
        return File;
    }
}