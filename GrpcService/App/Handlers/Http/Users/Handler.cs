#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.Users.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.Users;

public class Handler : BaseHandler
{
    private readonly Core.Users.Core Core;

    public Handler(Core.Users.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
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
            var token = Core.Login(new User
            {
                Username = body.username
            }, body.password);

            return JsonSerializer.Serialize(new LoginResponse
            {
                status_code = 0,
                token = token.Token1,
                expiration_date = token.ExpirationDate.Value
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpPost("/record-user")]
    public string RecordUser()
    {
        try
        {
            Authorize("register-user");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordUserValidation body;
        try
        {
            body = DecodePayloadJson<RecordUserValidation>();
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
            Core.Register(new User
            {
                Name = body.name,
                Password = body.password,
                Username = body.username,
                CompanyLevel = body.company_level,
                LastName = body.lastname,
                SkillLevel = body.skill_level
            }, ParsePermissionsArray(body.permissions));
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("/delete-user")]
    public string DeleteUser()
    {
        try
        {
            Authorize("delete-user");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        DeleteUserValidation body;

        try
        {
            body = DecodePayloadJson<DeleteUserValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        try
        {
            Core.DeleteUser(new User()
            {
                Username = body.username
            });
        }
        catch (Exception)
        {
            ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private List<Permission> ParsePermissionsArray(string[] titles)
    {
        var permissions = new List<Permission>();
        foreach (var title in titles)
            permissions.Add(new Permission
            {
                Title = title,
                GrantedBy = GetUserId() // User id field will be set in core
            });

        return permissions;
    }

    private class LoginResponse : Response
    {
        public string token { get; set; }
        public DateTime expiration_date { get; set; }
    }
}