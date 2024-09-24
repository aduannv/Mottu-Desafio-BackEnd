using Mottu.Domain.Dtos;

namespace Mottu.Domain.Interfaces
{
    public interface IRentalService
    {
        Task CreateRentalAsync(CreateRentalDto rental);
        Task<GetRentalDto> GetRentalAsync(string id);
        Task ReturnRentalAsync(string id, ReturnRentalDto returnRental);
    }
}