#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class UpdateMissionValidation
{
    public string description { get; set; }
    public int from_date { get; set; }
    public string location { get; set; }
    [Required] public int mission_id { get; set; }
    public int project_id { get; set; }
    public string title { get; set; }
    public int to_date { get; set; }
}