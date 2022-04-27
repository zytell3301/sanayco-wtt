#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class UpdateMissionValidation
{
    public string description;
    public int from_date;
    public string location;
    [Required] public int mission_id;
    public int project_id;
    public string title;
    public int to_date;
}