#region

using System.Security.Cryptography;
using GrpcService1.App.Core.Users;

#endregion

namespace GrpcService1.App.TokenGenerator;

public class TokenGenerator : ITokenGenerator
{
    private readonly int TokenLength;
    private readonly string ValidCharacters;

    public TokenGenerator(TokenGeneratorConfigs configs)
    {
        TokenLength = configs.TokenLength;
        ValidCharacters = configs.ValidaCharacters;
    }

    public string GenerateToken()
    {
        return generateToken();
    }

    private string generateToken()
    {
        var randomString = "";
        for (var i = 0; i < TokenLength; i++)
            randomString += ValidCharacters[RandomNumberGenerator.GetInt32(0, ValidCharacters.Length)];

        return randomString;
    }

    public class TokenGeneratorConfigs
    {
        public int TokenLength;
        public string ValidaCharacters;
    }
}