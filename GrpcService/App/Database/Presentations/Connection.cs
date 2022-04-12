using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database.Presentations;

public class Connection : Database.Connection
{
    public DbSet<Domain.Entities.Presentation> Presentations;

    public Connection(string connectionString) : base(connectionString)
    {
    }
}