// -----------------------------------------------------------------------
// <copyright file="ValidatorExtensions.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.FluentValidations.Extensions
{
    using System.Collections.Generic;

    using FluentValidation;

    using Helpers;

    /// <summary>
    /// The validator extensions.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// The list must contain fewer than.
        /// </summary>
        /// <typeparam name="T">
        /// A generic type object.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// Rule type representation.
        /// </typeparam>
        /// <param name="ruleBuilder">
        /// The rule builder.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="IRuleBuilderOptions{T,TProperty}"/>.
        /// </returns>
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder,
            int number)
            {
            return ruleBuilder.SetValidator(new ListCountValidator<TElement>(number));
        }
    }
}