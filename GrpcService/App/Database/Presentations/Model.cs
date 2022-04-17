using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.App.Database.Presentations;

[Table("presentations")]
public class Model
{
    [Column("id")] public int Id { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("start")] public DateTime? Start { get; set; }
    [Column("end")] public DateTime? End { get; set; }
}