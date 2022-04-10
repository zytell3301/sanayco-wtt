namespace GrpcService1.Domain.Entities;

public class Project
{
    public int Id;
    public string Name;
    public string Description;
    public User[] Members;
}