#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class AddMemberValidation
{
    [Required] public int project_id { get; set; }
    [Required] public int user_id { get; set; }
    [Required] public string level { get; set; }
}