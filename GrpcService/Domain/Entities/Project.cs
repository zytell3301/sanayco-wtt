#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("projects", Schema = "wtt")]
public class Project
{
    [Column("description")] public string Description;
    [Column("id")] public int Id;
    public ICollection<ProjectMember> Members;
    [Column("name")] public string Name;
}