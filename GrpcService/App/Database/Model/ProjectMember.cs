﻿namespace GrpcService1.App.Database.Model;

public class ProjectMember
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? ProjectId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Level { get; set; }
}