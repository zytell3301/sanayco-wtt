using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class CancelOffTimeValidation
{
    [Required] public int off_time_id { get; set; }
}