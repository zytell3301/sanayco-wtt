namespace GrpcService1.App.Database.Model;

public class Task
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? UserId { get; set; }
    public int? ProjectId { get; set; }
    public string? WorkLocation { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime StartTime { get; set; }
    public int Points { get; set; }

    public virtual Project? Project { get; set; }
    public virtual User? User { get; set; }
}