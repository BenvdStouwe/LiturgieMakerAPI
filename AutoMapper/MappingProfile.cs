using AutoMapper;
using LiturgieMakerAPI.LiturgieMaker.Controllers;
using LiturgieMakerAPI.LiturgieMaker.Model;

namespace LiturgieMakerAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Liturgie, LiturgieDto>();
            CreateMap<LiturgieDto, Liturgie>();
        }
    }
}