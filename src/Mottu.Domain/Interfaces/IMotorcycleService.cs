using Mottu.Domain.Dtos;

namespace Mottu.Domain.Interfaces
{
    public interface IMotorcycleService
    {
        Task CreateMotorcycleAsync(MotorcycleDto dto);
        Task<IEnumerable<MotorcycleDto>> GetMotorcyclesAsync(string? placa = null);
        Task<MotorcycleDto?> GetMotorcycleByIdAsync(string id);
        Task UpdateMotorcyclePlacaAsync(string id, string placa);
        Task DeleteMotorcycleAsync(string id);
    }
}
