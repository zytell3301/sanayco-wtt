﻿using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.Foods;

public interface IDatabase
{
    public void RecordFood(Food food);
    public void UpdateFoodInfo(Food food);
    public Food GetFoodInfo(Food food);
    public void ChangeFoodStatus(Food food);
    public void DeleteFood(Food food);
    public void RecordOrder(FoodOrder order);
    public void DeleteOrder(FoodOrder order);
}