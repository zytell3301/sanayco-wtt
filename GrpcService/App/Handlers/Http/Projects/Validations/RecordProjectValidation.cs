using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class RecordProjectValidation
{
    [MaxLength(1024)] public string description { get; set; }
    [MaxLength(32)] public string name { get; set; }
    [Required] public int creator_id { get; set; }
}