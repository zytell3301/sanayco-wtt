#region

using ErrorReporter;
using GrpcService1.App.Core.Users;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.ProfilePicturesStorage;

public class ProfilePicturesStorage : IProfilePicturesStorage
{
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;
    private readonly string PicturesRootDirectory;
    private readonly ITokenGenerator TokenGenerator;

    public ProfilePicturesStorage(ProfilePicturesStorageDependencies dependencies,
        ProfilePicturesStorageConfigs configs)
    {
        TokenGenerator = dependencies.TokenGenerator;
        ErrorReporter = dependencies.ErrorReporter;

        PicturesRootDirectory = configs.PicturesRootDirectory;

        InternalError = new InternalError(configs.InternalErrorMessage);
    }

    public string StoreProfilePicture(IFormFile file)
    {
        // try
        // {
        file.CopyTo(File.Create(PicturesRootDirectory + ""));
        return file.FileName;
        // }
        // catch (Exception e)
        // {
        // ErrorReporter.ReportException(e);
        // throw InternalError;
        // }
    }

    public class ProfilePicturesStorageDependencies
    {
        public IErrorReporter ErrorReporter;
        public ITokenGenerator TokenGenerator;
    }

    public class ProfilePicturesStorageConfigs
    {
        public string InternalErrorMessage;

        public string PicturesRootDirectory;
    }
}