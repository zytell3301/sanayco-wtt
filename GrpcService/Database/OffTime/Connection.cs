using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Database.OffTime;

public class Connection : GrpcService1.Database.Connection
{
    public DbSet<Domain.Entities.OffTime> OffTimes;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}