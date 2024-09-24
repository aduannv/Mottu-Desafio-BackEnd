using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;
using Mottu.Infrastructure.DbContext.Models;
using System.Data;

namespace Mottu.Application.Services;

public class DelivererService(AppDbContext context,
                        IMapper mapper,
                        IFileService fileService) : IDelivererService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IFileService _fileService = fileService;

    public async Task CreateDelivererAsync(DelivererDto delivererDto)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        try
        {
            var existingDeliverer = await _context.Deliverers
                .Where(d => d.Identificador.Equals(delivererDto.Identificador) ||
                            d.Cnpj.Equals(delivererDto.Cnpj) ||
                            d.NumeroCnh.Equals(delivererDto.NumeroCnh))
                .FirstOrDefaultAsync();

            if (existingDeliverer is not null)
            {
                if (existingDeliverer.Identificador.Equals(delivererDto.Identificador))
                    throw new ArgumentException("Já existe um entregador com este Identificador.");

                if (existingDeliverer.Cnpj.Equals(delivererDto.Cnpj))
                    throw new ArgumentException("Já existe um entregador com este CNPJ.");

                if (existingDeliverer.NumeroCnh.Equals(delivererDto.NumeroCnh))
                    throw new ArgumentException("Já existe um entregador com este número de CNH.");
            }

            var deliverer = _mapper.Map<Deliverer>(delivererDto);
            await _context.Deliverers.AddAsync(deliverer);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(delivererDto.ImagemCnh))
                await _fileService.SaveCnhImageAsync(delivererDto.ImagemCnh, delivererDto.NumeroCnh);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ChangeCnhImage(string id, string cnhImage)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        try
        {
            var deliverer = await _context.Deliverers
                .FirstOrDefaultAsync(m => m.Identificador == id)
                ?? throw new KeyNotFoundException("Entregador não encontrado.");

            await _fileService.SaveCnhImageAsync(cnhImage, deliverer.NumeroCnh);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}