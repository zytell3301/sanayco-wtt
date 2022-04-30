namespace GrpcService1.App.Database.Model;

public class FoodOrder
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public int UserId { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }

    public virtual Food Food { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}