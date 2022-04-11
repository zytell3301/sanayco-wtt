namespace GrpcService1.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("users", Schema = "wtt")]
public class User
{
    [Column(name: "id")] public int Id;
    [Column(name: "name")] public string Name;
    [Column("lastname")] public string LastName;
    [Column("skill_level")] public string SkillLevel;
    [Column("company_level")] public string CompanyLevel;
    [Column("created_at")] public DateTime? CreatedAt;
}