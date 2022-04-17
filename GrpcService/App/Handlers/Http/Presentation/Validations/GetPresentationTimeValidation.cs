using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Presentation.Validations;

public class GetPresentationTimeValidation
{
    [Required] public int user_id { get; set; }
}