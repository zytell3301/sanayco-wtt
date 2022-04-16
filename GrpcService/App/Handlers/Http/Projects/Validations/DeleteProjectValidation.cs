using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class DeleteMemberValidation
{
    [Required] public int ProjectId;
}