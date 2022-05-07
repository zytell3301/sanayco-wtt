namespace GrpcService1.App.Database.Model;

public class Presentation
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }

    public virtual User? User { get; set; }
}