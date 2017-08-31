namespace DotNetCoreKit.Validators.Extensions
{
    using System.Collections.Generic;

    using DotNetCoreKit.Validators.Extensions.Helpers;

    using FluentValidation;

    public static class ValidatiorExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder,
            int num) => ruleBuilder.SetValidator(new ListCountValidator<TElement>(num));
    }
}
