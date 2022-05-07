namespace GrpcService1.App.Database.Model;

public class Mission
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int ProjectId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Description { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Location { get; set; } = null!;
    public bool? IsVerified { get; set; }

    public virtual User Member { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}