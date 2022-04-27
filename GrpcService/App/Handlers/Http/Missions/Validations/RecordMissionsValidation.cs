#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class RecordMissionsValidation
{
    [Required] public string description;
    [Required] public int from_date;
    [Required] public bool is_verified;
    [Required] public string location;
    [Required] public int project_id;
    [Required] public string title;
    [Required] public int to_date;
}