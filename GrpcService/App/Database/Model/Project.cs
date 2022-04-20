﻿namespace GrpcService1.App.Database.Model;

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Description { get; set; }
}