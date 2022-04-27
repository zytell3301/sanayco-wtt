#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class RecordMissionsValidation
{
    [Required] public string description { get; set; }
    [Required] public int from_date { get; set; }
    [Required] public bool is_verified { get; set; }
    [Required] public string location { get; set; }
    [Required] public int project_id { get; set; }
    [Required] public string title { get; set; }
    [Required] public int to_date { get; set; }
}