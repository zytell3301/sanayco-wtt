#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("tasks")]
public class Task
{
    [Column("created_at")] public DateTime? CreatedAt { get; set; }
    [Column("description")] public string Description { get; set; }

    [Column("end_time")]
    [Required]
    [Timestamp]
    public DateTime EndTime { get; set; }

    [Column("id")] public int Id { get; set; }
    [Column("project_id")] [Required] public int ProjectId { get; set; }
    [Column("status")] public string Status { get; set; }

    [Column("title")]
    [Required, MaxLength(32)]
    public string Title { get; set; }

    [Column("work_location")] [Required] public string WorkLocation { get; set; }
    [Column("user_id")] public int UserId { get; set; }
}