// -----------------------------------------------------------------------
// <copyright file="ListCountValidator.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.FluentValidations.Extensions.Helpers
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
            var list = context.PropertyValue as IList<T>;

            if (list == null || list.Count < Max)
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("MaxElements", Max);
            return false;
        }
    }
}