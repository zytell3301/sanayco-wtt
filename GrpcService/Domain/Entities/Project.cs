using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.Domain.Entities;

[Table("projects", Schema = "wtt")]
public class Project
{
    [Column("id")] public int Id;
    [Column("name")] public string Name;
    [Column("description")] public string Description;
    public ICollection<ProjectMember> Members;
}