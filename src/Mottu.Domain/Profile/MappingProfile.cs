using AutoMapper;
using Mottu.Domain.Dtos;
using Mottu.Domain.Events;
using Mottu.Infrastructure.DbContext.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MotorcycleDto, Motorcycle>();
        CreateMap<Motorcycle, MotorcycleDto>();
        CreateMap<DelivererDto, Deliverer>();
        CreateMap<Deliverer, DelivererDto>();
        CreateMap<CreateRentalDto, Rental>();
        CreateMap<Rental, CreateRentalDto>();
        CreateMap<CreateMotorcycleEvent, BrandNewMotorcycle>();
    }
}