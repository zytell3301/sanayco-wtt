using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.Domain.Entities;

[Table("presentations", Schema = "wtt")]
public class Presentation
{
    [Column("id")] public int Id;
    [Column("user_id")] public int UserId;
    [Column("start")] public DateTime Start;
    [Column("end")] public DateTime End;
}