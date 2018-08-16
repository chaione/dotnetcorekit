// -----------------------------------------------------------------------
// <copyright file="PersonValidator.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions
{
    using DotNetCoreKit.Abstractions.Extensions;
    using DotNetCoreKit.Abstractions.InputDtos;
    using FluentValidation;

    // TODO: Extract this to a new project so that we can have a simpler project with just models, input and output dto's

    /// <inheritdoc />
    /// <summary>
    /// The person fluentvalidator.
    /// </summary>
    public class PersonValidator : AbstractValidator<Person>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonValidator"/> class.
        /// </summary>
        public PersonValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Age).InclusiveBetween(18, 60);
            RuleFor(person => person.Pets).ListMustContainFewerThan(10);
        }
    }
}