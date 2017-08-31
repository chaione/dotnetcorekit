namespace dotnetcorekit.AutoMapperProfiles
{
    using AutoMapper;
    using DotNetCoreKit.FluentValidations;
    using DotNetCoreKit.Dto;

    public class PeopleProfile : Profile
    {
        public PeopleProfile() => CreateMap<Person, People>();
    }
}