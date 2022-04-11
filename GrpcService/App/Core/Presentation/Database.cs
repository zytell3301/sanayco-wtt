#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Presentation;

/**
 * Dependency interface of presentation core.
 * Every action from presentation core to database
 * must be done via this interface.
 */
public interface IDatabase
{
    public void RecordPresentation(User user);
    public void RecordPresentationEnd(User user);

    /*
     * Current application of this api for now is for calculating user's presentation time.
     * If this will be the only application, you can consider adding a UserIsNotRepresent exception
     * for the case that the user has ended his representation and currently not present.
     */
    public DateTime GetPresentationTime(User user);
}