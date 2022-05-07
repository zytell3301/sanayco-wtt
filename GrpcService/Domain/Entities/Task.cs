#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace GrpcService1.Domain.Entities;

public class Task
{
    public DateTime? CreatedAt { get; set; }
    public string Description { get; set; }

    public DateTime EndTime { get; set; }

    public int Id { get; set; }
    [Required] public int ProjectId { get; set; }
    public string Status { get; set; }

    public string Title { get; set; }

    public string WorkLocation { get; set; }
    public int UserId { get; set; }

    public int Points { get; set; }

    public DateTime StartTime { get; set; }
}