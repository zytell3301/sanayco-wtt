#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class LoginValidation
{
    [Required]
    [MinLength(8)]
    [MaxLength(32)]
    public string username { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(32)]
    public string password { get; set; }
}