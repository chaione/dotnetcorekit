// -----------------------------------------------------------------------
// <copyright file="PeopleProfile.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.AutoMapperProfiles
{
    using AutoMapper;
    using DotNetCoreKit.Models.Domain;
    using DotNetCoreKit.Models.Dto;
    using FluentValidations;

    /// <inheritdoc />
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