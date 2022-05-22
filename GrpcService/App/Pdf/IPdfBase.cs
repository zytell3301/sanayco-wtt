using GrpcService1.App.Core.Presentation;
using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Pdf;

public interface IPdfBase
{
    public IPdfFile NewPdfFile(List<Presentation> presentations, IPdf.ReportInfo reportInfo);
    public interface IPdfFile
    {
        public byte[] GetPdfBytes();
    }
}