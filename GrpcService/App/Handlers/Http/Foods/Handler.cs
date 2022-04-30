﻿using System.Text.Json;
using GrpcService1.App.Handlers.Http.Foods.Validations;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Foods;

[Route("foods")]
public class Handler : BaseHandler
{
    private Core.Foods.Core Core;

    public Handler(Core.Foods.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    public string RecordFood()
    {
        try
        {
            Authorize("record-food");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordFoodValidation body;
        try
        {
            body = DecodePayloadJson<RecordFoodValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.RecordFood(new Food()
            {
                Price = body.price,
                Title = body.title,
                IsAvailable = body.is_available,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    public string UpdateFoodInfo()
    {
        try
        {
            Authorize("update-food-info");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateFoodInfoValidation body;
        try
        {
            body = DecodePayloadJson<UpdateFoodInfoValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.UpdateFoodInfo(new Food()
            {
                Price = body.price,
                Title = body.title,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-food-info/{id}")]
    public string GetFoodInfo(int id)
    {
        try
        {
            var food = Core.GetFoodInfo(new Food()
            {
                Id = id,
            });
            return JsonSerializer.Serialize(new GetFoodInfoResponse()
            {
                status_code = 0,
                food = new GetFoodInfoResponse.Food()
                {
                    id = food.Id,
                    is_available = food.IsAvailable,
                    price = food.Price,
                    title = food.Title,
                }
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    public string SetFoodAvailable()
    {
        try
        {
            Authorize("update-food-info");
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        ChangeFoodStatusValidation body;
        try
        {
            body = DecodePayloadJson<ChangeFoodStatusValidation>();
        }
        catch (Exception)
        {
            return ResponseToJson(DataValidationFailedResponse());
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.SetFoodAvailable(new Food()
            {
                Id = body.food_id,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    public string SetFoodUnavailable()
    {
        try
        {
            Authorize("update-food-info");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        ChangeFoodStatusValidation body;
        try
        {
            body = DecodePayloadJson<ChangeFoodStatusValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.SetFoodUnavailable(new Food()
            {
                Id = body.food_id,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    public string DeleteFood()
    {
        try
        {
            Authorize("delete-food");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        DeleteFoodValidation body;
        try
        {
            body = DecodePayloadJson<DeleteFoodValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.DeleteFood(new Food()
            {
                Id = body.food_id,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }


    public string OrderFood()
    {
        try
        {
            Authorize("order-food");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        OrderFoodValidation body;
        try
        {
            body = DecodePayloadJson<OrderFoodValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.OrderFood(new FoodOrder()
            {
                FoodId = body.food_id,
                UserId = GetUserId(),
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    public string CancelOrder()
    {
        CancelOrderValidation body;
        try
        {
            body = DecodePayloadJson<CancelOrderValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.CancelOrder(new FoodOrder()
            {
                Id = body.order_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private class GetFoodInfoResponse : Response
    {
        public Food food { get; set; }

        public class Food
        {
            public int id { get; set; }
            public string title { get; set; }
            public int price { get; set; }
            public bool is_available { get; set; }
        }
    }
}