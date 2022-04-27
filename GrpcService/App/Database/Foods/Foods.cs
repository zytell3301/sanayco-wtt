using ErrorReporter;
using GrpcService1.App.Core.Foods;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Food = GrpcService1.Domain.Entities.Food;
using FoodOrder = GrpcService1.Domain.Entities.FoodOrder;

namespace GrpcService1.App.Database.Foods;

public class Foods : IDatabase
{
    private wttContext Connection;
    private IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public Foods(wttContext connection, IErrorReporter errorReporter, InternalError internalError)
    {
        Connection = connection;
        ErrorReporter = errorReporter;

        InternalError = internalError;
    }

    public void RecordFood(Food food)
    {
        try
        {
            Connection.Foods.Add(new Model.Food()
            {
                Price = food.Price,
                Title = food.Title,
                IsAvailable = food.IsAvailable,
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
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