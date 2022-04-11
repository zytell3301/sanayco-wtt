using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.Domain.Entities;

[Table("tasks", Schema = "wtt")]
public class Task
{
    [Column("id")] public int Id;
    [Column("title")] public string Title;
    [Column("project_id")] public string ProjectId;
    [Column("work_location")] public string WorkLocation;
    [Column("end_time")] public DateTime EndTime;
    [Column("description")] public string Description;
    [Column("status")] public string Status;
    [Column("created_at")] public DateTime CreatedAt;
}