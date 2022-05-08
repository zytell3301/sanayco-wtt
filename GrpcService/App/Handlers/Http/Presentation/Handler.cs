#region

using System.Text.Json;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace GrpcService1.App.Handlers.Http.Presentation;

[Route("/presentation")]
public class Handler : BaseHandler
{
    private readonly Core.Presentation.Core Core;

    public Handler(Core.Presentation.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [HttpPost("record")]
    public string RecordPresentation()
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        try
        {
            Core.RecordPresentation(new User
            {
                Id = GetUserId()
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("record-end")]
    public string RecordPresentationEnd()
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        try
        {
            Core.RecordPresentationEnd(new User
            {
                Id = GetUserId()
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("get-presentation-time")]
    public string GetPresentationTime()
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        try
        {
            Core.GetPresentationTime(new User
            {
                Id = GetUserId()
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-range/{fromDate}/{toDate}")]
    public string GetPresentationsListRange(int fromDate, int toDate)
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        try
        {
            return JsonSerializer.Serialize(new GetPresentationsListRangeResponse()
            {
                presentations = Core.GetPresentationsRange(DateTime.UnixEpoch.AddSeconds(fromDate),
                    DateTime.UnixEpoch.AddSeconds(toDate),
                    GetUserId())
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    private class GetPresentationsListRangeResponse : Response
    {
        public List<Domain.Entities.Presentation> presentations { get; set; }
    }
}