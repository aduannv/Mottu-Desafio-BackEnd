using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;
using Mottu.Infrastructure.DbContext.Models;
using System.Data;

namespace Mottu.Application.Services;

public class MotorcycleService(AppDbContext context,
                         IMapper mapper,
                         ICreateMotorcyclePublisher createMotorcyclePublisher) : IMotorcycleService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly ICreateMotorcyclePublisher _createMotorcyclePublisher = createMotorcyclePublisher;

    public async Task CreateMotorcycleAsync(MotorcycleDto motorcycleDto)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        try
        {
            var motorcycleDb = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Identificador == motorcycleDto.Identificador);
            if (motorcycleDb is not null)
                throw new Exception("Moto já cadastrada com este identificador.");

            var motorcycleByPlaca = await GetMotorcyclesAsync(motorcycleDto.Placa);
            if (motorcycleByPlaca is not null && motorcycleByPlaca.Any())
                throw new Exception("Moto já cadastrada com esta placa.");

            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();

            await _createMotorcyclePublisher.Publish(motorcycle);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<MotorcycleDto>> GetMotorcyclesAsync(string? placa = null)
    {
        var motorcyclesDb = placa == null
            ? await _context.Motorcycles.ToListAsync()
            : await _context.Motorcycles.Where(m => m.Placa == placa).ToListAsync()
            ?? throw new KeyNotFoundException("Moto não encontrada");

        return _mapper.Map<List<MotorcycleDto>>(motorcyclesDb);
    }

    public async Task UpdateMotorcyclePlacaAsync(string id, string placa)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        try
        {
            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Identificador == id)
                ?? throw new KeyNotFoundException("Dados inválidos");

            motorcycle.Placa = placa;
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<MotorcycleDto?> GetMotorcycleByIdAsync(string id)
    {
        var motorcycleDb = await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Identificador == id)
            ?? throw new KeyNotFoundException("Moto não encontrada");

        return _mapper.Map<MotorcycleDto>(motorcycleDb);
    }

    public async Task DeleteMotorcycleAsync(string id)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        try
        {
            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Identificador == id)
                ?? throw new KeyNotFoundException("Dados inválidos");

            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}