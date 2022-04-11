#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("users", Schema = "wtt")]
public class User
{
    [Column("company_level")] public string CompanyLevel;
    [Column("created_at")] public DateTime? CreatedAt;
    [Column("id")] public int Id;
    [Column("lastname")] public string LastName;
    [Column("name")] public string Name;
    [Column("skill_level")] public string SkillLevel;
}