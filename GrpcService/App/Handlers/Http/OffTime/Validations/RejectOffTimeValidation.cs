#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class RejectOffTimeValidation
{
    [Required] public int OffTimeId;
}