#region

using System.Text.Json;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http;

public class BaseHandler : ControllerBase
{
    // This is the response that will be returned if the client is sending invalid data
    protected const string InvalidRequestResponse = "";

    private readonly AuthenticationFailed AuthenticationFailed;
    private readonly AuthorizationFailed AuthorizationFailed;
    private readonly ITokenSource TokenSource;
    private readonly IPermissionsSource PermissionsSource;

    private int? UserId;

    public BaseHandler(BaseHandlerDependencies baseHandlerDependencies)
    {
        TokenSource = baseHandlerDependencies.TokenSource;
        AuthenticationFailed = baseHandlerDependencies.AuthenticationFailed;
        AuthorizationFailed = baseHandlerDependencies.AuthorizationFailed;
        PermissionsSource = baseHandlerDependencies.PermissionsSource;
    }

    protected Response DataValidationFailedResponse()
    {
        return new Response
        {
            status_code = 2
        };
    }

    protected Response InternalErrorResponse()
    {
        return new Response
        {
            status_code = 1
        };
    }

    protected Response AuthorizationFailedResponse()
    {
        return new Response
        {
            status_code = 3
        };
    }

    protected Response AuthenticationFailedResponse()
    {
        return new Response
        {
            status_code = 4
        };
    }

    protected Response OperationSuccessfulResponse()
    {
        return new Response
        {
            status_code = 0
        };
    }

    protected string ResponseToJson(Response response)
    {
        return JsonSerializer.Serialize(response);
    }

    private string ParsePayload()
    {
        return new StreamReader(Request.BodyReader.AsStream()).ReadToEnd();
    }

    protected T DecodePayloadJson<T>()
    {
        return JsonSerializer.Deserialize<T>(ParsePayload());
    }

    protected int GetUserId()
    {
        switch (UserId.HasValue)
        {
            case false:
                Authenticate();
                break;
        }

        return UserId.Value;
    }

    protected void Authenticate()
    {
        try
        {
            UserId = TokenSource.GetTokenUserId(Request.Headers["_token"]);
        }
        catch (Exception e)
        {
            throw AuthenticationFailed;
        }
    }

    protected void Authorize(string permission)
    {
        try
        {
            switch (PermissionsSource.CheckPermission(GetUserId(), permission))
            {
                case false:
                    throw AuthorizationFailed;
            }
        }
        catch (AuthenticationFailed e)
        {
            throw AuthenticationFailed;
        }
        catch (Exception e)
        {
            throw AuthorizationFailed;
        }
    }

    protected class Response
    {
        public int status_code { get; set; }
    }
}