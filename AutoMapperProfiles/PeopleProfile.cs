namespace DotNetCoreKit.AutoMapperProfiles
{
    using AutoMapper;
    using DotNetCoreKit.FluentValidations;
    using DotNetCoreKit.Dto;
    using DotNetCoreKit.Models;

    public class PeopleProfile : Profile
    {
        public PeopleProfile()
        {
            CreateMap<Person, PeopleDto>();
            CreateMap<PeopleDto, People>();

        }
    }
}