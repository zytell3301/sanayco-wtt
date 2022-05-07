#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Foods;

public class Core
{
    private readonly FoodNotAvailable FoodNotAvailableError;
    private readonly InternalError InternalError;

    private readonly IDatabase Database;

    public Core(FoodsCoreDependencies dependencies, FoodsCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        FoodNotAvailableError = new FoodNotAvailable("FoodNotAvailableError");

        Database = dependencies.Database;
    }

    public bool CheckOrderOwnership(int orderId, int userId)
    {
        var order = Database.GetOrder(new FoodOrder()
        {
            Id = orderId,
        });
        return order.UserId == userId;
    }

    public void RecordFood(Food food)
    {
        try
        {
            Database.RecordFood(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdateFoodInfo(Food food)
    {
        try
        {
            Database.UpdateFoodInfo(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Food GetFoodInfo(Food food)
    {
        try
        {
            return Database.GetFoodInfo(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetFoodAvailable(Food food)
    {
        try
        {
            food.IsAvailable = true;
            Database.ChangeFoodStatus(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetFoodUnavailable(Food food)
    {
        try
        {
            food.IsAvailable = false;
            Database.ChangeFoodStatus(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteFood(Food food)
    {
        try
        {
            Database.DeleteFood(food);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void OrderFood(FoodOrder order)
    {
        try
        {
            var food = Database.GetFoodInfo(new Food
            {
                Id = order.FoodId
            });
            switch (food.IsAvailable)
            {
                case false:
                    throw FoodNotAvailableError;
            }

            order.Price = food.Price;
            order.Date = DateTime.Now;
            Database.RecordOrder(order);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void CancelOrder(FoodOrder order)
    {
        try
        {
            Database.DeleteOrder(order);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Domain.Entities.Food> GetAvailableFoodsList()
    {
        try
        {
            return Database.GetAvailableFoodsList();
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class FoodsCoreDependencies
    {
        public IDatabase Database;
    }

    public class FoodsCoreConfigs
    {
        public string InternalErrorMessage;
    }

    private class FoodNotAvailable : Domain.Errors.Status
    {
        public FoodNotAvailable(string message) : base(message)
        {
        }
    }
}