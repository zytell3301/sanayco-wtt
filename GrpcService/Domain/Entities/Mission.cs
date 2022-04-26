namespace GrpcService1.Domain.Entities;

public class Mission
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int ProjectId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Description { get; set; }
}