#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class CancelOffTimeValidation
{
    [Required] public int off_time_id { get; set; }
}