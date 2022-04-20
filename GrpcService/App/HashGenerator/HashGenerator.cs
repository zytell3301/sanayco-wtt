using ErrorReporter;
using GrpcService1.App.Core.Users;

namespace GrpcService1.App.HashGenerator;

public class HashGenerator : IHash
{
    private readonly int HashCost;

    private IErrorReporter ErrorReporter;

    public class HashGeneratorConfigs
    {
        public int HashCost;
    }

    public class HashGeneratorDependencies
    {
        public IErrorReporter ErrorReporter;
    }

    public HashGenerator(HashGeneratorConfigs configs, HashGeneratorDependencies dependencies)
    {
        ErrorReporter = dependencies.ErrorReporter;

        HashCost = configs.HashCost;
    }

    public string Hash(string expression)
    {
        throw new NotImplementedException();
    }

    public bool VerifyHash(string hash, string original)
    {
        throw new NotImplementedException();
    }
}