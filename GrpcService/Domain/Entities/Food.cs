namespace GrpcService1.Domain.Entities;

public class Food
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
}