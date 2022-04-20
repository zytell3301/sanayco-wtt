#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.Projects.Validations;

public class DeleteProjectValidation
{
    [Required] public int project_id { get; set; }
}