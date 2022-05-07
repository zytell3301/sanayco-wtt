#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("users", Schema = "wtt")]
public class User
{
    public string CompanyLevel;
    public DateTime? CreatedAt;
    public int Id;
    public string LastName;
    public string Name;
    public string Password;
    public string ProfilePicture;
    public string SkillLevel;
    public string Username;
}