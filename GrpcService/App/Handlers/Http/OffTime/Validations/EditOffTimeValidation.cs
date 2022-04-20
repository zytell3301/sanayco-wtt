#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class EditOffTimeValidation
{
    [Required] public int off_time_id { get; set; }
    [MaxLength(1024)] public string description { get; set; }
    public int from_date { get; set; }
    public int to_date { get; set; }
}