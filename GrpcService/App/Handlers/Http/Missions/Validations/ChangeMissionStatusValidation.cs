#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions.Validations;

public class ChangeMissionStatusValidation
{
    [Required] public int mission_id { get; set; }
}