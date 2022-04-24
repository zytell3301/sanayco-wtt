using System.Security.Cryptography;
using GrpcService1.App.Core.Users;

namespace GrpcService1.App.TokenGenerator;

public class TokenGenerator : ITokenGenerator
{
    private readonly int TokenLength;
    private readonly string ValidCharacters;

    public class TokenGeneratorConfigs
    {
        public int TokenLength;
        public string ValidaCharacters;
    }

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
        string randomString = "";
        for (var i = 0; i < TokenLength; i++)
        {
            randomString += ValidCharacters[RandomNumberGenerator.GetInt32(0, ValidCharacters.Length)];
        }

        return randomString;
    }
}