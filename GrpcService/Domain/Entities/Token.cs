namespace GrpcService1.Domain.Entities;

public class Token
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Token1 { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? CreatedAt { get; set; }
}