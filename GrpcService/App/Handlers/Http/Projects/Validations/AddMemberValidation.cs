using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class AddMemberValidation
{
    [Required] public int ProjectId;
    [Required] public int UserId;
    [Required] public string Level;
}