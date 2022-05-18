namespace GrpcService1.App.Excel;

public interface IExcel
{
    public IExcelManager NewExcel();

    public interface IExcelManager
    {
        public void SetCell(int line, int column, string value);

        public IExcelFile GetExcelFile();
    }

    public interface IExcelFile
    {
        public byte[] GetByte();
    }
}