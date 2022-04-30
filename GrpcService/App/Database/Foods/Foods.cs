#region

using ErrorReporter;
using GrpcService1.App.Core.Foods;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Food = GrpcService1.Domain.Entities.Food;
using FoodOrder = GrpcService1.Domain.Entities.FoodOrder;

#endregion

namespace GrpcService1.App.Database.Foods;

public class Foods : IDatabase
{
    private readonly InternalError InternalError;
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;

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
            Connection.Foods.Add(new Model.Food
            {
                Price = food.Price,
                Title = food.Title,
                IsAvailable = food.IsAvailable
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
        try
        {
            var model = Connection.Foods.First(f => f.Id == food.Id);
            model.Price = food.Price;
            model.Title = food.Title;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public Food GetFoodInfo(Food food)
    {
        // try
        // {
        return ConvertFoodModel(Connection.Foods.First(f => f.Id == food.Id));
        // }
        // catch (Exception e)
        // {
        // ErrorReporter.ReportException(e);
        // throw InternalError;
        // }
    }

    public void ChangeFoodStatus(Food food)
    {
        try
        {
            var model = Connection.Foods.First(f => f.Id == food.Id);
            model.IsAvailable = food.IsAvailable;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void DeleteFood(Food food)
    {
        try
        {
            var model = Connection.Foods.First(f => f.Id == food.Id);
            Connection.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void RecordOrder(FoodOrder order)
    {
        try
        {
            var model = new Model.FoodOrder
            {
                Price = order
                    .Price, // Integrity of food price is already checked in core. It is non of database layers business to check it.
                Date = order.Date,
                FoodId = order.FoodId,
                UserId = order.UserId
            };
            Connection.FoodOrders.Add(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void DeleteOrder(FoodOrder order)
    {
        Console.WriteLine(order.Id);
        try
        {
            var model = Connection.FoodOrders.First(o => o.Id == order.Id);
            Connection.FoodOrders.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    private Food ConvertFoodModel(Model.Food model)
    {
        return new Food
        {
            Id = model.Id,
            Price = model.Price,
            Title = model.Title,
            IsAvailable = model.IsAvailable
        };
    }
}