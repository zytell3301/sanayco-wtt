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
    private readonly IPermissionsSource PermissionsSource;
    private readonly ITokenSource TokenSource;
    private readonly Dictionary<string, Response> Responses = new();

    private int? UserId;

    public BaseHandler(BaseHandlerDependencies baseHandlerDependencies)
    {
        TokenSource = baseHandlerDependencies.TokenSource;
        AuthenticationFailed = baseHandlerDependencies.AuthenticationFailed;
        AuthorizationFailed = baseHandlerDependencies.AuthorizationFailed;
        PermissionsSource = baseHandlerDependencies.PermissionsSource;

        InitializeResponses();
    }

    private void InitializeResponses()
    {
        Responses["OperationSuccessfulResponse"] = new Response
        {
            status_code = 0
        };
        Responses["InternalErrorResponse"] = new Response
        {
            status_code = 1
        };
        Responses["DataValidationFailedResponse"] = new Response
        {
            status_code = 2
        };
        Responses["AuthorizationFailedResponse"] = new Response
        {
            status_code = 3
        };
        Responses["AuthenticationFailedResponse"] = new Response
        {
            status_code = 4
        };
    }

    protected Response DataValidationFailedResponse()
    {
        return Responses["DataValidationFailedResponse"];
    }

    protected Response InternalErrorResponse()
    {
        return Responses["InternalErrorResponse"];
    }

    protected Response AuthorizationFailedResponse()
    {
        return Responses["AuthorizationFailedResponse"];
    }

    protected Response AuthenticationFailedResponse()
    {
        return Responses["AuthenticationFailedResponse"];
    }

    protected Response OperationSuccessfulResponse()
    {
        return Responses["OperationSuccessfulResponse"];
    }

    protected string ResponseToJson(Response response)
    {
        return response.GetBufferedJson();
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
        catch (Exception)
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
        catch (AuthenticationFailed)
        {
            throw AuthenticationFailed;
        }
        catch (Exception)
        {
            throw AuthorizationFailed;
        }
    }

    protected class Response
    {
        private string JsonBuffer = "";

        public int status_code { get; set; }

        // This method returns json serialized string of current instance. If you are serializing classes that are static like status responses, consider
        // using GetBufferedJson.
        public string FreshSerialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // This method buffers the result json of class. So it is not reasonable to use it for classes that are going to be serialized once or changing constantly.
        // In case that your class changed after serializing and again call this method, you will get previous class json. Consider using FreshSerialize method instead.
        public string GetBufferedJson()
        {
            switch (JsonBuffer == "")
            {
                case true:
                    JsonBuffer = FreshSerialize();
                    break;
            }

            return JsonBuffer;
        }

        // Refreshes buffered json. Please read GetBufferedJson document before using these features.
        public void RefreshBuffer()
        {
            JsonBuffer = FreshSerialize();
        }
    }
}