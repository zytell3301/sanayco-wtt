using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class DeleteUserValidation
{
    [Required] public string username { get; set; }
}