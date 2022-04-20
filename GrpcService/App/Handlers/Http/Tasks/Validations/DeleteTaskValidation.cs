#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.tasks.Validations;

public class DeleteTaskValidation
{
    [Required] public int task_id { get; set; }
}