#region

using GrpcService1.App.Pdf;

#endregion

namespace GrpcService1.App.Core.Presentation;

public interface IPdf : IPdfBase
{
    public IPdfFile NewPdfFile(List<Domain.Entities.Presentation> presentations, ReportInfo reportInfo);

    public class ReportInfo
    {
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GeneratedAt { get; set; }
    }
}