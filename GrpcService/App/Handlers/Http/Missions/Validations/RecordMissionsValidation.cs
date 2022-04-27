using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class RecordMissionsValidation
{
    [Required] public int project_id;
    [Required] public int from_date;
    [Required] public int to_date;
    [Required] public string description;
    [Required] public string title;
    [Required] public string location;
    [Required] public bool is_verified;
}