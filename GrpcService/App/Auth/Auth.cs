using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ErrorReporter;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Microsoft.IdentityModel.Tokens;

namespace GrpcService1.App.Auth;

public class Auth : Core.Users.IAuth, Handlers.Http.IAuth
{
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;
    private readonly authConfigs Configs;


    public class AuthDependencies
    {
        public IErrorReporter ErrorReporter;
        public InternalError InternalError;
    }

    private class authConfigs
    {
        public authConfigs(AuthConfigs configs)
        {
            Issuer = configs.Issuer;
            Audience = configs.Audience;
            SecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configs.Key));
            SecurityAlgorithm = configs.SecurityAlgorithm;
        }

        public readonly string Issuer;
        public readonly string Audience;
        public readonly SymmetricSecurityKey SecretKey;
        public readonly string SecurityAlgorithm;
    }

    public class AuthConfigs
    {
        public string Issuer;
        public string Audience;
        public string Key;
        public string SecurityAlgorithm;
    }

    public Auth(AuthConfigs configs, AuthDependencies dependencies)
    {
        ErrorReporter = dependencies.ErrorReporter;
        InternalError = dependencies.InternalError;

        Configs = new authConfigs(configs);
    }

    public string GenerateAuthToken(Token token, List<Permission> permissions)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("_token", token.Token1),
                    new Claim("permissions", JsonSerializer.Serialize(permissions)),
                }),
                Audience = Configs.Audience,
                Issuer = Configs.Issuer,
                SigningCredentials = new SigningCredentials(Configs.SecretKey, Configs.SecurityAlgorithm),
            };
            var authToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(authToken);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public string ExtractToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidIssuer = Configs.Issuer,
                ValidAudience = Configs.Audience,
                IssuerSigningKey = Configs.SecretKey,
            }, out var _);
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            return securityToken.Claims.First(c => c.Type == "_token").Value;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }
}