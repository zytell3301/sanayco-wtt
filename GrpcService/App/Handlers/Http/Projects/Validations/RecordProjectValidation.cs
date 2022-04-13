using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class RecordProjectValidation
{
    [MaxLength(1024)] public string Description;
    [MaxLength(32)] public string Name;
    [Required] public int UserId;
}