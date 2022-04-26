#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class RecordUserValidation
{
    [Required] public string name { get; set; }
    [Required] public string lastname { get; set; }
    [Required] public string skill_level { get; set; }
    [Required] public string company_level { get; set; }
    [Required] public string password { get; set; }
    [Required] public string username { get; set; }
    [Required] public string[] permissions { get; set; }
}