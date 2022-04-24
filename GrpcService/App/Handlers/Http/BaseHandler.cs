#region

using System.Text.Json;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http;

public class BaseHandler : ControllerBase
{
    private ITokenSource TokenSource;
    private AuthenticationFailed AuthenticationFailed;

    public BaseHandler(ITokenSource tokenSource, AuthenticationFailed authenticationFailed)
    {
        TokenSource = tokenSource;
        AuthenticationFailed = authenticationFailed;
    }

    // This is the response that will be returned if the client is sending invalid data
    protected const string InvalidRequestResponse = "";

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
}