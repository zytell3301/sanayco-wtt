using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class RemoveMemberValidation
{
    [Required] public int UserId;
    [Required] public int ProjectId;
}