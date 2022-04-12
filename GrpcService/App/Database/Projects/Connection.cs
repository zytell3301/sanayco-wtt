using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database.Projects;

public class Connection : Database.Connection
{
    public DbSet<Project> Projects;
    public DbSet<ProjectMember> ProjectMembers;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}