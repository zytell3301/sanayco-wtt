using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Database.Tasks;

public class Connection : GrpcService1.Database.Connection
{
    public DbSet<Domain.Entities.Task> Tasks;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}