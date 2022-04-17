using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.Domain.Entities;

public class Presentation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}