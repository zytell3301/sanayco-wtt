using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.tasks.Validations;

public class EditTaskValidation
{
    public int Id;
    [MaxLength(1024)] public string Description;
    [Timestamp] public int EndTime;
    [MaxLength(32)] public string Title;
    public string WorkLocation;
    public int UserId;
}