using System.Text.Json;
using GrpcService1.App.Handlers.Http.Users.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Users;

public class Handler : BaseHandler
{
    private Core.Users.Core Core;

    public Handler(Core.Users.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    private class LoginResponse : Response
    {
        public string token { get; set; }
        public DateTime expiration_date { get; set; }
    }

    [HttpPost("/login")]
    public string Login()
    {
        LoginValidation body;
        try
        {
            body = DecodePayloadJson<LoginValidation>();
        }
        catch (Exception e)
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
            var token = Core.Login(new User()
            {
                Username = body.username,
            }, body.password);

            return JsonSerializer.Serialize(new LoginResponse()
            {
                status_code = 0,
                token = token.Token1,
                expiration_date = token.ExpirationDate.Value,
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }
}