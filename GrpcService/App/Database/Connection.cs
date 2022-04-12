using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database;

public class Connection : DbContext
{
    private string ConnectionString;

    public Connection(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}