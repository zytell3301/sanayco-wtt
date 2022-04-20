#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.tasks.Validations;

public class EditTaskValidation
{
    public int task_id { get; set; }
    [MaxLength(1024)] public string description { get; set; }
    [Timestamp] public int end_time { get; set; }
    [MaxLength(32)] public string title { get; set; }
    public string work_location { get; set; }
}