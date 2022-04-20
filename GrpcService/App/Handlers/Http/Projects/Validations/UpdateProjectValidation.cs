#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class UpdateProjectValidation
{
    [MaxLength(1024)] public string name { get; set; }
    [MaxLength(32)] public string description { get; set; }
    [Required] public int project_id { get; set; }
}