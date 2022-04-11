﻿using GrpcService1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Database;

public class Connection : DbContext
{
    private string ConnectionString;

    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<Domain.Entities.Project> Projects { get; set; }

    public Connection(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}