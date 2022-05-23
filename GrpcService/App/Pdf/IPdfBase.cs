namespace GrpcService1.App.Pdf;

public interface IPdfBase
{
    public interface IPdfFile
    {
        public byte[] GetPdfBytes();
    }
}