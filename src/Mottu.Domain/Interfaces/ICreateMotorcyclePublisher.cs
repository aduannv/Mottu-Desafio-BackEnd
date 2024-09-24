using Mottu.Infrastructure.DbContext.Models;

namespace Mottu.Domain.Interfaces
{
    public interface ICreateMotorcyclePublisher
    {
        Task Publish(Motorcycle motorcycle);
    }
}
