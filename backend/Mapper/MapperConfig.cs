using AutoMapper;
using backend.Dto.Size;
using backend.Entity;

namespace backend.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Size, SizeDto>();
    }

}
