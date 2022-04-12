#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("tasks", Schema = "wtt")]
public class Task
{
    [Column("created_at")] public DateTime CreatedAt;
    [Column("description")] public string Description;
    [Column("end_time")] [Required] [Timestamp] public DateTime EndTime;
    [Column("id")] public int Id;
    [Column("project_id")] [Required] public int ProjectId;
    [Column("status")] public string Status;

    [Column("title")] [Required, MaxLength(32)]
    public string Title;

    [Column("work_location")] [Required] public string WorkLocation;
}