using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class ApproveOffTimeValidation
{
    [Required] public int off_time_id { get; set; }
}