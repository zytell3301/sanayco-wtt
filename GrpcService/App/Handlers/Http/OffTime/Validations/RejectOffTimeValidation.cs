using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class RejectOffTimeValidation
{
    [Required] public int OffTimeId;
}