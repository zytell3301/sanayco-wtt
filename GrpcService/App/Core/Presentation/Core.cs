#region

using GrpcService1.App.Excel;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Presentation;

public class Core
{
    private readonly IDatabase database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;
    private IExcel Excel;
    private IPdf Pdf;

    public Core(PresentationCoreDependencies dependencies, PresentationCoreConfigs configs)
    {
        database = dependencies.Database;
        /*
         * Initiate the error instance once and use it forever.
         */
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        Excel = dependencies.Excel;
        Pdf = dependencies.Pdf;
    }

    public bool CheckEntityOwnership(int presentationId, int applicantId)
    {
        var presentation = database.GetPresentation(new Domain.Entities.Presentation
        {
            Id = presentationId
        });
        return presentation.UserId == applicantId;
    }

    /*
     * This method records given user's presentation time 
     */
    public void RecordPresentation(User user)
    {
        try
        {
            database.RecordPresentation(user);
        }
        catch (InternalError)
        {
            throw InternalError;
        }
    }

    /*
     * This method records the end of a user's presentation time
     */
    public void RecordPresentationEnd(User user)
    {
        try
        {
            database.RecordPresentationEnd(user);
        }
        catch (InternalError)
        {
            throw InternalError;
        }
    }

    /*
     * This method gets the latest representation time of the supplied user.
     * It is used for calculating users representation time or maybe other
     * feature updates.
     */
    public DateTime GetPresentationTime(User user)
    {
        DateTime presentationTime;
        try
        {
            presentationTime = database.GetPresentationTime(user);
        }
        catch (Exception)
        {
            throw InternalError;
        }

        return presentationTime;
    }

    public List<Domain.Entities.Presentation> GetPresentationsRange(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            return database.GetPresentationsRange(fromDate, toDate, userId);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdatePresentation(Domain.Entities.Presentation presentation)
    {
        try
        {
            database.UpdatePresentation(presentation);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Domain.Entities.Presentation GetPresentation(Domain.Entities.Presentation presentation)
    {
        try
        {
            return database.GetPresentation(presentation);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public IExcel.IExcelFile GenerateExcel(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            var presentations = database.GetPresentationsRange(fromDate, toDate, userId);
            var excel = Excel.NewExcel();
            excel.SetCell(1, 1, "id");
            excel.SetCell(1, 2, "user id");
            excel.SetCell(1, 3, "start");
            excel.SetCell(1, 4, "end");
            var i = 2;
            foreach (var presentation in presentations)
            {
                excel.SetCell(i, 1, presentation.Id.ToString());
                excel.SetCell(i, 2, presentation.UserId.ToString());
                excel.SetCell(i, 3, presentation.Start.ToString());
                excel.SetCell(i, 4, presentation.End.ToString());
                i++;
            }

            return excel.GetExcelFile();
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public IPdf.IPdfFile GeneratePdf(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            var presentations = database.GetPresentationsRange(fromDate, toDate, userId);
            var user = database.GetUser(userId);
            return Pdf.NewPdfFile(presentations, new IPdf.ReportInfo()
            {
                FromDate = fromDate.ToString(),
                GeneratedAt = DateTime.Now.ToString(),
                ToDate = toDate.ToString(),
                Lastname = user.LastName,
                Name = user.Name,
            });
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class PresentationCoreConfigs
    {
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
    }

    public class PresentationCoreDependencies
    {
        public IDatabase Database;
        public IExcel Excel;
        public IPdf Pdf;
    }
}