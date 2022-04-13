using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class UpdateProjectValidation
{
    [MaxLength(1024)] public string Name;
    [MaxLength(32)] public string Description;
}