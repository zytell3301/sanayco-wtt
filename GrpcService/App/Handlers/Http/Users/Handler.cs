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
            var token = Core.Login(new User
            {
                Username = body.username
            }, body.password);

            return JsonSerializer.Serialize(new LoginResponse
            {
                status_code = 0,
                token = token
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpPost("/record-user")]
    public string RecordUser([FromForm] string data, [FromForm] IFormFile profile_picture)
    {
        try
        {
            Authorize("create-user");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordUserValidation body;
        try
        {
            body = JsonSerializer.Deserialize<RecordUserValidation>(data);
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
            }, ParsePermissionsArray(body.permissions), profile_picture);
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
            Core.DeleteUser(new User
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

    [HttpGet("/get-user/{username}")]
    public string GetUser(string username)
    {
        try
        {
            Authorize("edit-user");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        try
        {
            var user = Core.GetUserByUsername(new User
            {
                Username = username
            });
            var permissions = Core.GetUserPermissions(user);
            return JsonSerializer.Serialize(new GetUserResponse(user, permissions));
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpPost("/update-user")]
    public string UpdateUser()
    {
        try
        {
            Authorize("edit-user");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateUserValidation body;
        try
        {
            body = DecodePayloadJson<UpdateUserValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        try
        {
            Core.UpdateUser(new User
            {
                Id = body.user_id,
                Name = body.name,
                Username = body.username,
                CompanyLevel = body.company_level,
                LastName = body.lastname,
                SkillLevel = body.skill_level
            }, ParseEditPermissionArray(body.permissions, body.user_id));
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private List<Permission> ParseEditPermissionArray(string[] titles, int userId)
    {
        var permissions = ParsePermissionsArray(titles);
        foreach (var permission in permissions) permission.UserId = userId;

        return permissions;
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

    private class GetUserResponse : Response
    {
        public GetUserResponse(Domain.Entities.User user, List<Domain.Entities.Permission> permissions)
        {
            this.user = new User
            {
                id = user.Id,
                company_level = user.CompanyLevel,
                lastname = user.LastName,
                name = user.Name,
                skill_level = user.SkillLevel,
                username = user.Username
            };
            foreach (var permission in permissions)
                this.permissions.Add(new Permission
                {
                    title = permission.Title
                });
        }

        public User user { get; }
        public List<Permission> permissions { get; } = new();

        public class User
        {
            public int id { get; set; }
            public string company_level { get; set; }
            public string lastname { get; set; }
            public string name { get; set; }
            public string skill_level { get; set; }
            public string username { get; set; }
        }

        public class Permission
        {
            public string title { get; set; }
        }
    }

    private class LoginResponse : Response
    {
        public string token { get; set; }
        public DateTime expiration_date { get; set; }
    }
}