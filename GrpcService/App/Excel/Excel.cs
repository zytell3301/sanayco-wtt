#region

using OfficeOpenXml;

#endregion

namespace GrpcService1.App.Excel;

public class Excel : IExcel
{
    public IExcel.IExcelManager NewExcel()
    {
        return new ExcelManager();
    }

    public class ExcelManager : IExcel.IExcelFile, IExcel.IExcelManager
    {
        private readonly ExcelPackage ExcelPackage;
        private readonly ExcelWorksheet ExcelWorksheet;
        private readonly MemoryStream Stream;

        public ExcelManager()
        {
            ExcelPackage = new ExcelPackage();
            ExcelWorksheet = ExcelPackage.Workbook.Worksheets.Add("Sheet 1");
            Stream = new MemoryStream();
        }

        public byte[] GetByte()
        {
            return Stream.GetBuffer();
        }

        public void SetCell(int line, int column, string value)
        {
            ExcelWorksheet.Cells[line, column].Value = value;
        }

        public IExcel.IExcelFile GetExcelFile()
        {
            ExcelPackage.SaveAs(Stream);
            return this;
        }
    }
}