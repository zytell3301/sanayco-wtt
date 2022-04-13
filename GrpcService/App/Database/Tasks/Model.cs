using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.App.Database.Tasks;

[Table("tasks")]
public class Model
{
    [Column("created_at")] public int? CreatedAt { get; set; }
    [Column("description")] public string Description { get; set; }

    [Column("end_time")] public int EndTime { get; set; }

    [Column("id")] public int Id { get; set; }
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("status")] public string Status { get; set; }

    [Column("title")] public string Title { get; set; }

    [Column("work_location")] public string WorkLocation { get; set; }
    [Column("user_id")] public int UserId { get; set; }
}