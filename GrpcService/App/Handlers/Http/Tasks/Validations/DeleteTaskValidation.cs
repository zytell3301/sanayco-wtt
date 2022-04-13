using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.tasks.Validations;

public class DeleteTaskValidation
{
    [Required] public int Id;
}