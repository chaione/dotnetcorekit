namespace DotNetCoreKit.FluentValidations
{
    using System.Collections.Generic;

    using DotNetCoreKit.FluentValidations.Extensions;

    using FluentValidation;

    //TODO: Extract this to a new project so that we can have a simpler project with just models, input and output dto's
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<int> Pets { get; set; }
    }

    /// <summary>
    /// The person fluentvalidator.
    /// </summary>
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
            this.RuleFor(x => x.Name).Length(0, 10);
            this.RuleFor(x => x.Email).EmailAddress();
            this.RuleFor(x => x.Age).InclusiveBetween(18, 60);
            this.RuleFor(person => person.Pets).ListMustContainFewerThan(10);
        }
    }
}
