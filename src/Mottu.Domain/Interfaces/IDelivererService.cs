using Mottu.Domain.Dtos;

namespace Mottu.Domain.Interfaces
{
    public interface IDelivererService
    {
        Task CreateDelivererAsync(DelivererDto delivererDto);
        Task ChangeCnhImage(string id, string imagemCnh);
    }
}
