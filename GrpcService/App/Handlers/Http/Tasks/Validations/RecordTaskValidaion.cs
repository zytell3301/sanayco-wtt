#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.App.Handlers.Http.tasks.Validations;

public class RecordTaskValidaion
{
    public string description { get; set; }
    [MaxLength(32)] public string title { get; set; }
    public int project_id { get; set; }
    public string work_location { get; set; }
    public decimal start_time { get; set; }
    public int points { get; set; }
}