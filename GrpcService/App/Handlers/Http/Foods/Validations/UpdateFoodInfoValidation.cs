namespace GrpcService1.App.Handlers.Http.Foods.Validations;

public class UpdateFoodInfoValidation
{
    public int food_id { get; set; }
    public string title { get; set; }
    public int price { get; set; }
}