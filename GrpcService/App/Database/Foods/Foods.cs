using ErrorReporter;
using GrpcService1.App.Core.Foods;
using GrpcService1.App.Database.Model;
using Food = GrpcService1.Domain.Entities.Food;
using FoodOrder = GrpcService1.Domain.Entities.FoodOrder;

namespace GrpcService1.App.Database.Foods;

public class Foods : IDatabase
{
    private wttContext Connection;
    private IErrorReporter ErrorReporter;

    public Foods(wttContext connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordFood(Food food)
    {
        throw new NotImplementedException();
    }

    public void UpdateFoodInfo(Food food)
    {
        throw new NotImplementedException();
    }

    public Food GetFoodInfo(Food food)
    {
        throw new NotImplementedException();
    }

    public void ChangeFoodStatus(Food food)
    {
        throw new NotImplementedException();
    }

    public void DeleteFood(Food food)
    {
        throw new NotImplementedException();
    }

    public void RecordOrder(FoodOrder order)
    {
        throw new NotImplementedException();
    }

    public void DeleteOrder(FoodOrder order)
    {
        throw new NotImplementedException();
    }
}