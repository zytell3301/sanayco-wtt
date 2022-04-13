#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#endregion

namespace GrpcService1.Domain.Entities;

public class Task
{
    public DateTime? CreatedAt { get; set; }
    public string Description { get; set; }

    [Required] [Timestamp] public DateTime EndTime { get; set; }

    public int Id { get; set; }
    [Required] public int ProjectId { get; set; }
    public string Status { get; set; }

    [Required, MaxLength(32)] public string Title { get; set; }

    [Required] public string WorkLocation { get; set; }
    public int UserId { get; set; }
}