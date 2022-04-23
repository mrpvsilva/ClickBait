using AutoMapper;

namespace ClickBait.Infra.Mapper
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Entities.Post, Entities.Post>()
                .ReverseMap();

            CreateMap<Domain.Entities.ClickCount, Entities.ClickCount>()
                .ReverseMap();
        }
    }
}
