using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Dtos;
using Mottu.Domain.Enums;
using Mottu.Domain.Interfaces;
using Mottu.Infrastructure.DbContext.Models;

namespace Mottu.Application.Services;

public class RentalService(AppDbContext context, IMapper mapper, IMotorcycleService motorcycleService) : IRentalService
{
    private const decimal PrecoDia7 = 30.00m;
    private const decimal PrecoDia15 = 28.00m;
    private const decimal PrecoDia30 = 22.00m;
    private const decimal PrecoDia45 = 20.00m;
    private const decimal PrecoDia50 = 18.00m;
    private const string ACCEPT_TYPE = "A";
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IMotorcycleService _motorcycleService = motorcycleService;

    public async Task CreateRentalAsync(CreateRentalDto rentalDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await Validations(rentalDto);
            var rental = _mapper.Map<Rental>(rentalDto);
            rental.Identificador = Guid.NewGuid().ToString();
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task Validations(CreateRentalDto rentalDto)
    {
        if (rentalDto.DataInicio >= DateTime.UtcNow.AddDays(1) ||
            rentalDto.DataTermino <= rentalDto.DataInicio ||
            rentalDto.DataPrevisaoTermino <= rentalDto.DataInicio)
            throw new ArgumentException("Datas de locação inválidas.");

        if (!await AcceptType(rentalDto.EntregadorId))
            throw new ArgumentException("Entregador não habilitado na categoria A.");

        _ = await _motorcycleService.GetMotorcycleByIdAsync(rentalDto.MotoId)
            ?? throw new ArgumentException("Moto não encontrada.");
    }

    public async Task<GetRentalDto> GetRentalAsync(string id)
    {
        var rental = await _context.Rentals
            .FirstOrDefaultAsync(r => r.Identificador == id) ?? throw new KeyNotFoundException("Locação não encontrada");

        var rentalDto = _mapper.Map<GetRentalDto>(rental);

        rentalDto.ValorDiaria = GetDailyRate((EPlanoLocacao)rental.Plano);

        return rentalDto;
    }

    public async Task ReturnRentalAsync(string id, ReturnRentalDto returnRental)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var rental = await _context.Rentals
                .FirstOrDefaultAsync(r => r.Identificador == id) 
                ?? throw new KeyNotFoundException("Locação não encontrada");

            rental.DataDevolucao = returnRental.DataDevolucao;
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private decimal GetDailyRate(EPlanoLocacao plano)
    {
        return (int)plano switch
        {
            7 => PrecoDia7,
            15 => PrecoDia15,
            30 => PrecoDia30,
            45 => PrecoDia45,
            50 => PrecoDia50,
            _ => throw new ArgumentException("Plano inválido.")
        };
    }

    private async Task<bool> AcceptType(string entregadorId)
    {
        var deliverer = await _context.Deliverers
            .FirstOrDefaultAsync(r => r.Identificador == entregadorId) ?? throw new ArgumentException("Entregador não encontrado.");

        return deliverer.TipoCnh.Contains(ACCEPT_TYPE);
    }

    // This part is commented because I was unsure of where to show the valor_total field, since it does not exist in the reference swagger.

    //private decimal CalcularValorTotal(CreateRentalDto rental)
    //{
    //    decimal valorDiaria = ObterPrecoDiaria(rental.Plano);
    //    int diasLocacao = (rental.DataTermino - rental.DataInicio).Days;
    //    decimal valorTotal = valorDiaria * diasLocacao;

    //    return valorTotal;
    //}

    //private decimal CalcularMulta(int plano, DateTime dataPrevisaoTermino, DateTime dataDevolucao)
    //{
    //    if (dataDevolucao < dataPrevisaoTermino)
    //    {
    //        int diasNaoUtilizados = (dataPrevisaoTermino - dataDevolucao).Days;
    //        decimal multa = (ObterPrecoDiaria(plano) * diasNaoUtilizados) * ObterPorcentagemMulta(plano);
    //        return multa;
    //    }
    //    else if (dataDevolucao > dataPrevisaoTermino)
    //    {
    //        int diasAdicionais = (dataDevolucao - dataPrevisaoTermino).Days;
    //        return diasAdicionais * 50.00m; // R$50,00 por diária adicional
    //    }

    //    return 0; // Sem multa
    //}

    //private decimal ObterPorcentagemMulta(int plano)
    //{
    //    return plano switch
    //    {
    //        7 => 0.20m,
    //        15 => 0.40m,
    //        _ => 0 // Para planos de 30, 45 e 50 dias, não há multa
    //    };
    //}
}
