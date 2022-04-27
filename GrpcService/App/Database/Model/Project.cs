namespace GrpcService1.App.Database.Model;

public class Project
{
    public Project()
    {
        Missions = new HashSet<Mission>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Mission> Missions { get; set; }
}