using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Database.Presentations;

public class Connection : GrpcService1.Database.Connection
{
    public DbSet<Domain.Entities.Presentation> Presentations;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}