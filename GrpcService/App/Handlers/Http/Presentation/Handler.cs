using GrpcService1.App.Handlers.Http.Presentation.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Presentation;

public class Handler : ControllerBase
{
    private Core.Presentation.Core Core;

    public Handler(Core.Presentation.Core core)
    {
        Core = core;
    }

    public string RecordPresentation([FromForm] int user_id)
    {
        RecordPresentationValidation validation = new RecordPresentationValidation()
        {
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.RecordPresentation(new User()
            {
                Id = validation.UserId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned  to user for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    public string RecordPresentationEnd([FromForm] int user_id)
    {
        RecordPresentationEndValidation validation = new RecordPresentationEndValidation()
        {
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned for invalid data
                return "data validation failed";
        }

        try
        {
            Core.RecordPresentationEnd(new User()
            {
                Id = validation.UserId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    public string GetPresentationTime([FromForm] int user_id)
    {
        GetPresentationTimeValidation validation = new GetPresentationTimeValidation()
        {
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to user because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.GetPresentationTime(new User()
            {
                Id = validation.UserId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }
}