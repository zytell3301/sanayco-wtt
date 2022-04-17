using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database.Presentations;

public class Connection : Database.Connection
{
    public DbSet<Model> Presentations { get; set; }

    public Connection(string connectionString) : base(connectionString)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Presentation>(entity => { entity.HasKey(e => e.Id); });

        base.OnModelCreating(modelBuilder);
    }
}