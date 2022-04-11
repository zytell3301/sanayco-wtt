#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("project_members", Schema = "wtt")]
public class ProjectMember
{
    [Column("created_at")] public DateTime CreatedAt;
    [Column("id")] public int Id;
    [Column("member_level")] public string Level;
    [Column("project_id")] public int ProjectId;
    [Column("user_id")] public int UserId;
}