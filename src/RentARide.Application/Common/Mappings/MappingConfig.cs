using Mapster;
using RentARide.Application.Common.Models;
using RentARide.Domain.Entities;

namespace RentARide.Application.Common.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Vehicle, VehicleDto>()
            .Map(dest => dest.VehicleType, src => src.VehicleType.Name)
            .Map(dest => dest.Status, src => src.Status.ToString());
    }
}
