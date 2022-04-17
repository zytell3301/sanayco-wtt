using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.App.Database.Presentations;

public class Model
{
    [Column("id")] public int Id;
    [Column("user_id")] public int UserId;
    [Column("start")] public DateTime Start;
    [Column("end")] public DateTime End;
}