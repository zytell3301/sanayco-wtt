namespace GrpcService1.Domain.Entities;

public class ProjectMember
{
    public int Id;
    public int UserId;
    public int ProjectId;
    public string MemberLevel;
    public DateTime CreatedAt;
}