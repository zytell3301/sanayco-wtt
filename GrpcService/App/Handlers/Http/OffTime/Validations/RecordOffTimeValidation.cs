#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.OffTime.Validations;

public class RecordOffTimeValidation
{
    [Required] public int from_date { get; set; }
    [Required] public int to_date { get; set; }
    [Required] public int user_id { get; set; }
    public string description { get; set; }
}