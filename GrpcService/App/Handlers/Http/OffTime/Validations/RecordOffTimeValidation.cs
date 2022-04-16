using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class RecordOffTimeValidation
{
    [Required] public int FromDate;
    [Required] public int ToDate;
    [Required] public int UserId;
    public string Description;
}