#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class RemoveMemberValidation
{
    [Required] public int user_id { get; set; }
    [Required] public int project_id { get; set; }
}