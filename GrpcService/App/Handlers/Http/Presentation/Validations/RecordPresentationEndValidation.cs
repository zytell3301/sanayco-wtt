using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Presentation.Validations;

public class RecordPresentationEndValidation
{
    [Required] public int UserId;
}