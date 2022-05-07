namespace GrpcService1.App.Core.Users;

public interface IProfilePicturesStorage
{
    public string StoreProfilePicture(IFormFile file);
}