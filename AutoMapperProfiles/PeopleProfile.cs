// -----------------------------------------------------------------------
// <copyright file="PeopleProfile.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.AutoMapperProfiles
{
    using AutoMapper;
    using DomainModels;
    using Dto;
    using FluentValidations;

    /// <summary>
    /// The people profile.
    /// </summary>
    public class PeopleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleProfile"/> class.
        /// </summary>
        public PeopleProfile()
        {
            CreateMap<Person, PeopleDto>();
            CreateMap<PeopleDto, People>();
        }
    }
}