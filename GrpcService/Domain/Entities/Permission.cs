namespace GrpcService1.Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public int GrantedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}