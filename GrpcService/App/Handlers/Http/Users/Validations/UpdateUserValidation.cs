using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Users.Validations;

public class UpdateUserValidation
{
    [Required] public int user_id { get; set; }
    public string name { get; set; }
    public string lastname { get; set; }
    public string username { get; set; }
    public string skill_level { get; set; }
    public string company_level { get; set; }
    public string[] permissions { get; set; }
}