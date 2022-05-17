using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Presentation.Validations;

public class UpdatePresentationValidation
{
    [Required] public int presentation_id { get; set; }
    [Required] public int start { get; set; }
    [Required] public int end { get; set; }
}