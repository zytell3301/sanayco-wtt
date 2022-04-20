using System.Security.Cryptography;

namespace GrpcService1.App.TokenGenerator;

public class TokenGenerator
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
}