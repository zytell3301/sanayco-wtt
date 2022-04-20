using ErrorReporter;
using GrpcService1.App.Core.Users;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.HashGenerator;

public class HashGenerator : IHash
{
    private readonly int HashCost;

    private InternalError InternalError;

    private IErrorReporter ErrorReporter;

    public class HashGeneratorConfigs
    {
        public int HashCost;
        public string InternalErrorMessage;
    }

    public class HashGeneratorDependencies
    {
        public IErrorReporter ErrorReporter;
    }

    public HashGenerator(HashGeneratorConfigs configs, HashGeneratorDependencies dependencies)
    {
        ErrorReporter = dependencies.ErrorReporter;

        HashCost = configs.HashCost;

        InternalError = new InternalError(configs.InternalErrorMessage);
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