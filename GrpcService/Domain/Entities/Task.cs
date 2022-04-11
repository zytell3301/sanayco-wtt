#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("tasks", Schema = "wtt")]
public class Task
{
    [Column("created_at")] public DateTime CreatedAt;
    [Column("description")] public string Description;
    [Column("end_time")] public DateTime EndTime;
    [Column("id")] public int Id;
    [Column("project_id")] public string ProjectId;
    [Column("status")] public string Status;
    [Column("title")] public string Title;
    [Column("work_location")] public string WorkLocation;
}