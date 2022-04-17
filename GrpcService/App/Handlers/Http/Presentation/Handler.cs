﻿using System.Text.Json;
using System.Text.Json.Nodes;
using GrpcService1.App.Handlers.Http.Presentation.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Presentation;

[Route("/presentation")]
public class Handler : BaseHandler
{
    private Core.Presentation.Core Core;

    public Handler(Core.Presentation.Core core)
    {
        Core = core;
    }

    [HttpPost("record")]
    public string RecordPresentation()
    {
        var req = DecodePayloadJson<RecordPresentationValidation>();

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
                Id = req.user_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned  to user for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("record-end")]
    public string RecordPresentationEnd([FromForm] int user_id)
    {
        var req = DecodePayloadJson<RecordPresentationEndValidation>();

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
                Id = req.user_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("get-presentation-time")]
    public string GetPresentationTime([FromForm] int user_id)
    {
        var body = DecodePayloadJson<GetPresentationTimeValidation>();

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
                Id = body.user_id,
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