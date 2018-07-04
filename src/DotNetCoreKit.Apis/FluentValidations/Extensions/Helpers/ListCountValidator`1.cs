// -----------------------------------------------------------------------
// <copyright file="ListCountValidator`1.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.FluentValidations.Extensions.Helpers
{
    using System.Collections.Generic;

    using FluentValidation.Validators;

    /// <summary>
    /// The list count validator.
    /// </summary>
    /// <typeparam name="T"> A generic type representation.
    /// </typeparam>
    public class ListCountValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCountValidator{T}"/> class.
        /// </summary>
        /// <param name="max">
        /// The max.
        /// </param>
        public ListCountValidator(int max)
            : base("{PropertyName} must contain fewer than {MaxElements} items.")
            {
            Max = max;
        }

        /// <summary>
        /// Gets the _max.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is IList<T> list) || list.Count < Max)
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("MaxElements", Max);
            return false;
        }
    }
}