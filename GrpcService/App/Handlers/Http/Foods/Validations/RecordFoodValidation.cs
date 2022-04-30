using System.ComponentModel.DataAnnotations;

namespace GrpcService1.App.Handlers.Http.Foods.Validations;

public class RecordFoodValidation
{
    [Required] public string title { get; set; }
    [Required] public int price { get; set; }
    [Required] public bool is_available { get; set; }
}