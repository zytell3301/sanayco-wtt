using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.Domain.Entities;

[Table("project_members", Schema = "wtt")]
public class ProjectMember
{
    [Column("id")] public int Id;
    [Column("user_id")] public int UserId;
    [Column("project_id")] public int ProjectId;
    [Column("member_level")] public string Level;
    [Column("created_at")] public DateTime CreatedAt;
}