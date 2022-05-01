#region

#endregion

namespace GrpcService1.Domain.Entities;

public class Project
{
    public string Description { get; set; }
    public int Id { get; set; }
    public ICollection<ProjectMember> Members { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}