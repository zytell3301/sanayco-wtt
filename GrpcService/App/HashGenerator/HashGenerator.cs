#region

using ErrorReporter;
using GrpcService1.App.Core.Users;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.HashGenerator;

public class HashGenerator : IHash
{
    private readonly IErrorReporter ErrorReporter;
    private readonly int HashCost;

    private readonly InternalError InternalError;

    public HashGenerator(HashGeneratorConfigs configs, HashGeneratorDependencies dependencies)
    {
        ErrorReporter = dependencies.ErrorReporter;

        HashCost = configs.HashCost;

        InternalError = new InternalError(configs.InternalErrorMessage);
    }

    public string Hash(string expression)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(expression, HashCost);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public bool VerifyHash(string original, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(hash, original);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public class HashGeneratorConfigs
    {
        public int HashCost;
        public string InternalErrorMessage;
    }

    public class HashGeneratorDependencies
    {
        public IErrorReporter ErrorReporter;
    }
}