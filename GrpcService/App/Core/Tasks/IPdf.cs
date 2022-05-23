using GrpcService1.App.Pdf;

namespace GrpcService1.App.Core.Tasks;

public interface IPdf : IPdfBase
{
    public IPdfBase.IPdfFile NewPdfFile(List<Domain.Entities.Task> tasks,ReportInfo reportInfo);
    
    public class ReportInfo
    {
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GeneratedAt { get; set; }
    }
}