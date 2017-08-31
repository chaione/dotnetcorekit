namespace DotNetCoreKit.FluentValidations.Extensions.Helpers
{
    using System.Collections.Generic;

    using FluentValidation.Validators;

    public class ListCountValidator<T> : PropertyValidator
    {
        private readonly int _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCountValidator{T}"/> class.
        /// </summary>
        /// <param name="max">
        /// The max.
        /// </param>
        public ListCountValidator(int max)
            : base("{PropertyName} must contain fewer than {MaxElements} items.")
        {
            this._max = max;
        }

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

            if (list == null || list.Count < this._max) { return true; }

            context.MessageFormatter.AppendArgument("MaxElements", this._max);
            return false;
        }
    }
}