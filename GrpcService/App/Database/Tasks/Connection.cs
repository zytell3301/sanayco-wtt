﻿using Microsoft.EntityFrameworkCore;

namespace GrpcService1.App.Database.Tasks;

public class Connection : Database.Connection
{
    public DbSet<Model> Tasks { get; set; }

    public Connection(string connectionString) : base(connectionString)
    {
    }
}