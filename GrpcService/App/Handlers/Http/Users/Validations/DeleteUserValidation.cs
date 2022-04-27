#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class DeleteUserValidation
{
    [Required] public string username { get; set; }
}