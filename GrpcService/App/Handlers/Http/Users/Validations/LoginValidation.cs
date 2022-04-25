using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class LoginValidation
{
    [Required] [MinLength(8)] [MaxLength(32)]
    public string username;

    [Required] [MinLength(8)] [MaxLength(32)]
    public string password;
}