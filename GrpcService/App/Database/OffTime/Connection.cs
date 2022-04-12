using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database.OffTime;

public class Connection : Database.Connection
{
    public DbSet<Domain.Entities.OffTime> OffTimes;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}