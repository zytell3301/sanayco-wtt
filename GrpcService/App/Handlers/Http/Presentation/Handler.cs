#region

using GrpcService1.App.Excel;
using System.Text.Json;
using GrpcService1.App.Handlers.Http.Presentation.Validations;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

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
            return JsonSerializer.Serialize(new GetPresentationsListRangeResponse
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

    [HttpGet("get-presentation/{presentationId}")]
    public string GetPresentation(int presentationId)
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
            return JsonSerializer.Serialize(new GetPresentationResponse
            {
                status_code = 0,
                presentation = Core.GetPresentation(new Domain.Entities.Presentation
                {
                    Id = presentationId
                })
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpPost("update-presentation")]
    public string UpdatePresentation()
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        UpdatePresentationValidation body;
        try
        {
            body = DecodePayloadJson<UpdatePresentationValidation>();
        }
        catch (Exception)
        {
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            switch (Core.CheckEntityOwnership(body.presentation_id, GetUserId()))
            {
                case false:
                    return ResponseToJson(AuthorizationFailedResponse());
            }
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        try
        {
            Core.UpdatePresentation(new Domain.Entities.Presentation
            {
                Id = body.presentation_id,
                End = DateTime.UnixEpoch.AddSeconds(body.end),
                Start = DateTime.UnixEpoch.AddSeconds(body.start)
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-excel-report/{fromDate}/{toDate}")]
    public IActionResult GetExcelFile(int fromDate, int toDate)
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return StatusCode(401, ResponseToJson(AuthenticationFailedResponse()));
        }

        try
        {
            var excel = Core.GenerateExcel(DateTime.UnixEpoch.AddSeconds(fromDate),
                DateTime.UnixEpoch.AddSeconds(toDate),
                GetUserId());
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = "report.xlsx",
                Inline = false,
            };

            return File(excel.GetByte(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        catch (Exception)
        {
            return StatusCode(500, ResponseToJson(InternalErrorResponse()));
        }
    }

    [HttpGet("get-pdf-report/{fromDate}/{toDate}")]
    public IActionResult GetPdfFile(int fromDate, int toDate)
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return StatusCode(401, ResponseToJson(AuthenticationFailedResponse()));
        }

        try
        {
            var pdf = Core.GeneratePdf(DateTime.UnixEpoch.AddSeconds(fromDate),
                DateTime.UnixEpoch.AddSeconds(toDate),
                GetUserId());
            return File(pdf.GetPdfBytes(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        catch (Exception)
        {
            return StatusCode(500, ResponseToJson(InternalErrorResponse()));
        }
    }

    private class GetPresentationResponse : Response
    {
        public Domain.Entities.Presentation presentation { get; set; }
    }

    private class GetPresentationsListRangeResponse : Response
    {
        public List<Domain.Entities.Presentation> presentations { get; set; }
    }
}