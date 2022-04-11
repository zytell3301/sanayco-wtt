using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Database.Projects;

public class Connection : GrpcService1.Database.Connection
{
    public DbSet<Project> Projects;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}