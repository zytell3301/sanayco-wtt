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
    private IPermissionsSource PermissionsSource;

    public BaseHandler(ITokenSource tokenSource, IPermissionsSource permissionsSource,
        AuthenticationFailed authenticationFailed,
        AuthorizationFailed authorizationFailed)
    {
        TokenSource = tokenSource;
        AuthenticationFailed = authenticationFailed;
        AuthorizationFailed = authorizationFailed;
        PermissionsSource = permissionsSource;
    }

    private string ParsePayload()
    {
        return new StreamReader(Request.BodyReader.AsStream()).ReadToEnd();
    }

    protected T DecodePayloadJson<T>()
    {
        return JsonSerializer.Deserialize<T>(ParsePayload());
    }

    protected int Authenticate()
    {
        try
        {
            return TokenSource.GetTokenUserId(Request.Headers["auth-token"]);
        }
        catch (Exception e)
        {
            throw AuthenticationFailed;
        }
    }

    protected int Authorize(string permission)
    {
        try
        {
            int userId = Authenticate();
            switch (PermissionsSource.CheckPermission(userId, permission))
            {
                case false:
                    throw AuthorizationFailed;
            }

            return userId;
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
}