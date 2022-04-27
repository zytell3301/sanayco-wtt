namespace GrpcService1.Domain.Entities;

public class FoodOrder
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public int UserId { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }
}