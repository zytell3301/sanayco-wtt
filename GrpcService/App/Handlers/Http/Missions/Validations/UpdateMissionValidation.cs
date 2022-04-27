using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class UpdateMissionValidation
{
    [Required] public int mission_id;
    public string description;
    public int project_id;
    public int from_date;
    public int to_date;
    public string title;
    public string location;
}