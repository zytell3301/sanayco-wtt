using GrpcService1.Domain.Entities;

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
}